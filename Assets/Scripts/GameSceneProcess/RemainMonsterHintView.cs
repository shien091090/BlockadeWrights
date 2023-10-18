using TMPro;
using UnityEngine;

namespace GameCore
{
    public class RemainMonsterHintView : MonoBehaviour, IRemainMonsterHintView
    {
        [SerializeField] private TextMeshProUGUI tmp_hint;

        public void SetRemainCountHint(string hint)
        {
            tmp_hint.text = hint;
        }
    }
}