using UnityEngine;
using TMPro;

namespace GodsGame
{
    public class TimerText : MonoBehaviour
    {

        TextMeshProUGUI time;

        void Start()
        {
            time = GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            time.text = GameManager.Instance.timer.ToString();
        }
    }
}
