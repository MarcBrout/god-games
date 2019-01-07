using UnityEngine;
using SceneLinkedSMB;
using UnityEngine.SceneManagement;
using System.Collections;

namespace GodsGame
{
    public class DieSMB : SceneLinkedSMB<PlayerBehaviour>
    {
        public float gameOverDelay = 0.1f;
        private float maximumColliderHeight;

        public override void OnStart(Animator animator)
        {
            maximumColliderHeight = m_MonoBehaviour.NavMeshAgent.baseOffset - 0.8f;
        }

        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GameManager.Instance.StatsManager.AddDeathCountFor(animator.name);
            //bullet time
            m_MonoBehaviour.PlayerDeath();
            AudioManager.Instance.PlaySfx("arena_battle_lost", "arena_events");
            m_MonoBehaviour.StartCoroutine(LoadGameOverScene());
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.GroundedHorizontalMovement(false);
        }

        public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }
   
        IEnumerator LoadGameOverScene()
        {
            yield return new WaitForSeconds(gameOverDelay);
            PlayerPrefs.SetString("lastLoadedScene", SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("GameOverArena");
        }
    }
}
