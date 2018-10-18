using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class LaunchCinematic : MonoBehaviour
    { 
        private Animator pressurPlateAnimator;
        private AudioSource audioSource;

        // Sounds Names
        private string PLATE_ARRAY = "items_pressure_plates";
        private string PLATE_ACTIVATED = "PRESSURE_PLATE_ACTIVATED";

        // Use this for initialization
        void Start()
        {
            pressurPlateAnimator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }


        public void PlateTriggerEnter(Collider col)
        {
            Debug.Log("OK");
            if (col.gameObject.tag == "Player")
            {
                AudioManager.Instance.PlaySfx3D(PLATE_ACTIVATED, PLATE_ARRAY, ref audioSource);
                SceneManager.LoadScene(2, LoadSceneMode.Additive);
            }
        }
    }
}
