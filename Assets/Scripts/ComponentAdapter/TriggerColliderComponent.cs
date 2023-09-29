using System;
using UnityEngine;

public class TriggerColliderComponent : MonoBehaviour
{
    [SerializeField] private ColliderHandleType handleType;
    private IColliderHandler handler;

    public void InitHandler(IColliderHandler handler)
    {
        this.handler = handler;
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (handleType == ColliderHandleType.Collision)
            handler?.CollisionEnter(new CollisionAdapter(col));
    }

    public void OnCollisionExit2D(Collision2D col)
    {
        if (handleType == ColliderHandleType.Collision)
            handler?.CollisionExit(new CollisionAdapter(col));
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (handleType == ColliderHandleType.Trigger)
            handler?.ColliderTriggerEnter(new TriggerColliderAdapter(col));
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (handleType == ColliderHandleType.Trigger)
            handler?.ColliderTriggerExit(new TriggerColliderAdapter(col));
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        if (handleType == ColliderHandleType.Trigger)
            handler?.ColliderTriggerStay(new TriggerColliderAdapter(col));
    }
}