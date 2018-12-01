using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GodsGame
{
    public class ShockwaveEvents : MonoBehaviour
    {
        ParticleSystem _shockwaveElement;

        private void Awake()
        {
            _shockwaveElement = GetComponent<ParticleSystem>();
        }

        // Don't set this too high, or NavMesh.SamplePosition() may slow down
        float onMeshThreshold = 3;

        public bool IsAgentOnNavMesh(GameObject agentObject)
        {
            Vector3 agentPosition = agentObject.transform.position;
            NavMeshHit hit;

            // Check for nearest point on navmesh to agent, within onMeshThreshold
            if (NavMesh.SamplePosition(agentPosition, out hit, onMeshThreshold, NavMesh.AllAreas))
            {
                // Check if the positions are vertically aligned
                if (Mathf.Approximately(agentPosition.x, hit.position.x)
                    && Mathf.Approximately(agentPosition.z, hit.position.z))
                {
                    // Lastly, check if object is below navmesh
                    return true;
                }
            }

            return false;
        }

        private void Update()
        {
           if (!IsAgentOnNavMesh(gameObject))
           {
                stopParticles();
           }
        }

        private void stopParticles()
        {
            _shockwaveElement.Stop();
        }

        public void onDamagePlayer(Damager damager, Damageable damageable)
        {
            stopParticles();
        }

        public void onCollision(Damager damager)
        {
        }

    }
}