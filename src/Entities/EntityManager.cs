using System.Diagnostics;

namespace OrionLibrary;

public class EntityManager
{
    public const int MaxEntities = 50000;

    public ushort EntityCount { get; private set; }

    private Queue<Entity> _availableEntities;
    private BitArray[] _signatures;
    private string?[] _tags;

    public EntityManager()
    {
        _availableEntities = new Queue<Entity>();
        for (int x = 0; x < MaxEntities; x++)
        {
            _availableEntities.Enqueue(x);
        }

        _signatures = new BitArray[MaxEntities];
        for (int x = 0; x < MaxEntities; x++)
        {
            _signatures[x] = new BitArray(ComponentManager.MaxComponents);
        }

        _tags = new string[MaxEntities];
    }

    public void AssignTag(Entity entity, string tag) =>
        _tags[entity] = tag;

    public string? GetTag(Entity entity) =>
        _tags[entity];

    public Entity CreateEntity(string? tag = null)
    {
        Debug.Assert(EntityCount < MaxEntities, "Too many entities in existence.");
        EntityCount++;
        Entity newEntity = _availableEntities.Dequeue();
        _tags[newEntity] = tag;

        return newEntity;
    }

    public void DestroyEntity(Entity entity)
    {
        Debug.Assert(entity < MaxEntities, "Entity out of range.");
        _signatures[entity].Reset();
        _tags[entity] = null;
        EntityCount--;
        _availableEntities.Enqueue(entity);
    }

    public void SetSignature(Entity entity, BitArray signature)
    {
        Debug.Assert(entity < MaxEntities, "Entity exceeds maximum capacity.");
        _signatures[entity] = signature;
    }

    public BitArray GetSignature(Entity entity)
    {
        Debug.Assert(entity < MaxEntities, "Entity exceeds maximum capacity.");
        return _signatures[entity];
    }

    public Entity FindByTag(string tag)
    {
        Entity entity = _tags.ToList().IndexOf(tag);

        return entity != -1 ? entity : throw new IndexOutOfRangeException();
    }
}