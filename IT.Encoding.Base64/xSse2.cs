using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace IT.Encoding.Base64;

internal static class xSse2
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<byte> LoadUnsafe(ref char src)
    {
        ref short ptr = ref Unsafe.As<char, short>(ref src);
        return Sse2.PackUnsignedSaturate(Vector128.LoadUnsafe(ref ptr), Vector128.LoadUnsafe(ref ptr, 8));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void StoreUnsafe(Vector128<byte> vector, ref char destination)
    {
        ref short ptr = ref Unsafe.As<char, short>(ref destination);
        Sse2.UnpackLow(vector, default).AsInt16().StoreUnsafe(ref ptr);
        Sse2.UnpackHigh(vector, default).AsInt16().StoreUnsafe(ref ptr, 8);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void StoreUnsafe(Vector128<sbyte> vector, ref char destination)
    {
        ref short ptr = ref Unsafe.As<char, short>(ref destination);
        Sse2.UnpackLow(vector, default).AsInt16().StoreUnsafe(ref ptr);
        Sse2.UnpackHigh(vector, default).AsInt16().StoreUnsafe(ref ptr, 8);

        //Unsafe.As<byte, Vector128<short>>(ref Unsafe.As<char, byte>(ref destination)) = Sse2.UnpackLow(vector, default).AsInt16();
        //Unsafe.As<byte, Vector128<short>>(ref Unsafe.As<char, byte>(ref Unsafe.AddByteOffset(ref destination, 16))) = Sse2.UnpackHigh(vector, default).AsInt16();
    }
}