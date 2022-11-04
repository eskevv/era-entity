using System.ComponentModel;
using System.Diagnostics;

namespace EraEntity.ComponentArray;

public class ComponentManager
{
    public const byte MaxComponents = 10;

    private byte _nextComponentType;
    private Dictionary<string, byte> _componentTypes;
    private Dictionary<string, IComponentArray> _components;

    public ComponentManager()
    {
        _componentTypes = new Dictionary<string, byte>();
        _components = new Dictionary<string, IComponentArray>();
    }

    // __Definitions__

    private ComponentArray<T> GetComponentArray<T>()
    {
        string typeName = typeof(T).Name;
        Debug.Assert(_componentTypes.ContainsKey(typeName), $"Component |{typeName}| not registered before use.");
        return (ComponentArray<T>)_components[typeName];
    }

    public void RegisterComponent<T>()
    {
        string typeName = typeof(T).Name;
        Debug.Assert(!_componentTypes.ContainsKey(typeName), $"Registering component type |{typeName}| more than once.");
        System.Console.WriteLine($"Components[{typeName}] - Registered Successfully.");
        _componentTypes[typeName] = _nextComponentType++;
        _components[typeName] = new ComponentArray<T>();
    }

    public ushort GetComponentType(Type type)
    {
        string typeName = type.Name;
        Debug.Assert(_componentTypes.ContainsKey(typeName), $"Component |{typeName}| not registered before use.");
        return _componentTypes[typeName];
    }

    public void AddComponent<T>(int entity, T component) where T : Component
    {
        var componentArray = GetComponentArray<T>();
        componentArray.InsertData(entity, component);
        component.Entity = entity;
    }

    public void RemoveComponent<T>(int entity)
    {
        var componentArray = GetComponentArray<T>();
        componentArray.RemoveData(entity);
    }

    public T GetComponent<T>(int entity)
    {
        var componentArray = GetComponentArray<T>();
        return componentArray.GetData(entity);
    }

    public bool HasComponentType<T>(int entity)
    {
        string typeName = typeof(T).Name;
        if (!_componentTypes.ContainsKey(typeName))
            return false;

        var componentArray = GetComponentArray<T>();
        if (!componentArray.IncludesEntity(entity))
            return false;

        return true;
    }

    public void DestroyEntityComponents(int entity)
    {
        foreach (var item in _components)
        {
            var component = item.Value;
            if (component.DestroyIndexedData(entity))
                Debug.WriteLine($"Successfully destroyed components of entity [{entity}]");
        }
    }
}