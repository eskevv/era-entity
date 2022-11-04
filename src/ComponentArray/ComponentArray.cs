using System.Diagnostics;

namespace OrionLibrary;

public class ComponentArray<T> : IComponentArray
{
    private T[] _componentArray;
    private int _size;
    private Dictionary<Entity, int> _dataIndexes;
    private Dictionary<int, Entity> _entityIndexes;

    public ComponentArray()
    {
        _componentArray = new T[EntityManager.MaxEntities];
        _dataIndexes = new Dictionary<Entity, int>();
        _entityIndexes = new Dictionary<int, Entity>();
    }

    // __Definitions__

    public bool IncludesEntity(Entity entity) => _dataIndexes.ContainsKey(entity);

    public bool DestroyIndexedData(Entity entity)
    {
        if (!_dataIndexes.ContainsKey(entity))
            return false;

        RemoveData(entity);
        return true;
    }

    public void InsertData(Entity entity, T component)
    {
        Debug.Assert(!_dataIndexes.ContainsKey(entity), "Component added to same entity more than once.");

        _dataIndexes[entity] = _size;
        _entityIndexes[_size] = entity;
        _componentArray[_size] = component;
        _size++;
    }

    public void RemoveData(Entity entity)
    {
        Debug.Assert(_dataIndexes.ContainsKey(entity), "Removing non-existent component.");

        int last_index = _size - 1;
        int last_entity = _entityIndexes[last_index];
        int removed_data_index = _dataIndexes[entity];

        _componentArray[removed_data_index] = _componentArray[last_index];
        _dataIndexes[last_entity] = removed_data_index;
        _entityIndexes[removed_data_index] = last_entity;

        _dataIndexes.Remove(entity);
        _entityIndexes.Remove(last_index);
        _size--;
    }

    public T GetData(Entity entity)
    {
        Debug.Assert(_dataIndexes.ContainsKey(entity), "Retrieving non-existent component.");
        int data_index = _dataIndexes[entity];
        
        return _componentArray[data_index];
    }
}