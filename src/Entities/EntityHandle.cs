using System.ComponentModel;

namespace EraEntity.Entities;

public class EntityHandle
{
    public int ID { get; private set; }
    private EntityScene _registry;

    public EntityHandle(int id, EntityScene scene)
    {
        ID = id;
        _registry = scene;
    }

    public void AddComponent<T>(T component) where T : Component =>
        _registry.AddComponent<T>(ID, component);

    public T GetComponent<T>() =>
        _registry.GetComponent<T>(ID);

    public void AssignTag(string tagName) =>
        _registry.AssignTag(ID, tagName);
}