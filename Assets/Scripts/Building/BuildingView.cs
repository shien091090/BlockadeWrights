using UnityEngine;

namespace GameCore
{
    public class BuildingView : MonoBehaviour, IGameObjectPoolEntity
    {
        public bool IsActive => gameObject.activeInHierarchy;

        public void SetPos(Vector2 pos)
        {
            transform.position = pos;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}