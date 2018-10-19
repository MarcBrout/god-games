using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour {

    private TextMeshProUGUI m_Text;
    public ScoreManager ScoreManager;

    // Use this for initialization
    void Start () {
        m_Text = GetComponent<TextMeshProUGUI>();
        string res = "SCORE: ";// ScoreManager.GetCurrentScore().ToString();
        m_Text.text = res;
	}
	
	// Update is called once per frame
	void Update()
    {
        string res = "SCORE: ";// ScoreManager.GetCurrentScore().ToString();
        m_Text.text = res;
    }
}