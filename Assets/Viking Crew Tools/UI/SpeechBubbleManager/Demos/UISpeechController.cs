using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VikingCrewTools.UI;
using System.Linq;

namespace VikingCrewDevelopment.Demos {
    public class UISpeechController : MonoBehaviour {

        public InputField txtMessage;
        public SayRandomThingsBehaviour talkBehaviour;
        public ToggleGroup toggles;


        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void OnTalk() {

            talkBehaviour.SaySomething(txtMessage.text, (SpeechBubbleManager.SpeechbubbleType)toggles.ActiveToggles().First<Toggle>().transform.GetSiblingIndex());
        }
    }
}