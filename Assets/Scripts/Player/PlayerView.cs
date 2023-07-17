using UnityEngine;
using Zenject;

namespace GameCore
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;

        [Inject] private PlayerModel playerModel;

        private void Update()
        {
            transform.Translate(playerModel.UpdateMove(moveSpeed, Time.deltaTime));
        }
    }
}