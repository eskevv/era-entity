using EraEntity.ComponentArray;
using EraEntity.Entities;

namespace EraEntity.Systems;

public class SystemSignature
{
    public BitArray BitArray { get; }
    private EntityScene _registry;

    public SystemSignature(EntityScene registry)
    {
        BitArray = new BitArray(ComponentManager.MaxComponents);
        _registry = registry;
    }

    public void AddRequirements<T>(params T[] types) where T : Type
    {
        var bits = new List<ushort>();

        foreach (var type in types)
        {
            bits.Add(_registry.GetComponentType(type));
        }

        BitArray.SetBits(bits.ToArray());
    }

    public void RemoveRequirements<T>(params T[] types) where T : Type
    {
        var bits = new List<ushort>();

        foreach (var type in types)
        {
            bits.Add(_registry.GetComponentType(type));
        }

        BitArray.ClearBits(bits.ToArray());
    }
}