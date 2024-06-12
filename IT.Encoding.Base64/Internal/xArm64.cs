using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;

namespace IT.Encoding.Base64;

internal static class xArm64
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<sbyte> Shuffle(Vector128<sbyte> vec, Vector128<sbyte> mask)
    {
        return AdvSimd.Arm64.VectorTableLookup(vec.AsByte(), mask.AsByte() & Vector128.Create((byte)0x8f)).AsSByte();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<ushort> MultiplyHigh(Vector128<ushort> vec)
    {
        Vector128<ushort> odd = Vector128.ShiftRightLogical(AdvSimd.Arm64.UnzipOdd(vec.AsUInt16(), vec.AsUInt16()), 6);
        Vector128<ushort> even = Vector128.ShiftRightLogical(AdvSimd.Arm64.UnzipEven(vec.AsUInt16(), vec.AsUInt16()), 10);
        return AdvSimd.Arm64.ZipLow(even, odd);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<short> MultiplyAddAdjacent(Vector128<sbyte> vecA)
    {
        Vector128<ushort> evens = AdvSimd.ShiftLeftLogicalWideningLower(AdvSimd.Arm64.UnzipEven(vecA.AsByte(), Vector128.Create((byte)1)).GetLower(), 6);
        Vector128<ushort> odds = AdvSimd.Arm64.TransposeOdd(vecA.AsByte(), Vector128<byte>.Zero).AsUInt16();
        return (evens + odds).AsInt16();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<int> MultiplyAddAdjacent(Vector128<short> vecA)
    {
        Vector128<int> evens = AdvSimd.ShiftLeftLogicalWideningLower(AdvSimd.Arm64.UnzipEven(vecA, Vector128.Create((byte)1).AsInt16()).GetLower(), 12);
        Vector128<int> odds = AdvSimd.Arm64.TransposeOdd(vecA, Vector128<short>.Zero).AsInt32();
        return (evens + odds).AsInt32();
    }
}