using System.Diagnostics;
using EraEntity.Entities;

namespace EraEntity.ComponentArray;

public class ComponentArray<T> : IComponentArray
{
    private T[] _componentArray;
    private int _size;
    private Dictionary<int, int> _dataIndexes;
    private Dictionary<int, int> _entityIndexes;

    public ComponentArray()
    {
        _componentArray = new T[EntityManager.MaxEntities];
        _dataIndexes = new Dictionary<int, int>();
        _entityIndexes = new Dictionary<int, int>();
    }

    // __Definitions__

    public bool IncludesEntity(int entity) => _dataIndexes.ContainsKey(entity);

    public bool DestroyIndexedData(int entity)
    {
        if (!_dataIndexes.ContainsKey(entity))
            return false;

        RemoveData(entity);
        return true;
    }

    public void InsertData(int entity, T component)
    {
        Debug.Assert(!_dataIndexes.ContainsKey(entity), "Component added to same entity more than once.");

        _dataIndexes[entity] = _size;
        _entityIndexes[_size] = entity;
        _componentArray[_size] = component;
        _size++;
    }

    public void RemoveData(int entity)
    {
        Debug.Assert(_dataIndexes.ContainsKey(entity), "Removing non-existent component.");

        int lastIndex = _size - 1;
        int lastEntity = _entityIndexes[lastIndex];
        int removedDataIndex = _dataIndexes[entity];

        _componentArray[removedDataIndex] = _componentArray[lastIndex];
        _dataIndexes[lastEntity] = removedDataIndex;
        _entityIndexes[removedDataIndex] = lastEntity;

        _dataIndexes.Remove(entity);
        _entityIndexes.Remove(lastIndex);
        _size--;
    }

    public T GetData(int entity)
    {
        Debug.Assert(_dataIndexes.ContainsKey(entity), "Retrieving non-existent component.");
        int dataIndex = _dataIndexes[entity];
        
        return _componentArray[dataIndex];
    }
}