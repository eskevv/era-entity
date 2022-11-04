using System.ComponentModel;
using EraEntity.ComponentArray;
using EraEntity.Entities;
using EraEntity.Systems;

namespace EraEntity;

public class EntityScene
{
    private ComponentManager _componentManager;
    private SystemManager _systemManager;
    private EntityManager _entityManager;

    private int[] _entitiesToKill;
    private int _destroyCounter;

    public int TotalEntities => _entityManager.EntityCount;

    public EntityScene()
    {
        _componentManager = new ComponentManager();
        _systemManager = new SystemManager();
        _entityManager = new EntityManager();
        _entitiesToKill = new int[EntityManager.MaxEntities];
    }

    public void Update()
    {
        for (var x = 0; x < _destroyCounter; x++)
        {
            int entity = _entitiesToKill[x];
            _entityManager.DestroyEntity(entity);
            _componentManager.DestroyEntityComponents(entity);
            _systemManager.WipeEntityFromSystems(entity);
        }

        _destroyCounter = 0;
    }

    public void DestroyEntity(int entity)
    {
        for (var x = 0; x < _destroyCounter; x++)
        {
            if (_entitiesToKill[x] == entity)
                return;
        }

        _entitiesToKill[_destroyCounter++] = entity;
    }

    public void AddComponent<T>(int entity, T component) where T : Component
    {
        _componentManager.AddComponent<T>(entity, component);
        var signature = _entityManager.GetSignature(entity);
        ushort componentType = _componentManager.GetComponentType(typeof(T));

        signature.SetBits(componentType);
        _entityManager.SetSignature(entity, signature);
        _systemManager.UpdateEntityReferences(entity, signature);

    }

    public void RemoveComponent<T>(int entity)
    {
        _componentManager.RemoveComponent<T>(entity);
        var signature = _entityManager.GetSignature(entity);
        ushort componentType = _componentManager.GetComponentType(typeof(T));

        signature.ClearBits(componentType);
        _entityManager.SetSignature(entity, signature);
        _systemManager.UpdateEntityReferences(entity, signature);
    }

    public EntityHandle CreateEntity(string? tag = null)
    {
        int entity = _entityManager.CreateEntity(tag);
        return new EntityHandle(entity, this);
    }

    #region Scene Interface

    public T RegisterSystem<T>() where T : ComponentSystem, new() => _systemManager.RegisterSystem<T>();

    public void RegisterComponent<T>() => _componentManager.RegisterComponent<T>();

    public void AssignTag(int entity, string tag) => _entityManager.AssignTag(entity, tag);

    public void AddSystemSignature<T>(SystemSignature signature) => _systemManager.SetSignature<T>(signature);

    public bool HasComponent<T>(int entity) => _componentManager.HasComponentType<T>(entity);

    public ushort GetComponentType(Type type) => _componentManager.GetComponentType(type);

    public T GetComponent<T>(int entity) => _componentManager.GetComponent<T>(entity);

    public string? GetTag(int entity) => _entityManager.GetTag(entity);

    public EntityHandle FindEntityByTag(string tag)
    {
        int entity = _entityManager.FindByTag(tag);
        return new EntityHandle(entity, this);
    }

    #endregion
}