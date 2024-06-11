using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace IT.Encoding.Base64;

public static class VectorBase64
{
    public static void ToChar176(ref byte by, ref char ch)
    {
        if (BitConverter.IsLittleEndian && Vector128.IsHardwareAccelerated && Sse2.IsSupported)
        {
            xSse2.StoreUnsafe(Vector128.LoadUnsafe(ref by), ref ch);
            Unsafe.AddByteOffset(ref ch, 32) = (char)Unsafe.AddByteOffset(ref by, 16);
            Unsafe.AddByteOffset(ref ch, 34) = (char)Unsafe.AddByteOffset(ref by, 17);
            Unsafe.AddByteOffset(ref ch, 36) = (char)Unsafe.AddByteOffset(ref by, 18);
            Unsafe.AddByteOffset(ref ch, 38) = (char)Unsafe.AddByteOffset(ref by, 19);
            Unsafe.AddByteOffset(ref ch, 40) = (char)Unsafe.AddByteOffset(ref by, 20);
            Unsafe.AddByteOffset(ref ch, 42) = (char)Unsafe.AddByteOffset(ref by, 21);
        }
        else
        {
            UnsafeBase64.ToChar176(ref by, ref ch);
        }
    }

    public static string ToString<T>(T encoded) where T : unmanaged
    {
        if (BitConverter.IsLittleEndian && Vector128.IsHardwareAccelerated && Sse2.IsSupported && Unsafe.SizeOf<T>() > 16)
        {
            return string.Create(Unsafe.SizeOf<T>(), encoded, static (chars, encoded) =>
            {
                ref byte b = ref Unsafe.As<T, byte>(ref encoded);
                xSse2.StoreUnsafe(Vector128.LoadUnsafe(ref b), ref MemoryMarshal.GetReference(chars));
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