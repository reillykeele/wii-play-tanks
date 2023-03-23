using UnityEngine;
using UnityEngine.AI;

namespace Actor
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AITankController : BaseTankController
    {
        private NavMeshAgent _agent;

        protected override void Awake()
        {
            base.Awake();

            _agent = GetComponent<NavMeshAgent>();

            _agent.speed = _moveSpeed;
        }

        void Update()
        {

        }

        void FixedUpdate()
        {
            
        }
    }
}
