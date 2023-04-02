using System.Collections.Generic;
using Actor.AITank.AITankBehaviour;
using Manager;
using UnityEngine;
using Util.AI.BehaviourTree;

namespace Actor.AITank
{
    public class PatrollingAITankController : AITankController
    {
        [Header("Patrolling AI Tank")]
        [SerializeField] private int _bTreeRefreshRate = 1;
        [SerializeField] private float _scanAngle = 45f;
        [SerializeField] private float _scanSpeed = 0.25f;

        protected BTree _tankBTree;

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

                        new ParallelNode(new List<Node>
                        {
                            // If we see the player, aim at them
                            new SelectorNode(new List<Node>
                            {
                                new SequenceNode(new List<Node>
                                {
                                    new IsPlayerInSightAndInShootingRangeNode(this),
                                    new AimAtPlayerNode(this),
                                    new ShootNode(this),
                                }),
                                new SequenceNode(new List<Node>
                                {
                                    // new DebugLogNode("bruh"),
                                    // new AimAtRandomNode(this, RandomAngle, RandomRefreshRate)
                                    new AimScanTowardsDestinationNode(this, _scanAngle, _scanSpeed)
                                })
                            }),
                            // Wander
                            new SequenceNode(new List<Node>
                            {
                                // choose a direction
                                new FindForwardDirectionNode(this, StoppingDistance),
                                new MoveToDestinationNode(this),
                            })
                        })
                    })
            };
        }

        private int ticks = 1;
        void FixedUpdate()
        {
            if (GameManager.Instance.IsPlaying() == false)
                return;

            if (ticks++ % _bTreeRefreshRate == 0)
            {
                ticks = 1;
                _tankBTree.Tick();
            }
        }
    }
}
