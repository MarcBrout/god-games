using UnityEngine;
using UnityEngine.Playables;

namespace GodsGame {
    public class Minotaur : MonoBehaviour {

        public Animator animator;
        public PlayableDirector timeline;
                
        static private string[] BoohSounds = new string[] {
            "arena_crowd_booh",
            "arena_crowd_booh_and_cheer",
        };

        public void OnTakeDamage(Damager damager, Damageable damageable)
        {
            if (damageable.CurrentHealth == 5)
            {
                timeline.Play();
            }
            CrowdManager.Instance.SetState(SpectatorBehaviour.STATES.PLAYER_HITTEN, 1000);
            AudioManager.Instance.PlaySfx(BoohSounds[UnityEngine.Random.Range(0, BoohSounds.Length)], "arena_ambience");
            animator.SetInteger("life", damageable.CurrentHealth);
        }
    }
}