using UnityEngine;
using Zenject;

namespace GameCore
{
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector2 touchRange;
        [SerializeField] private Transform cellHint;

        [Inject] private PlayerModel playerModel;
        [Inject] private IPlayerOperationModel playerOperationModel;

        public ITransform GetTransform
        {
            get
            {
                if (transformAdapter == null)
                    transformAdapter = GetComponent<TransformComponent>();

                return transformAdapter;
            }
        }

        public float MoveSpeed => moveSpeed;
        public Vector2 TouchRange => touchRange;

        private FaceDirectionComponent faceDirection;
        private TransformComponent transformAdapter;

        public void SetCellHintActive(bool isActive)
        {
            cellHint.gameObject.SetActive(isActive);
        }

        public void SetCellHintPosition(Vector2 pos)
        {
            cellHint.position = pos;
        }

        public void RefreshFaceDirection(FaceDirectionState faceDirectionState)
        {
            faceDirection.RefreshFaceDirection(faceDirectionState);
        }

        private void Update()
        {
            playerModel.Update();
        }

        private void Awake()
        {
            playerModel.Bind(this);
            faceDirection = GetComponent<FaceDirectionComponent>();
        }
    }
}