using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthDisplay : MonoBehaviour
{
    public List<Image> hearts; // Manually assign heart images
    public Sprite emptyHeartSprite; // Assign the empty heart sprite in the Inspector
    public Sprite fullHeartSprite; // Assign the full heart sprite in the Inspector

    private int lives = 5; // Starting lives

    public void LoseLife()
    {
        if (lives > 0)
        {
            lives--; // Reduce life count
            hearts[lives].sprite = emptyHeartSprite; // Change full heart to empty heart
            hearts[lives].rectTransform.sizeDelta = new Vector2(22, 19); // Set size to 22x19
            
        }
    }

    public void GainLife()
    {
        if (lives < hearts.Count) // Ensure we don’t go above the max heart slots
        {
            hearts[lives].sprite = fullHeartSprite; // Replace empty heart with full heart
            hearts[lives].rectTransform.sizeDelta = new Vector2(25, 25);
            lives++; // Increase life count
        }
    }
}
