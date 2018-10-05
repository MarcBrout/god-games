using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class PressurplateColorScript : MonoBehaviour
    {

        private Color plateColor;
        public Color disabledPlateColor = Color.gray;


        // Use this for initialization
        void Start()
        {
            plateColor = GetComponent<Renderer>().material.GetColor("_Color");

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShowDisableColor()
        {
            GetComponent<Renderer>().material.SetColor("_Color", disabledPlateColor);
        }

        public void ShowEnableColor()
        {
            GetComponent<Renderer>().material.SetColor("_Color", plateColor);
        }
    }
}
