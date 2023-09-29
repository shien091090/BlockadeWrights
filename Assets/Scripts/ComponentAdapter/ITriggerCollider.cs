public interface ITriggerCollider
{
    int Layer { get; }
    T GetComponent<T>();
}