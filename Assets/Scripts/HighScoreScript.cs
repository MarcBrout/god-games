using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GodsGame { 
    public class HighScoreScript : MonoBehaviour {

        public TextMeshProUGUI[] rankingList;
        public TextMeshProUGUI yourScore;
        public TMP_InputField inputField;
        public Button saveButton;
        public int level;
        //TODO: INPUT FIELD
        private Highscore highscore;
        private int newHighscoreIndex;

        // Use this for initialization
        private void Awake()
        {
            highscore = SaveLoad.LoadHighscore(level);
            if (highscore == null)
                highscore = new Highscore(level);

            newHighscoreIndex = -1;
        }

        void Start () {
            LoadScores();
        }

        void Update()
        {
            if (newHighscoreIndex >= 0) {
                inputField.text = inputField.text.ToUpper();
                rankingList[newHighscoreIndex].text = (newHighscoreIndex+1) + ". " + inputField.text + " - " + highscore.HighscoreArr[newHighscoreIndex].Value;
;            }
        }

        private void LoadScores()
        {
            if (highscore != null) {
                for (int i = 0; i < highscore.HighscoreArr.Length; i++)
                {
                    if (highscore.HighscoreArr[i].Value != 0)
                        rankingList[i].text = (i+1)+". " + highscore.HighscoreArr[i].Key + " - " + highscore.HighscoreArr[i].Value;
                }
            }
        }

        public void SaveHighscore() {
            highscore.HighscoreArr[newHighscoreIndex] = new KeyValuePair<string, float>(inputField.text, highscore.HighscoreArr[newHighscoreIndex].Value);
            SaveLoad.SaveHighscore(highscore);
            inputField.gameObject.SetActive(false);
            saveButton.interactable = false;
        }

        public void SubmitScore(float score) {
            newHighscoreIndex = highscore.CheckForNewHighscore(score);
            if (newHighscoreIndex >= 0)
            {
                highscore.UpdateHighScores("", score);
                LoadScores();
                // New highscore at position index
                yourScore.gameObject.SetActive(false);
                inputField.gameObject.SetActive(true);
                saveButton.gameObject.SetActive(true);
            }
            else
            {
                // No new Highscore
                yourScore.gameObject.SetActive(true);
                inputField.gameObject.SetActive(false);
                saveButton.gameObject.SetActive(false);
            }
        }
    }
}
