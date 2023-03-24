using System.Collections.Generic;
using Actor.AITank.AITankBehaviour;
using Util.AI.BehaviourTree;

namespace Actor.AITank
{
    public class SimpleAITankController : AITankController
    {
        private BTree _tankBTree;

        public int BTreeRefreshRate = 1;

        protected override void Awake()
        {
            base.Awake();
            
            InitializeBehaviourTree();
        }

        private void InitializeBehaviourTree()
        {
            _tankBTree = new BTree
            {
                Root = new SequenceNode(
                    new List<Node>
                    {
                        new IsPlayerInSightOfTurretNode(this),
                        new AimAtPlayerNode(this)
                    })
            };
        }

        private int ticks = 1;
        void FixedUpdate()
        {
            if (ticks++ % BTreeRefreshRate == 0)
            {
                ticks = 1;
                _tankBTree.Tick();
            }
        }
    }
}
