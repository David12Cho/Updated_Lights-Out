using UnityEngine;
using System.Collections;

public class LightBulbController : MonoBehaviour
{
    public GameObject[] lights;


    private float duration = 0f;
    public float darkInterval = 4.5f;
    public float lightInterval = 1.5f;
    private bool lightOn = false;
    
    void Start()
    {

    }

    void Update()
    {
        duration += Time.deltaTime;

            if(lightOn)
            {
                if (duration >= lightInterval)
                {
                    foreach (GameObject light in lights)
                    {
                        light.SetActive(false);
                    }
                    duration -= lightInterval;
                    lightOn = false;
                }
            }
            else
            {
                if(duration >= darkInterval)
                {
                    foreach (GameObject light in lights)
                    {
                        light.SetActive(true);
                    }
                    duration -= darkInterval;
                    lightOn = true;
                }
            }

    }
}
