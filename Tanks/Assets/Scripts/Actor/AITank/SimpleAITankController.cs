using System.Collections.Generic;
using Actor.AITank.AITankBehaviour;
using Manager;
using UnityEngine;
using Util.AI.BehaviourTree;

namespace Actor.AITank
{
    public class SimpleAITankController : AITankController
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
                            new IsPlayerVisibleAndInVisionRangeNode(this),
                            new AimAtPlayerNode(this),
                            new SelectorNode(new List<Node>
                            {
                                new ParallelNode(new List<Node>
                                {
                                    new SequenceNode(new List<Node>
                                    {
                                        new IsPlayerInSightAndInShootingRangeNode(this),
                                        new ShootNode(this)
                                    }),
                                    new SelectorNode(new List<Node>
                                    {
                                        new SequenceNode(new List<Node>
                                        {
                                            new InverterNode(new IsPlayerInMovementRangeNode(this)),
                                            new MoveToKnownPlayerLocationNode(this),
                                        }),
                                        new ClearPathNode(this)
                                    })
                                })
                            })
                        }),
                        new SequenceNode(new List<Node>
                        {
                            new KnowPlayerLocationNode(this),
                            new DebugLogNode("Know player location, moving..."),
                            new MoveToKnownPlayerLocationNode(this),
                            new AimAtDestinationNode(this)
                        })
                    })
            };
        }

        private int ticks = 1;
        void FixedUpdate()
        {
            if (GameManager.Instance.IsPlaying() == false)
                return;

            if (ticks++ % BTreeRefreshRate == 0)
            {
                ticks = 1;
                _tankBTree.Tick();
            }
        }
    }
}
