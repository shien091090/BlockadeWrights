public interface IColliderHandler
{
    void ColliderTriggerEnter(ITriggerCollider col);
    void ColliderTriggerExit(ITriggerCollider col);
    void ColliderTriggerStay(ITriggerCollider col);
    void CollisionEnter(ICollision col);
    void CollisionExit(ICollision col);
}