using Panda;
using UnityEngine.AI;
using UnityEngine;


namespace GodsGames
{
    public class Tasks : MonoBehaviour
    {
        public NavMeshAgent agent;
        public GameObject[] targets;
        public float attackRange;
        public string targetTag;
        private GameObject currentTarget;

        void Start ()
        {
            if (targets.Length == 0)
                targets = GameObject.FindGameObjectsWithTag(targetTag);
	    }

        /**
         * TOOLS
         **/
 
        [Task]
        public void AcquireNewTarget ()
        {
            float closestDistance = Mathf.Infinity;
            GameObject closestTarget = null;

            foreach (GameObject target in targets)
            {
                Vector3 diff = transform.position - target.transform.position;
                float distance = diff.sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }

            currentTarget = closestTarget;
            Task.current.Succeed();
        }

        float GetDistFromCurrentTarget()
        {
            if (currentTarget == null)
                return -1;
            return (currentTarget.transform.position - transform.position).sqrMagnitude;
        }

        /**
         * CHARGE
         **/

        [Task]
        public bool CanChargeTarget ()
        {
            // TODO: Use skills here
            return false;
        }

        [Task]
        public void ChargeTarget()
        {
            Task.current.Succeed();
        }

        /**
         * MOVE
         **/

        [Task]
        public bool IsTooFarFromTarget()
        {
            bool isTooFar = currentTarget != null && GetDistFromCurrentTarget() > attackRange;

            if (!isTooFar)
                agent.isStopped = true;

            return isTooFar;
        }

        [Task]
        public void MoveTowardTarget()
        {
            if (currentTarget == null)
            {
                Task.current.Fail();
                return;
            }

            agent.SetDestination(currentTarget.transform.position);
            agent.isStopped = false;

            Task.current.Succeed();
        }

        /**
         * ATTACK
         **/

        [Task]
        public bool CanAttackTarget ()
        {
            return GetDistFromCurrentTarget() <= attackRange;
        }

        [Task]
        public void AttackTarget ()
        {
            Task.current.Succeed();
        }

        /**
         * IDLE
         **/

        [Task]
        public void Idle ()
        {
            agent.isStopped = true;
            Task.current.Succeed();
        }
    }
}
