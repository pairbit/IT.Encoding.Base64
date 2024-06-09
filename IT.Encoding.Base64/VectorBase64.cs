using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace IT.Encoding.Base64;

public static class VectorBase64
{
    public static string ToString<T>(T encoded) where T : unmanaged
    {
        if (Vector128.IsHardwareAccelerated && Unsafe.SizeOf<T>() > 16)
        {
            return string.Create(Unsafe.SizeOf<T>(), encoded, static (chars, encoded) =>
            {
                ref byte b = ref Unsafe.As<T, byte>(ref encoded);
                ref short s = ref Unsafe.As<char, short>(ref MemoryMarshal.GetReference(chars));
                var v = Vector128.LoadUnsafe(ref b);
                Sse2.UnpackLow(v, Vector128<byte>.Zero).AsInt16().StoreUnsafe(ref s);
                Sse2.UnpackHigh(v, Vector128<byte>.Zero).AsInt16().StoreUnsafe(ref s, 8);
                for (int i = 16; i < chars.Length; i++)
                {
                    chars[i] = (char)Unsafe.AddByteOffset(ref b, i);
                }
            });
        }
        else
        {
            return Base64.ToString(encoded);
        }
    }
}