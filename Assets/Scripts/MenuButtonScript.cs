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
            AudioManager.Instance.PlaySfx("button_hover", "menu");

        }

        public void PlayerClickSound()
        {
            AudioManager.Instance.PlaySfx("button_click", "menu");
        }
    }
}
