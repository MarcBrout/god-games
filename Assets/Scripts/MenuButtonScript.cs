using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class MenuButtonScript : MonoBehaviour
    {

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
