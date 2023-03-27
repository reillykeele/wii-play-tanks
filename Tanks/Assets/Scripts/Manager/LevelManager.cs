using System.Collections.Generic;
using System.Linq;
using Actor;
using Actor.AITank;
using Data.Enum;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Util.Singleton;

namespace Manager
{
    public class LevelManager : Singleton<LevelManager>
    {
        public SceneType NextLevel;

        [HideInInspector] public TankController Player;
        [HideInInspector] public List<AITankController> AiTanks;

        private int _remainingTanks;

        public int RemainingTanks
        {
            get => _remainingTanks;
            set
            {
                _remainingTanks = value;
                if (_remainingTanks <= 0) GameManager.Instance.LevelClear(NextLevel);
            }
        }

        protected override void Awake()
        {
            base.Awake();
        }

        void Start()
        {
            Player = FindObjectOfType<TankController>();
            AiTanks = FindObjectsOfType<AITankController>().ToList();

            _remainingTanks = AiTanks.Count;
        }

        void Update()
        {

            if (Keyboard.current?.numpadPlusKey.wasPressedThisFrame == true)
            {
                GameManager.Instance.LevelClear(NextLevel);
            }

            if (Keyboard.current?.numpadEnterKey.wasPressedThisFrame == true)
            {
                // transition to the next level
                GameManager.Instance.TransitionLevel(NextLevel);
            }
        }
    }
}
