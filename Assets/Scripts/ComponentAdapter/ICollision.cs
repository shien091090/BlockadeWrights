using UnityEngine;

namespace GameCore
{
    public interface ICollision
    {
        int Layer { get; }
        T GetComponent<T>();
        bool CheckPhysicsOverlapCircle(Vector3 point, float radius, string layerMask);
    }
}