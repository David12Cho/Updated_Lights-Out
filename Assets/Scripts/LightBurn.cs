// using UnityEngine;

// public class LightBurn : MonoBehaviour
// {
//     public float revolutionDuration = 6f;  // Time (in seconds) for one full rotation

//     private bool playerWasExposed = false;
//     private float timer = 0f;

//     // Called when the player enters the light beam trigger
//     void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             FindObjectOfType<HealthDisplay>().LoseLife();
//         }
//     }

//     // When the player leaves, we can still remember they were hit this revolution.
//     // void OnTriggerExit(Collider other)
//     // {

//     // }

// }
