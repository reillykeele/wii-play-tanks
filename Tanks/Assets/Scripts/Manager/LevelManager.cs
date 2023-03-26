using System.Collections.Generic;
using Actor;
using Actor.AITank;
using Data.Enum;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Util.Singleton;
using SceneType = Data.Enum.SceneType;

namespace Manager
{
    public class LevelManager : Singleton<LevelManager>
    {
        public SceneType CurrentLevel; // use scene name instead? 
        public SceneType NextLevel;

        [HideInInspector] public TankController Player;
        [HideInInspector] public List<AITankController> AiTanks;

        protected override void Awake()
        {
            base.Awake();
        }

        void Update()
        {
            if (Keyboard.current?.numpadEnterKey.wasPressedThisFrame == true)
            {
                // transition to the next level
                LoadingManager.Instance.TransitionScene(NextLevel);
            }
        }
    }
}
