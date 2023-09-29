using UnityEngine;

public class CollisionAdapter : ICollision
{
    public int Layer => collision.gameObject.layer;
    private readonly Collision2D collision;

    public CollisionAdapter(Collision2D col)
    {
        collision = col;
    }

    public bool CheckPhysicsOverlapCircle(Vector3 point, float radius, string layerMask)
    {
        return Physics2D.OverlapCircle(point, radius, LayerMask.GetMask(layerMask));
    }

    public T GetComponent<T>()
    {
        return collision.gameObject.GetComponent<T>();
    }
}