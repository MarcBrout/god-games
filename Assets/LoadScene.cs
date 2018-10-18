using UnityEngine;
using UnityEngine.SceneManagement;

namespace GodsGame
{
    public class LoadScene : MonoBehaviour
    {
        public int indexScene = 0;

        public void OnLoad(Activable activable, Trigger trigger)
        {
            SceneManager.LoadScene(indexScene);
        }
    }
}