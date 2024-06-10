using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace IT.Encoding.Base64;

internal static class xSse2
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector128<byte> LoadUnsafe(ref char src)
    {
        ref short ptr = ref Unsafe.As<char, short>(ref src);
        return Sse2.PackUnsignedSaturate(Vector128.LoadUnsafe(ref ptr), Vector128.LoadUnsafe(ref ptr, 8));
    }
}