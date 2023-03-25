using System.Collections.Generic;
using Actor.AITank.AITankBehaviour;
using UnityEngine;
using Util.AI.BehaviourTree;

namespace Actor.AITank
{
    public class StationaryAITankController : AITankController
    {
        protected BTree _tankBTree;

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
                Root = new SelectorNode(
                    new List<Node>
                    {
                        new SequenceNode(new List<Node>
                        {
                            new IsPlayerInSightAndInShootingRangeNode(this),
                            new AimAtPlayerNode(this),
                            new ShootNode(this)
                        }),
                        new AimAtRandomNode(this, 4f)
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
