using System.Diagnostics;
using EraEntity.ComponentArray;


namespace EraEntity.Entities;

public class EntityManager
{
    public const int MaxEntities = 50000;

    public ushort EntityCount { get; private set; }

    private Queue<int> _availableEntities;
    private BitArray[] _signatures;
    private string?[] _tags;

    public EntityManager()
    {
        _availableEntities = new Queue<int>();
        for (var x = 0; x < MaxEntities; x++)
        {
            _availableEntities.Enqueue(x);
        }

        _signatures = new BitArray[MaxEntities];
        for (var x = 0; x < MaxEntities; x++)
        {
            _signatures[x] = new BitArray(ComponentManager.MaxComponents);
        }

        _tags = new string[MaxEntities];
    }

    public void AssignTag(int entity, string tag) =>
        _tags[entity] = tag;

    public string? GetTag(int entity) =>
        _tags[entity];

    public int CreateEntity(string? tag = null)
    {
        Debug.Assert(EntityCount < MaxEntities, "Too many entities in existence.");
        EntityCount++;
        int newEntity = _availableEntities.Dequeue();
        _tags[newEntity] = tag;

        return newEntity;
    }

    public void DestroyEntity(int entity)
    {
        Debug.Assert(entity < MaxEntities, "Entity out of range.");
        _signatures[entity].Reset();
        _tags[entity] = null;
        EntityCount--;
        _availableEntities.Enqueue(entity);
    }

    public void SetSignature(int entity, BitArray signature)
    {
        Debug.Assert(entity < MaxEntities, "Entity exceeds maximum capacity.");
        _signatures[entity] = signature;
    }

    public BitArray GetSignature(int entity)
    {
        Debug.Assert(entity < MaxEntities, "Entity exceeds maximum capacity.");
        return _signatures[entity];
    }

    public int FindByTag(string tag)
    {
        int entity = _tags.ToList().IndexOf(tag);

        return entity != -1 ? entity : throw new IndexOutOfRangeException();
    }
}