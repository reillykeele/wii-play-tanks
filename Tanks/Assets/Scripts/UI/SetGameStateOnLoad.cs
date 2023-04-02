using Manager;
using Scene;
using UnityEngine;
using Util.Enums;

namespace UI
{
    public class SetGameStateOnLoad : AOnSceneLoad
    {
        [SerializeField] private GameState _gameState;

        protected override void OnSceneLoad()
        {
            GameManager.Instance.ChangeGameState(_gameState);
        }
    }
}