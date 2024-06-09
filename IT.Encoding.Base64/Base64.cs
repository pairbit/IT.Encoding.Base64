using System;
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

    public static EncodingStatus TryToChars<T>(T encoded, Span<char> chars) where T : unmanaged
    {
        if (chars.Length < Unsafe.SizeOf<T>()) return EncodingStatus.InvalidDestinationLength;

        ref byte b = ref Unsafe.As<T, byte>(ref encoded);
        chars[0] = (char)b;
        for (int i = 1; i < chars.Length; i++)
        {
            chars[i] = (char)Unsafe.AddByteOffset(ref b, i);
        }
        return EncodingStatus.Done;
    }

    public static char[] ToChars<T>(T encoded) where T : unmanaged
    {
        var chars = new char[Unsafe.SizeOf<T>()];
        ref byte b = ref Unsafe.As<T, byte>(ref encoded);
        chars[0] = (char)b;
        for (int i = 1; i < chars.Length; i++)
        {
            chars[i] = (char)Unsafe.AddByteOffset(ref b, i);
        }
        return chars;
    }

    public static bool TryTo<T>(ReadOnlySpan<char> encoded, out T value) where T : unmanaged
    {
        value = default;
        if (Unsafe.SizeOf<T>() != encoded.Length) return false;

        ref byte b = ref Unsafe.As<T, byte>(ref value);

        b = (byte)encoded[0];
        for (int i = 1; i < encoded.Length; i++)
        {
            Unsafe.AddByteOffset(ref b, i) = (byte)encoded[i];
        }

        return true;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static T To<T>(ReadOnlySpan<char> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != encoded.Length) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, $"length != {Unsafe.SizeOf<T>()}");

        T value = default;
        ref byte b = ref Unsafe.As<T, byte>(ref value);

        b = (byte)encoded[0];
        for (int i = 1; i < encoded.Length; i++)
        {
            Unsafe.AddByteOffset(ref b, i) = (byte)encoded[i];
        }

        return value;
    }
}