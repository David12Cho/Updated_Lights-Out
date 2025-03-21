using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [System.Serializable]
    public struct CutsceneSlide
    {
        public Sprite image;
        [TextArea(3, 5)]
        public string[] dialogueLines; // Multiple text segments for the same image
        public float displayTime; // Time to display the slide if there is no dialogue
    }

    public Image cutsceneImage;
    public TMP_Text dialogueText;
    public TMP_Text theEndText; 
    public AudioClip biteSound; 
    public GameObject dialogueBox; // The UI panel for text
    public Button nextButton; // Button to skip typing effect & go to next text/image
    public Button skipButton;
    public float skipButtonDuration = 3f;
    public CutsceneSlide[] slides;
    public float textSpeed = 0.05f;

    private int currentSlideIndex = 0;
    private int currentTextIndex = 0;
    private bool isTyping = false;
    private string fullText;
    public Image fadePanel; // Assign a UI Panel's Image in Inspector
    public float fadeDuration = 1f; // Adjust fade speed
    private Coroutine typingCoroutine; // Hold the reference to the typing coroutine

    public AudioClip typingSound; // The sound effect for typing
    private AudioSource audioSource; // AudioSource to play the typing sound
    private bool hasPlayedBiteSound = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to the same GameObject
        nextButton.onClick.AddListener(OnNextButtonClick);
        // Add an EventTrigger to skip typing when clicking anywhere in the dialogue box
        AddClickListenerToDialogueBox();
        skipButton.gameObject.SetActive(false);
        StartCoroutine(ShowSkipButtonTemporarily());
        ShowSlide(0);
    }

    IEnumerator ShowSkipButtonTemporarily()
    {
        skipButton.gameObject.SetActive(true); // Show the skip button
        yield return new WaitForSeconds(skipButtonDuration);
        skipButton.gameObject.SetActive(false); // Hide the button after 3 seconds
    }

    public void SkipCutscene()
    {
        StopAllCoroutines();
        if (SceneManager.GetActiveScene().name == "Cutscene-1") {
            LevelManager.Instance.LoadScene("SampleScene", "CrossFade"); // Load next scene   
        } else if (SceneManager.GetActiveScene().name == "Cutscene-2") {
            LevelManager.Instance.LoadScene("Level 2 (Docks)", "CrossFade");
        } else if (SceneManager.GetActiveScene().name == "Cutscene-3") {
            LevelManager.Instance.LoadScene("Level 3 (Lighthouse)", "CrossFade");
        } else if (SceneManager.GetActiveScene().name == "Cutscene-4") {
            LevelManager.Instance.LoadScene("Level 4 (Boss)", "CrossFade");
        } else if (SceneManager.GetActiveScene().name == "Cutscene-5") {
            audioSource.Stop();
            StartCoroutine(PlayEndSequence());
        }
    }

    void OnNextButtonClick()
    {
        SkipTypingEffect();
    }

    // This method handles skipping typing effect
    void SkipTypingEffect()
    {
        if (isTyping)
        {
            // Stop the typing coroutine immediately
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine); // Stop the specific typing coroutine
            }

            // Set the full text immediately
            dialogueText.text = fullText;
            isTyping = false;

            // Stop the typing sound
            audioSource.Stop();
        }
        else
        {
            // Proceed to the next text or slide after the typing effect is complete
            NextTextOrSlide();
        }
    }

    // Adds a listener to detect clicks anywhere in the dialogue box
    void AddClickListenerToDialogueBox()
    {
        var pointerClick = dialogueBox.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        entry.callback.AddListener((eventData) => SkipTypingEffect()); // Calls SkipTypingEffect on click
        pointerClick.triggers.Add(entry);
    }

    void ShowSlide(int index)
    {
        if (index >= slides.Length)
        {
            EndCutscene();
            return;
        }

        currentSlideIndex = index;
        currentTextIndex = 0; // Reset text index for new slide
        
        // Handle the first slide with no fade transition
        if (index == 0)
        {
            cutsceneImage.sprite = slides[index].image;
            HandleTextAndTiming(index);
        }
        else
        {
            StartCoroutine(TransitionWithFade(index)); // Fade transition for all slides except the first one
        }
    }

    void HandleTextAndTiming(int index)
    {
        // Handle the dialogue box visibility and timing based on whether there are text lines
        if (slides[index].dialogueLines.Length > 0 && slides[index].dialogueLines[0] != "")
        {
            dialogueBox.SetActive(true); // Show the dialogue box if there's text
            ShowText(0);
        }
        else
        {
            dialogueBox.SetActive(false); // Hide the dialogue box if there's no text
            StartCoroutine(WaitAndShowNextSlide(index));
        }
    }

    IEnumerator WaitAndShowNextSlide(int index)
    {
        // If no dialogue is present, wait for the specified display time and then go to the next slide
        yield return new WaitForSeconds(slides[index].displayTime);
        ShowSlide(index + 1);
    }

    void ShowText(int textIndex)
    {
        if (textIndex >= slides[currentSlideIndex].dialogueLines.Length)
        {
            ShowSlide(currentSlideIndex + 1); // Proceed to next slide after all text is shown
            return;
        }

        fullText = slides[currentSlideIndex].dialogueLines[textIndex];
        dialogueText.text = "";
        typingCoroutine = StartCoroutine(TypeText(fullText));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;

        // Start playing the typing sound and set it to loop
        audioSource.clip = typingSound;
        audioSource.loop = true;
        audioSource.Play();

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        // Stop the typing sound once typing is finished
        audioSource.Stop();
        isTyping = false;
    }

    void NextTextOrSlide()
    {
        currentTextIndex++;

        if (currentTextIndex < slides[currentSlideIndex].dialogueLines.Length)
        {
            ShowText(currentTextIndex); // Show next dialogue segment
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "Cutscene-5" && currentSlideIndex == slides.Length - 1)
            {
                StartCoroutine(PlayEndSequence());
            }
            else
            {
                ShowSlide(currentSlideIndex + 1); // Move to next slide if not the final one
            }
        }
    }

    IEnumerator PlayEndSequence()
    {
        isTyping = true; 

        PlayBiteSound();

        fadePanel.gameObject.SetActive(true);
        yield return StartCoroutine(FadeToBlack(true));

        theEndText.gameObject.SetActive(true);

        // Keep "The End" displayed for a few seconds before quitting or returning to the main menu
        yield return new WaitForSeconds(3f);

        LevelManager.Instance.LoadScene("Menu", "CrossFade");
    }

    void PlayBiteSound()
    {
        if (!hasPlayedBiteSound && biteSound != null)
        {
            audioSource.PlayOneShot(biteSound); // Play sound once
            hasPlayedBiteSound = true; // Mark it as played
        }
    }

    void EndCutscene()
    {
        // Transition to the next scene or return to gameplay
        if (SceneManager.GetActiveScene().name == "Cutscene-1") 
        {
            LevelManager.Instance.LoadScene("SampleScene", "CrossFade");
        } else if (SceneManager.GetActiveScene().name == "Cutscene-2") {
            LevelManager.Instance.LoadScene("Level 2 (Docks)", "CrossFade");    
        } else if (SceneManager.GetActiveScene().name == "Cutscene-3") {
            LevelManager.Instance.LoadScene("Level 3 (Lighthouse)", "CrossFade");    
        } else if (SceneManager.GetActiveScene().name == "Cutscene-4") {
            LevelManager.Instance.LoadScene("Level 4 (Boss)", "CrossFade");    
        } 
    }

    IEnumerator TransitionWithFade(int index)
    {
        fadePanel.gameObject.SetActive(true); // Enable fade panel before transition
        yield return StartCoroutine(FadeToBlack(true)); // Fade to black before switching

        cutsceneImage.sprite = slides[index].image; // Change the image

        HandleTextAndTiming(index); // Handle text or wait for the next slide

        yield return StartCoroutine(FadeToBlack(false)); // Fade back in after switching
        fadePanel.gameObject.SetActive(false); // Disable fade panel after transition
    }

    IEnumerator FadeToBlack(bool fadeIn)
    {
        float elapsedTime = 0f;
        float startAlpha = fadeIn ? 0f : 1f; // Fade in starts from alpha 0, fade out starts from alpha 1
        float endAlpha = fadeIn ? 1f : 0f;   // Fade in ends at alpha 1, fade out ends at alpha 0

        // Your custom color: #1C1D1F
        Color customColor = new Color(28f / 255f, 29f / 255f, 31f / 255f); // Convert hex to normalized RGB values (0-1 scale)

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadePanel.color = new Color(customColor.r, customColor.g, customColor.b, alpha); // Keep RGB, change alpha
            yield return null;
        }

        // Ensure the final alpha value is set correctly
        fadePanel.color = new Color(customColor.r, customColor.g, customColor.b, endAlpha);
    }
}
