using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace IT.Encoding.Base64;

public static class VectorBase64
{
    public static string ToString176(ref byte encoded)
    {
        var str = new string('\0', 22);
        ref char ch = ref Unsafe.AsRef(in str.GetPinnableReference());
        ref short s = ref Unsafe.As<char, short>(ref ch);
        var v = Vector128.LoadUnsafe(ref encoded);
        Sse2.UnpackLow(v, Vector128<byte>.Zero).AsInt16().StoreUnsafe(ref s);
        Sse2.UnpackHigh(v, Vector128<byte>.Zero).AsInt16().StoreUnsafe(ref s, 8);
        Unsafe.AddByteOffset(ref ch, 32) = (char)Unsafe.AddByteOffset(ref encoded, 16);
        Unsafe.AddByteOffset(ref ch, 34) = (char)Unsafe.AddByteOffset(ref encoded, 17);
        Unsafe.AddByteOffset(ref ch, 36) = (char)Unsafe.AddByteOffset(ref encoded, 18);
        Unsafe.AddByteOffset(ref ch, 38) = (char)Unsafe.AddByteOffset(ref encoded, 19);
        Unsafe.AddByteOffset(ref ch, 40) = (char)Unsafe.AddByteOffset(ref encoded, 20);
        Unsafe.AddByteOffset(ref ch, 42) = (char)Unsafe.AddByteOffset(ref encoded, 21);
        return str;
    }

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