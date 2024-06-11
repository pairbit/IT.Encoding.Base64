using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace IT.Encoding.Base64;

internal static class xVector128
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void StoreUnsafe(Vector128<byte> vector, ref char destination)
    {
        ref short ptr = ref Unsafe.As<char, short>(ref destination);
        Vector128.WidenLower(vector).AsInt16().StoreUnsafe(ref ptr);
        Vector128.WidenUpper(vector).AsInt16().StoreUnsafe(ref ptr, 8);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void StoreUnsafe(Vector128<sbyte> vector, ref char destination)
    {
        ref short ptr = ref Unsafe.As<char, short>(ref destination);
        Vector128.WidenLower(vector).StoreUnsafe(ref ptr);
        Vector128.WidenUpper(vector).StoreUnsafe(ref ptr, 8);
    }
}