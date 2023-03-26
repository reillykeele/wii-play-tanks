using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Tween
{
    public class GroupTweenController : MonoBehaviour
    {
        private IEnumerable<BaseTween> _tweens;

        [Header("Group Tween")]
        [SerializeField] private float _delay = 0f;
        [SerializeField] private float _delayBetweenElements = 0f;
        
        void Awake()
        {
            _tweens = GetComponentsInChildren<BaseTween>();

            for (var i = 0; i < _tweens.Count(); ++i)
                _tweens.ElementAt(i).SetDelay(_delay + (i * _delayBetweenElements));
        }
    }
}
