using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class MenuButtonScript : MonoBehaviour
    {
        AudioSource a;

        private void Start()
        {
            AudioSource a = GetComponent<AudioSource>();
        }

        public void PlayerHoverSound()
        {
            Debug.Log("Hover Sound");
            AudioManager.Instance.PlaySfx3D("button_hover", "menu", ref a);
        }

        public void PlayerClickSound()
        {
            AudioManager.Instance.PlaySfx3D("button_click", "menu", ref a);

        }
    }
}
