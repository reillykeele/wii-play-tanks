using Manager;
using UnityEngine;
using Util.Enums;

namespace UI.ButtonControllers
{
    public class AdjustMusicValueSelectableController : AdjustValueSelectableController
    {
        [SerializeField] 
        private AudioMixerGroupVolumeType _mixerGroup;

        protected override void Start()
        {
            _value = GetVolume() + 80;

            OnValueChangedEvent.Invoke("" + _value);
        }

        protected override void Increase()
        {
            var newVol = Mathf.Min(20, GetVolume() + _incrementValue);
            SetVolume(newVol);

        }

        protected override void Decrease()
        {
            var newVol = Mathf.Max(-80, GetVolume() - _incrementValue);
            SetVolume(newVol);
        }

        private float GetVolume()
        {
            var volName = _mixerGroup.ToString();
            GameManager.Instance.Mixer.GetFloat(volName, out var vol);
            return vol;
        }

        private void SetVolume(float vol)
        {
            var volName = _mixerGroup.ToString();
            GameManager.Instance.Mixer.SetFloat(volName, vol);

            _value = vol + 80f;
        }
    }
}
