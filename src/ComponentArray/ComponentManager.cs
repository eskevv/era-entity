using System.Diagnostics;

namespace OrionLibrary;

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
        string type_name = typeof(T).Name;
        Debug.Assert(_componentTypes.ContainsKey(type_name), $"Component |{type_name}| not registered before use.");
        return (ComponentArray<T>)_components[type_name];
    }

    public void RegisterComponent<T>()
    {
        string type_name = typeof(T).Name;
        Debug.Assert(!_componentTypes.ContainsKey(type_name), $"Registering component type |{type_name}| more than once.");
        System.Console.WriteLine($"Components[{type_name}] - Registered Successfully.");
        _componentTypes[type_name] = _nextComponentType++;
        _components[type_name] = new ComponentArray<T>();
    }

    public ushort GetComponentType(Type type)
    {
        string type_name = type.Name;
        Debug.Assert(_componentTypes.ContainsKey(type_name), $"Component |{type_name}| not registered before use.");
        return _componentTypes[type_name];
    }

    public void AddComponent<T>(Entity entity, T component) where T : Component
    {
        ComponentArray<T> component_array = GetComponentArray<T>();
        component_array.InsertData(entity, component);
        component.Entity = entity;
    }

    public void RemoveComponent<T>(Entity entity)
    {
        ComponentArray<T> component_array = GetComponentArray<T>();
        component_array.RemoveData(entity);
    }

    public T GetComponent<T>(Entity entity)
    {
        ComponentArray<T> component_array = GetComponentArray<T>();
        return component_array.GetData(entity);
    }

    public bool HasComponentType<T>(Entity entity)
    {
        string type_name = typeof(T).Name;
        if (!_componentTypes.ContainsKey(type_name))
            return false;

        ComponentArray<T> component_array = GetComponentArray<T>();
        if (!component_array.IncludesEntity(entity))
            return false;

        return true;
    }

    public void DestroyEntityComponents(Entity entity)
    {
        foreach (var item in _components)
        {
            var component = item.Value;
            if (component.DestroyIndexedData(entity))
                Debug.WriteLine($"Successfully destroyed components of entity [{entity}]");
        }
    }
}