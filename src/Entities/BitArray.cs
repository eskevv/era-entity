namespace EraEntity.Entities;

public class BitArray
{
    private bool[] _set;

    public BitArray(byte size)
    {
        _set = new bool[size];
    }

    public void SetBits(params ushort[] bitNumbers)
    {
        foreach (ushort bitPlace in bitNumbers)
            _set[bitPlace] = true;
    }

    public void ClearBits(params ushort[] bitNumbers)
    {
        foreach (ushort bitPlace in bitNumbers)
            _set[bitPlace] = false;
    }

    public void Reset()
    {
        for (var x = 0; x < _set.Length; x++)
            _set[x] = false;
    }

    public void SetAll()
    {
        for (var x = 0; x < _set.Length; x++)
            _set[x] = true;
    }

    public bool Includes(BitArray b) =>
        !b._set.Where((t, x) => t && !_set[x]).Any();

    public override string ToString()
    {
        var values = _set.ToList().Select(x => x ? 1 : 0);
        return string.Join("", values.Reverse());
    }
}