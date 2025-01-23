using UnityEngine;

public static class Bits 
{
    public static bool IsSet(int value, int BitPosition)
    {
        return((value & (1 << BitPosition)) != 0);
    }

    public static void SetBit(ref int value, int BitPosition)
    {
        value |= 1 << BitPosition;
    }
}
