using System.Collections.Generic;
using Actor.AITank.AITankBehaviour;
using Manager;
using UnityEngine;
using Util.AI.BehaviourTree;

namespace Actor.AITank
{
    public class SniperAITankController : AITankController
    {
        [Header("Sniper AI Tank")]
        [SerializeField] private int _bTreeRefreshRate = 1;
        [SerializeField] private float _scanAngle = 60f;
        [SerializeField] private float _scanSpeed = 0.1f;

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
                        new SequenceNode(new List<Node>
                        {
                            new IsPlayerInSightAndInShootingRangeNode(this),
                            new AimAtPlayerNode(this),
                            new ShootNode(this)
                        }),
                        new AimScanNode(this, _scanAngle, _scanSpeed)
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