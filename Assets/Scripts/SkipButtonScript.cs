using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SkipButtonScript : MonoBehaviour
{
    public float fadingSpeed;
    public float startFadinginSeconds = 3;
    public int scene;
    public int timeToChangeScene = 2;

    private bool pressed;
    private float startActiveTime;
    private float startPressedTime;
    private int fill = 0;

    void Start()
    {
        string keyToSkip = cInput.GetText("Jump_P1");

        GameObject buttonObject = this.transform.Find("ButtonImage").gameObject;
        buttonObject.GetComponent<Image>().sprite = getButtonImage(keyToSkip);

        startActiveTime = Time.time;
    }

    void Update()
    {
        if (cInput.GetButton("Jump_P1"))
        {
            if (!pressed)
            {
                startPressedTime = Time.time;

            }
            pressed = true;

            foreach (Transform child in transform)
                child.gameObject.SetActive(true);

            startActiveTime = Time.time;
            
        }
        else {
            pressed = false;
        }

        if (pressed)
        {
            Image img = this.transform.Find("FullCircle").gameObject.GetComponent<Image>();
            img.fillAmount = (Time.time-startPressedTime)/timeToChangeScene;
        }
        else {
            Image img = this.transform.Find("FullCircle").gameObject.GetComponent<Image>();
            img.fillAmount = 0;
        }

        if (pressed && Time.time >= startPressedTime + timeToChangeScene)
        {
            SceneManager.LoadScene(scene);
        }

        if (Time.time > startActiveTime + startFadinginSeconds) {
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
        }
    }

    private Sprite getButtonImage(string button)
    {
        Sprite buttonImg = null;

        //TODO: implement more buttons
        switch (button)
        {
            case "Space":
                buttonImg = Resources.Load<Sprite>("UI-Buttons/Space-Button");
                break;
            case "Joystick1Button0":
                buttonImg = Resources.Load<Sprite>("UI-Buttons/A-Button");
                break;
            case "Joystick1Button1":
                buttonImg = Resources.Load<Sprite>("UI-Buttons/B-Button");
                break;
            case "Joystick1Button2":
                buttonImg = Resources.Load<Sprite>("UI-Buttons/X-Button");
                break;
            case "Joystick1Button3":
                buttonImg = Resources.Load<Sprite>("UI-Buttons/Y-Button");
                break;
            default:
                buttonImg = Resources.Load<Sprite>("UI-Buttons/A-Button");
                break;
        }

        return buttonImg;
    }
}
