using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IT.Encoding.Base64;

public static class Base64Url
{
    #region Encode128

    public static EncodingStatus TryEncode128(UInt128 value, Span<byte> encoded)
    {
        if (encoded.Length < 22) return EncodingStatus.InvalidDestinationLength;

        VectorBase64Url.Encode128(ref Unsafe.As<UInt128, byte>(ref value), ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode128(UInt128 value, Span<char> encoded)
    {
        if (encoded.Length < 22) return EncodingStatus.InvalidDestinationLength;

        VectorBase64Url.Encode128(ref Unsafe.As<UInt128, byte>(ref value), ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode128(UInt128 value, Span<byte> encoded)
    {
        if (encoded.Length < 22) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 22");

        VectorBase64Url.Encode128(ref Unsafe.As<UInt128, byte>(ref value), ref MemoryMarshal.GetReference(encoded));
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode128(UInt128 value, Span<char> encoded)
    {
        if (encoded.Length < 22) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 22");

        VectorBase64Url.Encode128(ref Unsafe.As<UInt128, byte>(ref value), ref MemoryMarshal.GetReference(encoded));
    }

    public static byte[] Encode128ToBytes(UInt128 value)
    {
        var encoded = new byte[22];

        VectorBase64Url.Encode128(ref Unsafe.As<UInt128, byte>(ref value), ref encoded[0]);

        return encoded;
    }

    public static char[] Encode128ToChars(UInt128 value)
    {
        var encoded = new char[22];

        VectorBase64Url.Encode128(ref Unsafe.As<UInt128, byte>(ref value), ref encoded[0]);

        return encoded;
    }

    public static string Encode128ToString(UInt128 value) => string.Create(22, value, static (chars, value) =>
    {
        VectorBase64Url.Encode128(ref Unsafe.As<UInt128, byte>(ref value), ref MemoryMarshal.GetReference(chars));
    });

    #endregion Encode128

    #region Decode128

    public static EncodingStatus TryDecode128(ReadOnlySpan<byte> encoded, out UInt128 value)
    {
        value = default;
        if (encoded.Length != 22) return EncodingStatus.InvalidDataLength;

        if (!VectorBase64Url.TryDecode128(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<UInt128, byte>(ref value)))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode128(ReadOnlySpan<char> encoded, out UInt128 value)
    {
        value = default;
        if (encoded.Length != 22) return EncodingStatus.InvalidDataLength;

        if (!VectorBase64Url.TryDecode128(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<UInt128, byte>(ref value)))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode128(ReadOnlySpan<byte> encoded, out UInt128 value, out byte invalid)
    {
        value = default;
        if (encoded.Length != 22)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        if (!VectorBase64Url.TryDecode128(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<UInt128, byte>(ref value), out invalid))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode128(ReadOnlySpan<char> encoded, out UInt128 value, out char invalid)
    {
        value = default;
        if (encoded.Length != 22)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        if (!VectorBase64Url.TryDecode128(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<UInt128, byte>(ref value), out invalid))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static UInt128 Decode128(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 22) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 22");

        UInt128 value = 0;
        if (!VectorBase64Url.TryDecode128(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<UInt128, byte>(ref value), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid byte");

        return value;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static UInt128 Decode128(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 22) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 22");

        UInt128 value = 0;
        if (!VectorBase64Url.TryDecode128(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<UInt128, byte>(ref value), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid char");

        return value;
    }

    #endregion Decode128

    #region Valid128

    public static EncodingStatus TryValid128(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 22) return EncodingStatus.InvalidDataLength;
        return VectorBase64Url.IsValid128(ref MemoryMarshal.GetReference(encoded)) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid128(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 22) return EncodingStatus.InvalidDataLength;
        return VectorBase64Url.IsValid128(ref MemoryMarshal.GetReference(encoded)) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid128(ReadOnlySpan<byte> encoded, out byte invalid)
    {
        if (encoded.Length != 22)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        return VectorBase64Url.IsValid128(ref MemoryMarshal.GetReference(encoded), out invalid) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid128(ReadOnlySpan<char> encoded, out char invalid)
    {
        if (encoded.Length != 22)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        return VectorBase64Url.IsValid128(ref MemoryMarshal.GetReference(encoded), out invalid) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Valid128(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 22) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 22");
        if (!VectorBase64Url.IsValid128(ref MemoryMarshal.GetReference(encoded), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid byte");
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Valid128(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 22) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 22");
        if (!VectorBase64Url.IsValid128(ref MemoryMarshal.GetReference(encoded), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid char");
    }

    #endregion Valid128

    #region Encode72

    public static EncodingStatus TryEncode72(ReadOnlySpan<byte> bytes, Span<byte> encoded)
    {
        if (bytes.Length != 9) return EncodingStatus.InvalidDataLength;
        if (encoded.Length < 12) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode72(Bytes, ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode72(ReadOnlySpan<byte> bytes, Span<char> encoded)
    {
        if (bytes.Length != 9) return EncodingStatus.InvalidDataLength;
        if (encoded.Length < 12) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode72(Chars, ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode72<T>(T value, Span<byte> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 9) return EncodingStatus.InvalidDataLength;
        if (encoded.Length < 12) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode72(Bytes, ref Unsafe.As<T, byte>(ref value), ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode72<T>(T value, Span<char> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 9) return EncodingStatus.InvalidDataLength;
        if (encoded.Length < 12) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode72(Chars, ref Unsafe.As<T, byte>(ref value), ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode72(ReadOnlySpan<byte> bytes, Span<byte> encoded)
    {
        if (bytes.Length != 9) throw new ArgumentOutOfRangeException(nameof(bytes), bytes.Length, "length != 9");
        if (encoded.Length < 12) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 12");

        UnsafeBase64.Encode72(Bytes, ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded));
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode72(ReadOnlySpan<byte> bytes, Span<char> encoded)
    {
        if (bytes.Length != 9) throw new ArgumentOutOfRangeException(nameof(bytes), bytes.Length, "length != 9");
        if (encoded.Length < 12) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 12");

        UnsafeBase64.Encode72(Chars, ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded));
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode72<T>(T value, Span<byte> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 9) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 9");
        if (encoded.Length < 12) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 12");

        UnsafeBase64.Encode72(Bytes, ref Unsafe.As<T, byte>(ref value), ref MemoryMarshal.GetReference(encoded));
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode72<T>(T value, Span<char> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 9) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 9");
        if (encoded.Length < 12) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 12");

        UnsafeBase64.Encode72(Chars, ref Unsafe.As<T, byte>(ref value), ref MemoryMarshal.GetReference(encoded));
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static byte[] Encode72ToBytes<T>(T value) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 9) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 9");

        var encoded = new byte[12];

        UnsafeBase64.Encode72(Bytes, ref Unsafe.As<T, byte>(ref value), ref encoded[0]);

        return encoded;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static char[] Encode72ToChars<T>(T value) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 9) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 9");

        var encoded = new char[12];

        UnsafeBase64.Encode72(Chars, ref Unsafe.As<T, byte>(ref value), ref encoded[0]);

        return encoded;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static string Encode72ToString<T>(T value) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 9) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 9");

        return string.Create(12, value, static (chars, value) =>
        {
            UnsafeBase64.Encode72(Chars, ref Unsafe.As<T, byte>(ref value), ref MemoryMarshal.GetReference(chars));
        });
    }

    #endregion Encode72

    #region Decode72

    public static EncodingStatus TryDecode72<T>(ReadOnlySpan<byte> encoded, out T value) where T : unmanaged
    {
        value = default;
        if (encoded.Length != 12) return EncodingStatus.InvalidDataLength;
        if (Unsafe.SizeOf<T>() != 9) return EncodingStatus.InvalidDestinationLength;

        if (!UnsafeBase64.TryDecode72(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<T, byte>(ref value)))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode72<T>(ReadOnlySpan<char> encoded, out T value) where T : unmanaged
    {
        value = default;
        if (encoded.Length != 12) return EncodingStatus.InvalidDataLength;
        if (Unsafe.SizeOf<T>() != 9) return EncodingStatus.InvalidDestinationLength;

        if (!UnsafeBase64.TryDecode72(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<T, byte>(ref value)))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode72<T>(ReadOnlySpan<byte> encoded, out T value, out byte invalid) where T : unmanaged
    {
        value = default;
        if (encoded.Length != 12)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        if (Unsafe.SizeOf<T>() != 9)
        {
            invalid = default;
            return EncodingStatus.InvalidDestinationLength;
        }
        if (!UnsafeBase64.TryDecode72(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<T, byte>(ref value), out invalid))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode72<T>(ReadOnlySpan<char> encoded, out T value, out char invalid) where T : unmanaged
    {
        value = default;
        if (encoded.Length != 12)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        if (Unsafe.SizeOf<T>() != 9)
        {
            invalid = default;
            return EncodingStatus.InvalidDestinationLength;
        }
        if (!UnsafeBase64.TryDecode72(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<T, byte>(ref value), out invalid))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static T Decode72<T>(ReadOnlySpan<byte> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 9) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 9");
        if (encoded.Length != 12) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 12");

        T value = default;
        if (!UnsafeBase64.TryDecode72(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<T, byte>(ref value), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "Invalid byte");

        return value;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static T Decode72<T>(ReadOnlySpan<char> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 9) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 9");
        if (encoded.Length != 12) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 12");

        T value = default;
        if (!UnsafeBase64.TryDecode72(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<T, byte>(ref value), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "Invalid char");

        return value;
    }

    #endregion Decode72

    #region Valid72

    public static EncodingStatus TryValid72(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 12) return EncodingStatus.InvalidDataLength;
        return UnsafeBase64.IsValid72(Map, ref MemoryMarshal.GetReference(encoded)) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid72(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 12) return EncodingStatus.InvalidDataLength;
        return UnsafeBase64.IsValid72(Map, ref MemoryMarshal.GetReference(encoded)) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid72(ReadOnlySpan<byte> encoded, out byte invalid)
    {
        if (encoded.Length != 12)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        return UnsafeBase64.IsValid72(Map, ref MemoryMarshal.GetReference(encoded), out invalid) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid72(ReadOnlySpan<char> encoded, out char invalid)
    {
        if (encoded.Length != 12)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        return UnsafeBase64.IsValid72(Map, ref MemoryMarshal.GetReference(encoded), out invalid) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Valid72(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 12) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 12");
        if (!UnsafeBase64.IsValid72(Map, ref MemoryMarshal.GetReference(encoded), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "Invalid byte");
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Valid72(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 12) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 12");
        if (!UnsafeBase64.IsValid72(Map, ref MemoryMarshal.GetReference(encoded), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "Invalid char");
    }

    #endregion Valid72

    #region Encode64

    public static EncodingStatus TryEncode64(ReadOnlySpan<byte> bytes, Span<byte> encoded)
    {
        if (bytes.Length != 8) return EncodingStatus.InvalidDataLength;
        if (encoded.Length < 11) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode64(Bytes, ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode64(ReadOnlySpan<byte> bytes, Span<char> encoded)
    {
        if (bytes.Length != 8) return EncodingStatus.InvalidDataLength;
        if (encoded.Length < 11) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode64(Chars, ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode64(ulong value, Span<byte> encoded)
    {
        if (encoded.Length < 11) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode64(Bytes, ref Unsafe.As<ulong, byte>(ref value), ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode64(ulong value, Span<char> encoded)
    {
        if (encoded.Length < 11) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode64(Chars, ref Unsafe.As<ulong, byte>(ref value), ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode64(ReadOnlySpan<byte> bytes, Span<byte> encoded)
    {
        if (bytes.Length != 8) throw new ArgumentOutOfRangeException(nameof(bytes), bytes.Length, "length != 8");
        if (encoded.Length < 11) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 11");

        UnsafeBase64.Encode64(Bytes, ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded));
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode64(ReadOnlySpan<byte> bytes, Span<char> encoded)
    {
        if (bytes.Length != 8) throw new ArgumentOutOfRangeException(nameof(bytes), bytes.Length, "length != 8");
        if (encoded.Length < 11) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 11");

        UnsafeBase64.Encode64(Chars, ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded));
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode64(ulong value, Span<byte> encoded)
    {
        if (encoded.Length < 11) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 11");

        UnsafeBase64.Encode64(Bytes, ref Unsafe.As<ulong, byte>(ref value), ref MemoryMarshal.GetReference(encoded));
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode64(ulong value, Span<char> encoded)
    {
        if (encoded.Length < 11) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 11");

        UnsafeBase64.Encode64(Chars, ref Unsafe.As<ulong, byte>(ref value), ref MemoryMarshal.GetReference(encoded));
    }

    public static byte[] Encode64ToBytes(ulong value)
    {
        var encoded = new byte[11];

        UnsafeBase64.Encode64(Bytes, ref Unsafe.As<ulong, byte>(ref value), ref encoded[0]);

        return encoded;
    }

    public static char[] Encode64ToChars(ulong value)
    {
        var encoded = new char[11];

        UnsafeBase64.Encode64(Chars, ref Unsafe.As<ulong, byte>(ref value), ref encoded[0]);

        return encoded;
    }

    public static string Encode64ToString(ulong value) => string.Create(11, value, static (chars, value) =>
    {
        UnsafeBase64.Encode64(Chars, ref Unsafe.As<ulong, byte>(ref value), ref MemoryMarshal.GetReference(chars));
    });

    #endregion Encode64

    #region Decode64

    public static EncodingStatus TryDecode64(ReadOnlySpan<byte> encoded, out ulong value)
    {
        value = default;
        if (encoded.Length != 11) return EncodingStatus.InvalidDataLength;

        if (!UnsafeBase64.TryDecode64(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<ulong, byte>(ref value)))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode64(ReadOnlySpan<char> encoded, out ulong value)
    {
        value = default;
        if (encoded.Length != 11) return EncodingStatus.InvalidDataLength;

        if (!UnsafeBase64.TryDecode64(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<ulong, byte>(ref value)))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode64(ReadOnlySpan<byte> encoded, out ulong value, out byte invalid)
    {
        value = default;
        if (encoded.Length != 11)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        if (!UnsafeBase64.TryDecode64(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<ulong, byte>(ref value), out invalid))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode64(ReadOnlySpan<char> encoded, out ulong value, out char invalid)
    {
        value = default;
        if (encoded.Length != 11)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        if (!UnsafeBase64.TryDecode64(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<ulong, byte>(ref value), out invalid))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static ulong Decode64(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 11) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 11");

        ulong value = 0;
        if (!UnsafeBase64.TryDecode64(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<ulong, byte>(ref value), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid byte");

        return value;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static ulong Decode64(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 11) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 11");

        ulong value = 0;
        if (!UnsafeBase64.TryDecode64(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<ulong, byte>(ref value), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid char");

        return value;
    }

    #endregion Decode64

    #region Valid64

    public static EncodingStatus TryValid64(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 11) return EncodingStatus.InvalidDataLength;
        return UnsafeBase64.IsValid64(Map, ref MemoryMarshal.GetReference(encoded)) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid64(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 11) return EncodingStatus.InvalidDataLength;
        return UnsafeBase64.IsValid64(Map, ref MemoryMarshal.GetReference(encoded)) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid64(ReadOnlySpan<byte> encoded, out byte invalid)
    {
        if (encoded.Length != 11)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        return UnsafeBase64.IsValid64(Map, ref MemoryMarshal.GetReference(encoded), out invalid) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid64(ReadOnlySpan<char> encoded, out char invalid)
    {
        if (encoded.Length != 11)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        return UnsafeBase64.IsValid64(Map, ref MemoryMarshal.GetReference(encoded), out invalid) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Valid64(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 11) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 11");
        if (!UnsafeBase64.IsValid64(Map, ref MemoryMarshal.GetReference(encoded), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid byte");
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Valid64(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 11) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 11");
        if (!UnsafeBase64.IsValid64(Map, ref MemoryMarshal.GetReference(encoded), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid char");
    }

    #endregion Valid64

    #region Encode32

    public static EncodingStatus TryEncode32(uint value, Span<byte> encoded)
    {
        if (encoded.Length < 6) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode32(Bytes, ref Unsafe.As<uint, byte>(ref value), ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode32(uint value, Span<char> encoded)
    {
        if (encoded.Length < 6) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode32(Chars, ref Unsafe.As<uint, byte>(ref value), ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode32(uint value, Span<byte> encoded)
    {
        if (encoded.Length < 6) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 6");

        UnsafeBase64.Encode32(Bytes, ref Unsafe.As<uint, byte>(ref value), ref MemoryMarshal.GetReference(encoded));
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode32(uint value, Span<char> encoded)
    {
        if (encoded.Length < 6) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 6");

        UnsafeBase64.Encode32(Chars, ref Unsafe.As<uint, byte>(ref value), ref MemoryMarshal.GetReference(encoded));
    }

    public static byte[] Encode32ToBytes(uint value)
    {
        var encoded = new byte[6];

        UnsafeBase64.Encode32(Bytes, ref Unsafe.As<uint, byte>(ref value), ref encoded[0]);

        return encoded;
    }

    public static char[] Encode32ToChars(uint value)
    {
        var encoded = new char[6];

        UnsafeBase64.Encode32(Chars, ref Unsafe.As<uint, byte>(ref value), ref encoded[0]);

        return encoded;
    }

    public static string Encode32ToString(uint value) => string.Create(6, value, static (chars, value) =>
    {
        UnsafeBase64.Encode32(Chars, ref Unsafe.As<uint, byte>(ref value), ref MemoryMarshal.GetReference(chars));
    });

    #endregion Encode32

    #region Decode32

    public static EncodingStatus TryDecode32(ReadOnlySpan<byte> encoded, out uint value)
    {
        value = default;
        if (encoded.Length != 6) return EncodingStatus.InvalidDataLength;

        if (!UnsafeBase64.TryDecode32(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<uint, byte>(ref value)))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode32(ReadOnlySpan<char> encoded, out uint value)
    {
        value = default;
        if (encoded.Length != 6) return EncodingStatus.InvalidDataLength;

        if (!UnsafeBase64.TryDecode32(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<uint, byte>(ref value)))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode32(ReadOnlySpan<byte> encoded, out uint value, out byte invalid)
    {
        value = default;
        if (encoded.Length != 6)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        if (!UnsafeBase64.TryDecode32(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<uint, byte>(ref value), out invalid))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode32(ReadOnlySpan<char> encoded, out uint value, out char invalid)
    {
        value = default;
        if (encoded.Length != 6)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        if (!UnsafeBase64.TryDecode32(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<uint, byte>(ref value), out invalid))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static uint Decode32(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 6) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 6");

        uint value = 0;
        if (!UnsafeBase64.TryDecode32(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<uint, byte>(ref value), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid byte");

        return value;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static uint Decode32(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 6) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 6");

        uint value = 0;
        if (!UnsafeBase64.TryDecode32(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<uint, byte>(ref value), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid char");

        return value;
    }

    #endregion Decode32

    #region Valid32

    public static EncodingStatus TryValid32(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 6) return EncodingStatus.InvalidDataLength;
        return UnsafeBase64.IsValid32(Map, ref MemoryMarshal.GetReference(encoded)) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid32(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 6) return EncodingStatus.InvalidDataLength;
        return UnsafeBase64.IsValid32(Map, ref MemoryMarshal.GetReference(encoded)) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid32(ReadOnlySpan<byte> encoded, out byte invalid)
    {
        if (encoded.Length != 6)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        return UnsafeBase64.IsValid32(Map, ref MemoryMarshal.GetReference(encoded), out invalid) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid32(ReadOnlySpan<char> encoded, out char invalid)
    {
        if (encoded.Length != 6)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        return UnsafeBase64.IsValid32(Map, ref MemoryMarshal.GetReference(encoded), out invalid) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Valid32(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 6) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 6");
        if (!UnsafeBase64.IsValid32(Map, ref MemoryMarshal.GetReference(encoded), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid byte");
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Valid32(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 6) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 6");
        if (!UnsafeBase64.IsValid32(Map, ref MemoryMarshal.GetReference(encoded), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid char");
    }

    #endregion Valid32

    #region Encode24

    public static EncodingStatus TryEncode24(ReadOnlySpan<byte> bytes, Span<byte> encoded)
    {
        if (bytes.Length != 3) return EncodingStatus.InvalidDataLength;
        if (encoded.Length < 4) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode24(Bytes, ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode24(ReadOnlySpan<byte> bytes, Span<char> encoded)
    {
        if (bytes.Length != 3) return EncodingStatus.InvalidDataLength;
        if (encoded.Length < 4) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode24(Chars, ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode24<T>(T value, Span<byte> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 3) return EncodingStatus.InvalidDataLength;
        if (encoded.Length < 4) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode24(Bytes, ref Unsafe.As<T, byte>(ref value), ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode24<T>(T value, Span<char> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 3) return EncodingStatus.InvalidDataLength;
        if (encoded.Length < 4) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode24(Chars, ref Unsafe.As<T, byte>(ref value), ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode24(ReadOnlySpan<byte> bytes, Span<byte> encoded)
    {
        if (bytes.Length != 3) throw new ArgumentOutOfRangeException(nameof(bytes), bytes.Length, "length != 3");
        if (encoded.Length < 4) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 4");

        UnsafeBase64.Encode24(Bytes, ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded));
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode24(ReadOnlySpan<byte> bytes, Span<char> encoded)
    {
        if (bytes.Length != 3) throw new ArgumentOutOfRangeException(nameof(bytes), bytes.Length, "length != 3");
        if (encoded.Length < 4) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 4");

        UnsafeBase64.Encode24(Chars, ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded));
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode24<T>(T value, Span<byte> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 3) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 3");
        if (encoded.Length < 4) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 4");

        UnsafeBase64.Encode24(Bytes, ref Unsafe.As<T, byte>(ref value), ref MemoryMarshal.GetReference(encoded));
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode24<T>(T value, Span<char> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 3) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 3");
        if (encoded.Length < 4) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 4");

        UnsafeBase64.Encode24(Chars, ref Unsafe.As<T, byte>(ref value), ref MemoryMarshal.GetReference(encoded));
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Encode24ToInt32<T>(T value)
    {
        if (Unsafe.SizeOf<T>() != 3) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 3");

        return UnsafeBase64.Encode24ToInt32(Bytes, ref Unsafe.As<T, byte>(ref value));
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static byte[] Encode24ToBytes<T>(T value) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 3) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 3");

        var encoded = new byte[4];

        UnsafeBase64.Encode24(Bytes, ref Unsafe.As<T, byte>(ref value), ref encoded[0]);

        return encoded;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static char[] Encode24ToChars<T>(T value) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 3) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 3");

        var encoded = new char[4];

        UnsafeBase64.Encode24(Chars, ref Unsafe.As<T, byte>(ref value), ref encoded[0]);

        return encoded;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static string Encode24ToString<T>(T value) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 3) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 3");

        return string.Create(4, value, static (chars, value) =>
        {
            UnsafeBase64.Encode24(Chars, ref Unsafe.As<T, byte>(ref value), ref MemoryMarshal.GetReference(chars));
        });
    }

    #endregion Encode24

    #region Decode24

    public static EncodingStatus TryDecode24<T>(ReadOnlySpan<byte> encoded, out T value) where T : unmanaged
    {
        value = default;
        if (encoded.Length != 4) return EncodingStatus.InvalidDataLength;
        if (Unsafe.SizeOf<T>() != 3) return EncodingStatus.InvalidDestinationLength;

        if (!UnsafeBase64.TryDecode24(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<T, byte>(ref value)))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode24<T>(ReadOnlySpan<char> encoded, out T value) where T : unmanaged
    {
        value = default;
        if (encoded.Length != 4) return EncodingStatus.InvalidDataLength;
        if (Unsafe.SizeOf<T>() != 3) return EncodingStatus.InvalidDestinationLength;

        if (!UnsafeBase64.TryDecode24(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<T, byte>(ref value)))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode24<T>(ReadOnlySpan<byte> encoded, out T value, out byte invalid) where T : unmanaged
    {
        value = default;
        if (encoded.Length != 4)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        if (Unsafe.SizeOf<T>() != 3)
        {
            invalid = default;
            return EncodingStatus.InvalidDestinationLength;
        }
        if (!UnsafeBase64.TryDecode24(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<T, byte>(ref value), out invalid))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode24<T>(ReadOnlySpan<char> encoded, out T value, out char invalid) where T : unmanaged
    {
        value = default;
        if (encoded.Length != 4)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        if (Unsafe.SizeOf<T>() != 3)
        {
            invalid = default;
            return EncodingStatus.InvalidDestinationLength;
        }
        if (!UnsafeBase64.TryDecode24(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<T, byte>(ref value), out invalid))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static T Decode24<T>(ReadOnlySpan<byte> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 3) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 3");
        if (encoded.Length != 4) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 4");

        T value = default;
        if (!UnsafeBase64.TryDecode24(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<T, byte>(ref value), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "Invalid byte");

        return value;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static T Decode24<T>(ReadOnlySpan<char> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 3) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 3");
        if (encoded.Length != 4) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 4");

        T value = default;
        if (!UnsafeBase64.TryDecode24(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<T, byte>(ref value), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "Invalid char");

        return value;
    }

    #endregion Decode24

    #region Valid24

    public static EncodingStatus TryValid24(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 4) return EncodingStatus.InvalidDataLength;
        return UnsafeBase64.IsValid24(Map, ref MemoryMarshal.GetReference(encoded)) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid24(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 4) return EncodingStatus.InvalidDataLength;
        return UnsafeBase64.IsValid24(Map, ref MemoryMarshal.GetReference(encoded)) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid24(ReadOnlySpan<byte> encoded, out byte invalid)
    {
        if (encoded.Length != 4)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        return UnsafeBase64.IsValid24(Map, ref MemoryMarshal.GetReference(encoded), out invalid) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid24(ReadOnlySpan<char> encoded, out char invalid)
    {
        if (encoded.Length != 4)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        return UnsafeBase64.IsValid24(Map, ref MemoryMarshal.GetReference(encoded), out invalid) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Valid24(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 4) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 4");
        if (!UnsafeBase64.IsValid24(Map, ref MemoryMarshal.GetReference(encoded), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "Invalid byte");
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Valid24(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 4) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 4");
        if (!UnsafeBase64.IsValid24(Map, ref MemoryMarshal.GetReference(encoded), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "Invalid char");
    }

    #endregion Valid24

    #region Encode16

    public static EncodingStatus TryEncode16(ushort value, Span<byte> encoded)
    {
        if (encoded.Length < 3) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode16(Bytes, ref Unsafe.As<ushort, byte>(ref value), ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode16(ushort value, Span<char> encoded)
    {
        if (encoded.Length < 3) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode16(Chars, ref Unsafe.As<ushort, byte>(ref value), ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode16(ushort value, Span<byte> encoded)
    {
        if (encoded.Length < 3) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 3");

        UnsafeBase64.Encode16(Bytes, ref Unsafe.As<ushort, byte>(ref value), ref MemoryMarshal.GetReference(encoded));
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode16(ushort value, Span<char> encoded)
    {
        if (encoded.Length < 3) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 3");

        UnsafeBase64.Encode16(Chars, ref Unsafe.As<ushort, byte>(ref value), ref MemoryMarshal.GetReference(encoded));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode16(ushort value, out byte encoded0, out byte encoded1, out byte encoded2)
    {
        var map = Bytes;
        //TODO: refactoring
        ref byte src = ref Unsafe.As<ushort, byte>(ref value);
        int i = src << 16 | Unsafe.AddByteOffset(ref src, 1) << 8;
        encoded0 = map[i >> 18];
        encoded1 = map[i >> 12 & 0x3F];
        encoded2 = map[i >> 6 & 0x3F];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode16(ushort value, out char encoded0, out char encoded1, out char encoded2)
    {
        var map = Chars;
        ref byte src = ref Unsafe.As<ushort, byte>(ref value);
        int i = src << 16 | Unsafe.AddByteOffset(ref src, 1) << 8;
        encoded0 = map[i >> 18];
        encoded1 = map[i >> 12 & 0x3F];
        encoded2 = map[i >> 6 & 0x3F];
    }

    public static byte[] Encode16ToBytes(ushort value)
    {
        var encoded = new byte[3];

        UnsafeBase64.Encode16(Bytes, ref Unsafe.As<ushort, byte>(ref value), ref encoded[0]);

        return encoded;
    }

    public static char[] Encode16ToChars(ushort value)
    {
        var encoded = new char[3];

        UnsafeBase64.Encode16(Chars, ref Unsafe.As<ushort, byte>(ref value), ref encoded[0]);

        return encoded;
    }

    public static string Encode16ToString(ushort value) => string.Create(3, value, static (chars, value) =>
    {
        UnsafeBase64.Encode16(Chars, ref Unsafe.As<ushort, byte>(ref value), ref MemoryMarshal.GetReference(chars));
    });

    #endregion Encode16

    #region Decode16

    public static EncodingStatus TryDecode16(ReadOnlySpan<byte> encoded, out ushort value)
    {
        value = default;
        if (encoded.Length != 3) return EncodingStatus.InvalidDataLength;

        if (!UnsafeBase64.TryDecode16(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<ushort, byte>(ref value)))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode16(ReadOnlySpan<char> encoded, out ushort value)
    {
        value = default;
        if (encoded.Length != 3) return EncodingStatus.InvalidDataLength;

        if (!UnsafeBase64.TryDecode16(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<ushort, byte>(ref value)))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode16(ReadOnlySpan<byte> encoded, out ushort value, out byte invalid)
    {
        value = default;
        if (encoded.Length != 3)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        if (!UnsafeBase64.TryDecode16(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<ushort, byte>(ref value), out invalid))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode16(ReadOnlySpan<char> encoded, out ushort value, out char invalid)
    {
        value = default;
        if (encoded.Length != 3)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        if (!UnsafeBase64.TryDecode16(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<ushort, byte>(ref value), out invalid))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static ushort Decode16(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 3) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 3");

        ushort value = 0;
        if (!UnsafeBase64.TryDecode16(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<ushort, byte>(ref value), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid byte");

        return value;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static ushort Decode16(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 3) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 3");

        ushort value = 0;
        if (!UnsafeBase64.TryDecode16(Map, ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<ushort, byte>(ref value), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid char");

        return value;
    }

    #endregion Decode16

    #region Valid16

    public static EncodingStatus TryValid16(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 3) return EncodingStatus.InvalidDataLength;
        return UnsafeBase64.IsValid16(Map, ref MemoryMarshal.GetReference(encoded)) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid16(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 3) return EncodingStatus.InvalidDataLength;
        return UnsafeBase64.IsValid16(Map, ref MemoryMarshal.GetReference(encoded)) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid16(ReadOnlySpan<byte> encoded, out byte invalid)
    {
        if (encoded.Length != 3)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        return UnsafeBase64.IsValid16(Map, ref MemoryMarshal.GetReference(encoded), out invalid) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid16(ReadOnlySpan<char> encoded, out char invalid)
    {
        if (encoded.Length != 3)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        return UnsafeBase64.IsValid16(Map, ref MemoryMarshal.GetReference(encoded), out invalid) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Valid16(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 3) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 3");
        if (!UnsafeBase64.IsValid16(Map, ref MemoryMarshal.GetReference(encoded), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid byte");
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Valid16(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 3) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 3");
        if (!UnsafeBase64.IsValid16(Map, ref MemoryMarshal.GetReference(encoded), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid char");
    }

    #endregion Valid16

    #region Encode8

    public static EncodingStatus TryEncode8(byte value, Span<byte> encoded)
    {
        if (encoded.Length < 2) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode8(Bytes, ref value, ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode8(byte value, Span<char> encoded)
    {
        if (encoded.Length < 2) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode8(Chars, ref value, ref MemoryMarshal.GetReference(encoded));

        return EncodingStatus.Done;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode8(byte value, Span<byte> encoded)
    {
        if (encoded.Length < 2) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 2");

        UnsafeBase64.Encode8(Bytes, ref value, ref MemoryMarshal.GetReference(encoded));
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode8(byte value, Span<char> encoded)
    {
        if (encoded.Length < 2) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 2");

        UnsafeBase64.Encode8(Chars, ref value, ref MemoryMarshal.GetReference(encoded));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode8(byte value, out byte encoded0, out byte encoded1)
    {
        var map = Bytes;
        int i = value << 8;
        encoded0 = map[i >> 10];
        encoded1 = map[i >> 4 & 0x3F];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode8(byte value, out char encoded0, out char encoded1)
    {
        var map = Chars;
        int i = value << 8;
        encoded0 = map[i >> 10];
        encoded1 = map[i >> 4 & 0x3F];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short Encode8ToInt16(byte value) => UnsafeBase64.Encode8ToInt16(Bytes, ref value);

    public static byte[] Encode8ToBytes(byte value)
    {
        var encoded = new byte[2];

        UnsafeBase64.Encode8(Bytes, ref value, ref encoded[0]);

        return encoded;
    }

    public static char[] Encode8ToChars(byte value)
    {
        var encoded = new char[2];

        UnsafeBase64.Encode8(Chars, ref value, ref encoded[0]);

        return encoded;
    }

    public static string Encode8ToString(byte value) => string.Create(2, value, static (chars, value) =>
    {
        UnsafeBase64.Encode8(Chars, ref value, ref MemoryMarshal.GetReference(chars));
    });

    #endregion Encode8

    #region Decode8

    public static EncodingStatus TryDecode8(ReadOnlySpan<byte> encoded, out byte value)
    {
        value = default;
        if (encoded.Length != 2) return EncodingStatus.InvalidDataLength;

        if (!UnsafeBase64.TryDecode8(Map, ref MemoryMarshal.GetReference(encoded), ref value))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode8(ReadOnlySpan<char> encoded, out byte value)
    {
        value = default;
        if (encoded.Length != 2) return EncodingStatus.InvalidDataLength;

        if (!UnsafeBase64.TryDecode8(Map, ref MemoryMarshal.GetReference(encoded), ref value))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode8(ReadOnlySpan<byte> encoded, out byte value, out byte invalid)
    {
        value = default;
        if (encoded.Length != 2)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        if (!UnsafeBase64.TryDecode8(Map, ref MemoryMarshal.GetReference(encoded), ref value, out invalid))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    public static EncodingStatus TryDecode8(ReadOnlySpan<char> encoded, out byte value, out char invalid)
    {
        value = default;
        if (encoded.Length != 2)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        if (!UnsafeBase64.TryDecode8(Map, ref MemoryMarshal.GetReference(encoded), ref value, out invalid))
        {
            value = default;
            return EncodingStatus.InvalidData;
        }
        return EncodingStatus.Done;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static byte Decode8(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 2) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 2");

        byte value = 0;
        if (!UnsafeBase64.TryDecode8(Map, ref MemoryMarshal.GetReference(encoded), ref value, out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid byte");

        return value;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static byte Decode8(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 2) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 2");

        byte value = 0;
        if (!UnsafeBase64.TryDecode8(Map, ref MemoryMarshal.GetReference(encoded), ref value, out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid char");

        return value;
    }

    #endregion Decode8

    #region Valid8

    public static EncodingStatus TryValid8(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 2) return EncodingStatus.InvalidDataLength;
        return UnsafeBase64.IsValid8(Map, ref MemoryMarshal.GetReference(encoded)) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid8(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 2) return EncodingStatus.InvalidDataLength;
        return UnsafeBase64.IsValid8(Map, ref MemoryMarshal.GetReference(encoded)) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid8(ReadOnlySpan<byte> encoded, out byte invalid)
    {
        if (encoded.Length != 2)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        return UnsafeBase64.IsValid8(Map, ref MemoryMarshal.GetReference(encoded), out invalid) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    public static EncodingStatus TryValid8(ReadOnlySpan<char> encoded, out char invalid)
    {
        if (encoded.Length != 2)
        {
            invalid = default;
            return EncodingStatus.InvalidDataLength;
        }
        return UnsafeBase64.IsValid8(Map, ref MemoryMarshal.GetReference(encoded), out invalid) ? EncodingStatus.Done : EncodingStatus.InvalidData;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Valid8(ReadOnlySpan<byte> encoded)
    {
        if (encoded.Length != 2) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 2");
        if (!UnsafeBase64.IsValid8(Map, ref MemoryMarshal.GetReference(encoded), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid byte");
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Valid8(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 2) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 2");
        if (!UnsafeBase64.IsValid8(Map, ref MemoryMarshal.GetReference(encoded), out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid char");
    }

    #endregion Valid8

    #region Fields

    public const string Encoding = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";

    public static readonly char[] Chars = Encoding.ToCharArray();

    public static readonly byte[] Bytes = System.Text.Encoding.UTF8.GetBytes(Encoding);

    public static readonly sbyte[] Map = [
    -1, //0
    -1, //1
    -1, //2
    -1, //3
    -1, //4
    -1, //5
    -1, //6
    -1, //7
    -1, //8
    -1, //9
    -1, //10
    -1, //11
    -1, //12
    -1, //13
    -1, //14
    -1, //15
    -1, //16
    -1, //17
    -1, //18
    -1, //19
    -1, //20
    -1, //21
    -1, //22
    -1, //23
    -1, //24
    -1, //25
    -1, //26
    -1, //27
    -1, //28
    -1, //29
    -1, //30
    -1, //31
    -1, //32
    -1, //33
    -1, //34
    -1, //35
    -1, //36
    -1, //37
    -1, //38
    -1, //39
    -1, //40
    -1, //41
    -1, //42
    62, //43 -> +
    -1, //44
    62, //45 -> -
    -1, //46
    63, //47 -> /
    52, //48 -> 0
    53, //49 -> 1
    54, //50 -> 2
    55, //51 -> 3
    56, //52 -> 4
    57, //53 -> 5
    58, //54 -> 6
    59, //55 -> 7
    60, //56 -> 8
    61, //57 -> 9
    -1, //58
    -1, //59
    -1, //60
    -1, //61
    -1, //62
    -1, //63
    -1, //64
     0, //65 -> A
     1, //66 -> B
     2, //67 -> C
     3, //68 -> D
     4, //69 -> E
     5, //70 -> F
     6, //71 -> G
     7, //72 -> H
     8, //73 -> I
     9, //74 -> J
    10, //75 -> K
    11, //76 -> L
    12, //77 -> M
    13, //78 -> N
    14, //79 -> O
    15, //80 -> P
    16, //81 -> Q
    17, //82 -> R
    18, //83 -> S
    19, //84 -> T
    20, //85 -> U
    21, //86 -> V
    22, //87 -> W
    23, //88 -> X
    24, //89 -> Y
    25, //90 -> Z
    -1, //91
    -1, //92
    -1, //93
    -1, //94
    63, //95 -> _
    -1, //96
    26, //97 -> a
    27, //98 -> b
    28, //99 -> c
    29, //100 -> d
    30, //101 -> e
    31, //102 -> f
    32, //103 -> g
    33, //104 -> h
    34, //105 -> i
    35, //106 -> j
    36, //107 -> k
    37, //108 -> l
    38, //109 -> m
    39, //110 -> n
    40, //111 -> o
    41, //112 -> p
    42, //113 -> q
    43, //114 -> r
    44, //115 -> s
    45, //116 -> t
    46, //117 -> u
    47, //118 -> v
    48, //119 -> w
    49, //120 -> x
    50, //121 -> y
    51, //122 -> z
    -1, //123
    -1, //124
    -1, //125
    -1, //126
    -1, //127
    -1, //128
    -1, //129
    -1, //130
    -1, //131
    -1, //132
    -1, //133
    -1, //134
    -1, //135
    -1, //136
    -1, //137
    -1, //138
    -1, //139
    -1, //140
    -1, //141
    -1, //142
    -1, //143
    -1, //144
    -1, //145
    -1, //146
    -1, //147
    -1, //148
    -1, //149
    -1, //150
    -1, //151
    -1, //152
    -1, //153
    -1, //154
    -1, //155
    -1, //156
    -1, //157
    -1, //158
    -1, //159
    -1, //160
    -1, //161
    -1, //162
    -1, //163
    -1, //164
    -1, //165
    -1, //166
    -1, //167
    -1, //168
    -1, //169
    -1, //170
    -1, //171
    -1, //172
    -1, //173
    -1, //174
    -1, //175
    -1, //176
    -1, //177
    -1, //178
    -1, //179
    -1, //180
    -1, //181
    -1, //182
    -1, //183
    -1, //184
    -1, //185
    -1, //186
    -1, //187
    -1, //188
    -1, //189
    -1, //190
    -1, //191
    -1, //192
    -1, //193
    -1, //194
    -1, //195
    -1, //196
    -1, //197
    -1, //198
    -1, //199
    -1, //200
    -1, //201
    -1, //202
    -1, //203
    -1, //204
    -1, //205
    -1, //206
    -1, //207
    -1, //208
    -1, //209
    -1, //210
    -1, //211
    -1, //212
    -1, //213
    -1, //214
    -1, //215
    -1, //216
    -1, //217
    -1, //218
    -1, //219
    -1, //220
    -1, //221
    -1, //222
    -1, //223
    -1, //224
    -1, //225
    -1, //226
    -1, //227
    -1, //228
    -1, //229
    -1, //230
    -1, //231
    -1, //232
    -1, //233
    -1, //234
    -1, //235
    -1, //236
    -1, //237
    -1, //238
    -1, //239
    -1, //240
    -1, //241
    -1, //242
    -1, //243
    -1, //244
    -1, //245
    -1, //246
    -1, //247
    -1, //248
    -1, //249
    -1, //250
    -1, //251
    -1, //252
    -1, //253
    -1, //254
    -1, //255
    ];

    #endregion Fields
}