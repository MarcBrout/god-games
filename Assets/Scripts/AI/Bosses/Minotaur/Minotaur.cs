using UnityEngine;

namespace GodsGame {
    public class Minotaur : MonoBehaviour {

        public Animator animator;

        static private string[] BoohSounds = new string[] {
            "arena_crowd_booh",
            "arena_crowd_booh_and_cheer",
        };

        public void OnTakeDamage(Damager damager, Damageable damageable)
        {
            Debug.Log("BOSS IS HIT !");
            CrowdManager.Instance.SetState(SpectatorBehaviour.STATES.PLAYER_HITTEN, 1000);
            AudioManager.Instance.PlaySfx(BoohSounds[Random.Range(0, BoohSounds.Length)], "arena_ambience");
            animator.SetInteger("life", damageable.CurrentHealth);
            animator.SetTrigger("hit");
        }
    }
}