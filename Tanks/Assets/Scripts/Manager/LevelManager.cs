using System.Collections.Generic;
using System.Linq;
using Actor;
using Actor.AITank;
using Data;
using Data.Enum;
using UnityEngine;
using UnityEngine.InputSystem;
using Util.Coroutine;
using Util.Enums;
using Util.Singleton;

namespace Manager
{
    public class LevelManager : Singleton<LevelManager>
    {
        public LevelData CurrentLevelData;
        public LevelData NextLevelData;

        [HideInInspector] public TankController Player;
        [HideInInspector] public List<AITankController> AiTanks;

        private int _remainingTanks;

        public int RemainingTanks
        {
            get => _remainingTanks;
            set
            {
                _remainingTanks = value;
                if (_remainingTanks <= 0) GameManager.Instance.LevelClear(NextLevelData); 
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
            # if DEBUG
            if (Keyboard.current?.numpadPlusKey.wasPressedThisFrame == true)
            {
                GameManager.Instance.LevelClear(NextLevelData);
            }

            if (Keyboard.current?.numpadEnterKey.wasPressedThisFrame == true)
            {
                // transition to the next level
                GameManager.Instance.TransitionLevel(NextLevelData);
            }
            #endif
        }

        public void ResetLevel()
        {
            GameManager.Instance.ChangeGameState(GameState.Cutscene);
            StartCoroutine(CoroutineUtil.WaitForExecute(() => GameManager.Instance.TransitionLevel(CurrentLevelData), 3f));
        }
    }
}
