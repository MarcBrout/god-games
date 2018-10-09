using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace GodsGame
{
    public class ChangeControlMenu : MonoBehaviour
    {

        protected class ControlButton
        {
            public GameObject go;
            public Button button;
            public TextMeshProUGUI label { get; private set; }
            public string cInputLabel;
            TextMeshProUGUI buttonText;

            public ControlButton(GameObject controlButtonGo, bool player1 = true)
            {
                go = controlButtonGo;
                label = controlButtonGo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                if (player1)
                {
                    button = controlButtonGo.transform.GetChild(1).GetComponent<Button>();
                    buttonText = controlButtonGo.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
                }
                else
                {
                    button = controlButtonGo.transform.GetChild(2).GetComponent<Button>();
                    buttonText = controlButtonGo.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
                }
            }

            public void UpdateLabel(string newLabel)
            {
                label.text = newLabel;
            }

            public void UpdateButtonText(string newText)
            {
                buttonText.text = newText;
            }
        }

        private readonly string m_Player1Input = "P1";
        private readonly string m_Player2Input = "P2";
        private List<ControlButton> m_ControlButtonsP1;
        private List<ControlButton> m_ControlButtonsP2;

        public GameObject controlButtonPrefab;
        public GameObject controlMenuUI;
        public GameObject controlPanel;

        void Start()
        {
            m_ControlButtonsP1 = new List<ControlButton>();
            m_ControlButtonsP2 = new List<ControlButton>();
            for (int i = 0; i < cInput.length; ++i)
            {
                string cinputName = cInput.GetText(i);

                if (cinputName.Contains(m_Player1Input))
                {
                    GameObject go = Instantiate(controlButtonPrefab);
                    CreateControlButton(go, m_ControlButtonsP1, i);
                }
                else if (cinputName.Contains(m_Player2Input))
                {
                    CreateControlButton(m_ControlButtonsP1.Find(x => { return x.label.text.Contains(cinputName.Truncate(cinputName.Length - 3)); }).go, m_ControlButtonsP2, i, false);
                }
                else
                {
                    throw new System.Exception("Control Menu : Input does not belong to payer1 or player 2 : " + cInput.GetText(i));
                }
            }
        }

        private void CreateControlButton(GameObject controlButton, List<ControlButton> controlButtonsList, int cInputIndex, bool player1 = true)
        {
            string inputName = cInput.GetText(cInputIndex);
            controlButton.transform.SetParent(controlPanel.transform);
            controlButtonsList.Add(new ControlButton(controlButton, player1));
            controlButtonsList[controlButtonsList.Count - 1].cInputLabel = inputName;
            if (player1)
                controlButtonsList[controlButtonsList.Count - 1].UpdateLabel(inputName.Truncate(inputName.Length - 3));
            controlButtonsList[controlButtonsList.Count - 1].button.onClick.AddListener(delegate { UpdateInput(cInputIndex, controlButtonsList, controlButtonsList.Count - 1); });
            controlButtonsList[controlButtonsList.Count - 1].UpdateButtonText(cInput.GetText(cInputIndex, 1));
        }

        private void UpdateInput(int cInputIndex, List<ControlButton> controlButtonsList, int buttonListIndex)
        {
            cInput.ChangeKey(cInputIndex, 1);
            //controlButtonsList[buttonListIndex].UpdateButtonText(cInput.GetText(cInputIndex, 1));
        }

        public void Update()
        {
            for (int i = 0; i < m_ControlButtonsP1.Count; i++)
            {
                m_ControlButtonsP1[i].UpdateButtonText(cInput.GetText(m_ControlButtonsP1[i].cInputLabel, 1));
                m_ControlButtonsP2[i].UpdateButtonText(cInput.GetText(m_ControlButtonsP2[i].cInputLabel, 1));
            }
        }

        public void GoBack()
        {
            if (PauseMenu.Instance != null)
            {
                PauseMenu.Instance.pauseMenuUI.SetActive(true);
                controlMenuUI.SetActive(false);
            }
        }

        public void ResetToDefault()
        {
            cInput.ResetInputs();
        }

    }
}
