using TMPro;
using UnityEngine;

namespace GameCore
{
    public class WaveHintView : MonoBehaviour, IWaveHintView
    {
        [SerializeField] private TextMeshProUGUI tmp_waveHint;

        public void SetWaveHint(string waveHint)
        {
            tmp_waveHint.text = waveHint;
        }
    }
}