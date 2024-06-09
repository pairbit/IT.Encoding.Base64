using System.Runtime.CompilerServices;

namespace IT.Encoding.Base64;

public static class Base64
{
    public static string ToString(ushort encoded) => string.Create(2, encoded, static (chars, encoded) =>
    {
        ref byte b = ref Unsafe.As<ushort, byte>(ref encoded);
        chars[0] = (char)b;
        chars[1] = (char)Unsafe.AddByteOffset(ref b, 1);
    });

    public static string ToString<T>(T encoded) where T : unmanaged => string.Create(Unsafe.SizeOf<T>(), encoded, static (chars, encoded) =>
    {
        ref byte b = ref Unsafe.As<T, byte>(ref encoded);
        chars[0] = (char)b;
        for (int i = 1; i < chars.Length; i++)
        {
            chars[i] = (char)Unsafe.AddByteOffset(ref b, i);
        }
    });
}