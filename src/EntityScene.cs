namespace OrionLibrary;

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
        for (int x = 0; x < _destroyCounter; x++)
        {
            Entity entity = _entitiesToKill[x];
            _entityManager.DestroyEntity(entity);
            _componentManager.DestroyEntityComponents(entity);
            _systemManager.WipeEntityFromSystems(entity);
        }

        _destroyCounter = 0;
    }

    public void DestroyEntity(Entity entity)
    {
        for (int x = 0; x < _destroyCounter; x++)
        {
            if (_entitiesToKill[x] == entity)
                return;
        }

        _entitiesToKill[_destroyCounter++] = entity;
    }

    public void AddComponent<T>(Entity entity, T component) where T : Component
    {
        _componentManager.AddComponent<T>(entity, component);
        BitArray signature = _entityManager.GetSignature(entity);
        ushort component_type = _componentManager.GetComponentType(typeof(T));

        signature.SetBits(component_type);
        _entityManager.SetSignature(entity, signature);
        _systemManager.UpdateEntityReferences(entity, signature);

    }

    public void RemoveComponent<T>(Entity entity)
    {
        _componentManager.RemoveComponent<T>(entity);
        BitArray signature = _entityManager.GetSignature(entity);
        ushort component_type = _componentManager.GetComponentType(typeof(T));

        signature.ClearBits(component_type);
        _entityManager.SetSignature(entity, signature);
        _systemManager.UpdateEntityReferences(entity, signature);
    }

    public EntityHandle CreateEntity(string? tag = null)
    {
        Entity entity = _entityManager.CreateEntity(tag);
        return new EntityHandle(entity);
    }

    #region Scene Interface

    public T RegisterSystem<T>() where T : ComponentSystem, new() => _systemManager.RegisterSystem<T>();

    public void RegisterComponent<T>() => _componentManager.RegisterComponent<T>();

    public void AssignTag(Entity entity, string tag) => _entityManager.AssignTag(entity, tag);

    public void AddSystemSignature<T>(SystemSignature signature) => _systemManager.SetSignature<T>(signature);

    public bool HasComponent<T>(Entity entity) => _componentManager.HasComponentType<T>(entity);

    public ushort GetComponentType(Type type) => _componentManager.GetComponentType(type);

    public T GetComponent<T>(Entity entity) => _componentManager.GetComponent<T>(entity);

    public string? GetTag(Entity entity) => _entityManager.GetTag(entity);

    public EntityHandle FindEntityByTag(string tag)
    {
        Entity entity = _entityManager.FindByTag(tag);
        return new EntityHandle(entity);
    }

    #endregion
}