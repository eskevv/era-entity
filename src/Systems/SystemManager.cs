using System.Diagnostics;
using EraEntity.Entities;

namespace EraEntity.Systems;

public class SystemManager
{
    private Dictionary<string, List<SystemSignature>> _signatures;
    private Dictionary<string, ComponentSystem> _systems;

    public SystemManager()
    {
        _signatures = new Dictionary<string, List<SystemSignature>>();
        _systems = new Dictionary<string, ComponentSystem>();
    }

    public T RegisterSystem<T>() where T : ComponentSystem, new()
    {
        string type_name = typeof(T).Name;
        Debug.Assert(!_systems.ContainsKey(type_name), "Registering system more than once.");
        Console.WriteLine($"Systems[{type_name}] - Registered Successfully.");

        ComponentSystem system = new T();
        _systems[type_name] = system;
        return (T)system;
    }

    public void SetSignature<T>(SystemSignature signature)
    {
        string type_name = typeof(T).Name;
        Debug.Assert(_systems.ContainsKey(type_name), "System used before registered.");

        if (!_signatures.ContainsKey(type_name))
            _signatures[type_name] = new List<SystemSignature>();
            
        _signatures[type_name].Add(signature);
    }

    public void WipeEntityFromSystems(int entity)
    {
        foreach (var item in _systems)
        {
            var system = item.Value;
            system.Entities.Remove(entity);
        }
    }

    public void UpdateEntityReferences(int entity, BitArray entitySignature)
    {
        foreach (var item in _systems)
        {
            string type_name = item.Key;
            var system = item.Value;
            var system_signatures = _signatures[type_name];

            for (var x = 0; x < system_signatures.Count; x++)
            {
                var signature = system_signatures[x];

                if (entitySignature.Includes(signature.BitArray))
                {
                    AddEntityToSystem(system, entity, entitySignature, signature.BitArray);
                    break;
                }
                else if (x == system_signatures.Count - 1)
                {
                    RemoveEntityFromSystem(system, entity);
                }
            }
        }
    }

    private void AddEntityToSystem(ComponentSystem system, int entity, BitArray entitySignature, BitArray systemSignature)
    {
        if (!system.Entities.Add(entity))
            return;

        Console.WriteLine($"\nEntity Signature: {entitySignature} AND System Signature: {systemSignature}");
        Console.WriteLine($"Added Entities[{entity}] TO: Systems[{system.GetType().Name}]");
        Console.WriteLine($"Systems[{system.GetType().Name}] EntityCount = {system.Entities.Count}");
        system.WasUpdated = true;
    }

    private void RemoveEntityFromSystem(ComponentSystem system, int entity)
    {
        if (!system.Entities.Remove(entity))
            return;

        Console.WriteLine($"\nRemoved Entities[{entity}] FROM: Systems[{system.GetType().Name}]");
        Console.WriteLine($"Systems[{system.GetType().Name}] EntityCount = {system.Entities.Count}");
    }
}