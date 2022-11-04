namespace OrionLibrary;

public class EntityHandle
{
    public Entity ID { get; private set; }
    private EntityScene _registry;

    public EntityHandle(Entity id)
    {
        ID = id;
        _registry = Orion.Scene;
    }

    public void AddComponent<T>(T component) where T : Component =>
        _registry.AddComponent<T>(ID, component);

    public T GetComponent<T>() =>
        _registry.GetComponent<T>(ID);

    public void AssignTag(string tagName) =>
        _registry.AssignTag(ID, tagName);
}