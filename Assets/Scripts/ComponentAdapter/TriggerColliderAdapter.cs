using UnityEngine;

public class TriggerColliderAdapter : ITriggerCollider
{
    public int Layer => collider.gameObject.layer;
    private readonly Collider2D collider;

    public TriggerColliderAdapter(Collider2D collider)
    {
        this.collider = collider;
    }

    public T GetComponent<T>()
    {
        return collider.GetComponent<T>();
    }
}