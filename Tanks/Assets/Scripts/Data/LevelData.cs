using Data.Enum;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/Level/Level Data")]
    public class LevelData : UnityEngine.ScriptableObject
    {
        public SceneType SceneType;
        public string LevelName;
        public int NumTanks;
    }
}
