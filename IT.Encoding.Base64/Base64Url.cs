using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

namespace IT.Encoding.Base64;

public static class Base64Url
{
    #region Encode128

    public static void VectorEncode128(ref byte src, ref byte encoded, byte[] map)
    {
        if (BitConverter.IsLittleEndian && Vector128.IsHardwareAccelerated && (Ssse3.IsSupported || AdvSimd.Arm64.IsSupported))
        {
            Vector128<sbyte> v;

            if (Ssse3.IsSupported)
            {
                v = Ssse3.Shuffle(Vector128.LoadUnsafe(ref src).AsSByte(), Vector128.Create(
                     1, 0, 2, 1,
                     4, 3, 5, 4,
                     7, 6, 8, 7,
                    10, 9, 11, 10
                ));
                v = Sse2.MultiplyHigh(
                       (v & Vector128.Create(0x0fc0fc00).AsSByte()).AsUInt16(), Vector128.Create(0x04000040).AsUInt16()).AsSByte() |
                      ((v & Vector128.Create(0x003f03f0).AsSByte()).AsInt16() * Vector128.Create(0x01000010).AsInt16()).AsSByte();

                v += Ssse3.Shuffle(Vector128.Create(
                           65, 71, -4, -4,
                           -4, -4, -4, -4,
                           -4, -4, -4, -4,
                           -17, 32, 0, 0
                        ),
                        Sse2.SubtractSaturate(v.AsByte(), Vector128.Create((byte)51)).AsSByte() -
                        Vector128.GreaterThan(v, Vector128.Create((sbyte)25)));
            }
            else
            {
                v = AdvSimd.Arm64.VectorTableLookup(Vector128.LoadUnsafe(ref src).AsSByte().AsByte(), Vector128.Create(
                     1, 0, 2, 1,
                     4, 3, 5, 4,
                     7, 6, 8, 7,
                    10, 9, 11, 10
                ).AsByte() & Vector128.Create((byte)0x8f)).AsSByte();

                var temp = (v & Vector128.Create(0x0fc0fc00).AsSByte()).AsUInt16();

                v = AdvSimd.Arm64.ZipLow(
                        Vector128.ShiftRightLogical(AdvSimd.Arm64.UnzipEven(temp.AsUInt16(), temp.AsUInt16()), 10),
                        Vector128.ShiftRightLogical(AdvSimd.Arm64.UnzipOdd(temp.AsUInt16(), temp.AsUInt16()), 6)).AsSByte() |
                      ((v & Vector128.Create(0x003f03f0).AsSByte()).AsInt16() * Vector128.Create(0x01000010).AsInt16()).AsSByte();

                v += AdvSimd.Arm64.VectorTableLookup(Vector128.Create(
                           65, 71, -4, -4,
                           -4, -4, -4, -4,
                           -4, -4, -4, -4,
                           -17, 32, 0, 0
                        ).AsByte(),
                        (AdvSimd.SubtractSaturate(v.AsByte(), Vector128.Create((byte)51)).AsSByte() -
                         Vector128.GreaterThan(v, Vector128.Create((sbyte)25))).AsByte() & Vector128.Create((byte)0x8f)).AsSByte();
            }

            v.AsByte().StoreUnsafe(ref encoded);

            UnsafeBase64.Encode24(ref Unsafe.AddByteOffset(ref src, 12), ref Unsafe.AddByteOffset(ref encoded, 16), map);
            UnsafeBase64.Encode8(ref Unsafe.AddByteOffset(ref src, 15), ref Unsafe.AddByteOffset(ref encoded, 20), map);
        }
        else
        {
            UnsafeBase64.Encode128(ref src, ref encoded, map);
        }
    }

    public static void VectorEncode128(ref byte src, ref char encoded, char[] map)
    {
        if (BitConverter.IsLittleEndian && Vector128.IsHardwareAccelerated && (Ssse3.IsSupported || AdvSimd.Arm64.IsSupported))
        {
            if (Ssse3.IsSupported)
            {
                Vector128<sbyte> v = Ssse3.Shuffle(Vector128.LoadUnsafe(ref src).AsSByte(), Vector128.Create(
                     1, 0, 2, 1,
                     4, 3, 5, 4,
                     7, 6, 8, 7,
                    10, 9, 11, 10
                ));
                v = Sse2.MultiplyHigh(
                       (v & Vector128.Create(0x0fc0fc00).AsSByte()).AsUInt16(), Vector128.Create(0x04000040).AsUInt16()).AsSByte() |
                      ((v & Vector128.Create(0x003f03f0).AsSByte()).AsInt16() * Vector128.Create(0x01000010).AsInt16()).AsSByte();

                v += Ssse3.Shuffle(Vector128.Create(
                           65, 71, -4, -4,
                           -4, -4, -4, -4,
                           -4, -4, -4, -4,
                           -17, 32, 0, 0
                        ),
                        Sse2.SubtractSaturate(v.AsByte(), Vector128.Create((byte)51)).AsSByte() -
                        Vector128.GreaterThan(v, Vector128.Create((sbyte)25)));

                ref short ptr = ref Unsafe.As<char, short>(ref encoded);
                Sse2.UnpackLow(v, Vector128<sbyte>.Zero).AsInt16().StoreUnsafe(ref ptr);
                Sse2.UnpackHigh(v, Vector128<sbyte>.Zero).AsInt16().StoreUnsafe(ref ptr, 8);
            }
            else
            {
                Vector128<sbyte> v = AdvSimd.Arm64.VectorTableLookup(Vector128.LoadUnsafe(ref src).AsSByte().AsByte(), Vector128.Create(
                     1, 0, 2, 1,
                     4, 3, 5, 4,
                     7, 6, 8, 7,
                    10, 9, 11, 10
                ).AsByte() & Vector128.Create((byte)0x8f)).AsSByte();

                var temp = (v & Vector128.Create(0x0fc0fc00).AsSByte()).AsUInt16();

                v = AdvSimd.Arm64.ZipLow(
                        Vector128.ShiftRightLogical(AdvSimd.Arm64.UnzipEven(temp.AsUInt16(), temp.AsUInt16()), 10),
                        Vector128.ShiftRightLogical(AdvSimd.Arm64.UnzipOdd(temp.AsUInt16(), temp.AsUInt16()), 6)).AsSByte() |
                      ((v & Vector128.Create(0x003f03f0).AsSByte()).AsInt16() * Vector128.Create(0x01000010).AsInt16()).AsSByte();

                v += AdvSimd.Arm64.VectorTableLookup(Vector128.Create(
                           65, 71, -4, -4,
                           -4, -4, -4, -4,
                           -4, -4, -4, -4,
                           -17, 32, 0, 0
                        ).AsByte(),
                        (AdvSimd.SubtractSaturate(v.AsByte(), Vector128.Create((byte)51)).AsSByte() -
                         Vector128.GreaterThan(v, Vector128.Create((sbyte)25))).AsByte() & Vector128.Create((byte)0x8f)).AsSByte();

                ref short ptr = ref Unsafe.As<char, short>(ref encoded);

                (Vector128<short> lower, Vector128<short> upper) = Vector128.Widen(v);

                lower.StoreUnsafe(ref ptr);
                upper.StoreUnsafe(ref ptr, 8);
            }

            UnsafeBase64.Encode24(ref Unsafe.AddByteOffset(ref src, 12), ref Unsafe.AddByteOffset(ref encoded, 32), map);
            UnsafeBase64.Encode8(ref Unsafe.AddByteOffset(ref src, 15), ref Unsafe.AddByteOffset(ref encoded, 40), map);
        }
        else
        {
            UnsafeBase64.Encode128(ref src, ref encoded, map);
        }
    }

    public static bool TryVectorDecode128(ref byte encoded, ref byte src, sbyte[] map)
    {
        if (BitConverter.IsLittleEndian && Vector128.IsHardwareAccelerated && (Ssse3.IsSupported || AdvSimd.Arm64.IsSupported))
        {
            Vector128<sbyte> vector;

            if (Ssse3.IsSupported)
            {
                vector = Vector128.LoadUnsafe(ref encoded).AsSByte();

                Vector128<sbyte> maskSlashOrUnderscore = Vector128.Create((sbyte)0x5F);//_
                Vector128<sbyte> hiNibbles = Vector128.ShiftRightLogical(vector.AsInt32(), 4).AsSByte() & maskSlashOrUnderscore;
                Vector128<sbyte> eq5F = Vector128.Equals(vector, maskSlashOrUnderscore);

                // Take care as arguments are flipped in order!
                //Vector128<sbyte> outside = Sse2.AndNot(eq5F, below | above);
                if (Vector128.AndNot(
                    Vector128.LessThan(vector, Ssse3.Shuffle(Vector128.Create(
                        1, 1, 0x2d, 0x30,
                        0x41, 0x50, 0x61, 0x70,
                        1, 1, 1, 1,
                        1, 1, 1, 1
                    ), hiNibbles)) |
                    Vector128.GreaterThan(vector, Ssse3.Shuffle(Vector128.Create(
                       0x00, 0x00, 0x2d, 0x39,
                       0x4f, 0x5a, 0x6f, 0x7a,
                       0x00, 0x00, 0x00, 0x00,
                       0x00, 0x00, 0x00, 0x00
                    ), hiNibbles)), eq5F) != Vector128<sbyte>.Zero) return false;

                vector = Ssse3.Shuffle(
                    Sse2.MultiplyAddAdjacent(
                        Ssse3.MultiplyAddAdjacent(
                            (vector + Ssse3.Shuffle(Vector128.Create(
                                0, 0, 17, 4,
                              -65, -65, -71, -71,
                                0, 0, 0, 0,
                                0, 0, 0, 0
                            ), hiNibbles) + (eq5F & Vector128.Create((sbyte)33))).AsByte(),
                            Vector128.Create(0x01400140).AsSByte()),
                        Vector128.Create(0x00011000).AsInt16()).AsSByte(),
                    Vector128.Create(
                        2, 1, 0, 6,
                        5, 4, 10, 9,
                        8, 14, 13, 12,
                       -1, -1, -1, -1
                    ));
            }
            else
            {
                throw new NotImplementedException();
            }

            vector.AsByte().StoreUnsafe(ref src);

            return UnsafeBase64.TryDecode32(ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 12), map);
        }
        else
        {
            return UnsafeBase64.TryDecode128(ref encoded, ref src, map);
        }
    }

    public static bool TryVectorDecode128(ref char encoded, ref byte src, sbyte[] map)
    {
        if (BitConverter.IsLittleEndian && Vector128.IsHardwareAccelerated && (Ssse3.IsSupported || AdvSimd.Arm64.IsSupported))
        {
            Vector128<sbyte> vector;

            if (Ssse3.IsSupported)
            {
                Vector128<sbyte> lutLo = Vector128.Create(
                    1, 1, 0x2d, 0x30,
                    0x41, 0x50, 0x61, 0x70,
                    1, 1, 1, 1,
                    1, 1, 1, 1
                );
                Vector128<sbyte> lutHi = Vector128.Create(
                   0x00, 0x00, 0x2d, 0x39,
                   0x4f, 0x5a, 0x6f, 0x7a,
                   0x00, 0x00, 0x00, 0x00,
                   0x00, 0x00, 0x00, 0x00
                );
                Vector128<sbyte> lutShift = Vector128.Create(
                    0, 0, 17, 4,
                  -65, -65, -71, -71,
                    0, 0, 0, 0,
                    0, 0, 0, 0
                );
                Vector128<sbyte> mergeConstant0 = Vector128.Create(0x01400140).AsSByte();
                Vector128<short> mergeConstant1 = Vector128.Create(0x00011000).AsInt16();
                Vector128<sbyte> shuffleVec = Vector128.Create(
                    2, 1, 0, 6,
                    5, 4, 10, 9,
                    8, 14, 13, 12,
                   -1, -1, -1, -1
                );

                Vector128<sbyte> maskSlashOrUnderscore = Vector128.Create((sbyte)0x5F);//_
                Vector128<sbyte> shiftForSlashOrUnderscore = Vector128.Create((sbyte)33);

                ref short ptr = ref Unsafe.As<char, short>(ref encoded);

                vector = Sse2.PackUnsignedSaturate(Vector128.LoadUnsafe(ref ptr), Vector128.LoadUnsafe(ref ptr, 8)).AsSByte();

                Vector128<sbyte> hiNibbles = Vector128.ShiftRightLogical(vector.AsInt32(), 4).AsSByte() & maskSlashOrUnderscore;

                //////////////////////////////////////////
                //if (!TBase64Encoder.TryDecode128Core(vector, hiNibbles, maskSlashOrUnderscore, lutLo, lutHi, lutShift, shiftForSlashOrUnderscore, out vector))
                //    return false;

                Vector128<sbyte> lowerBound = Ssse3.Shuffle(lutLo, hiNibbles);
                Vector128<sbyte> upperBound = Ssse3.Shuffle(lutHi, hiNibbles);

                Vector128<sbyte> below = Vector128.LessThan(vector, lowerBound);
                Vector128<sbyte> above = Vector128.GreaterThan(vector, upperBound);
                Vector128<sbyte> eq5F = Vector128.Equals(vector, maskSlashOrUnderscore);

                // Take care as arguments are flipped in order!
                //Vector128<sbyte> outside = Sse2.AndNot(eq5F, below | above);
                Vector128<sbyte> outside = Vector128.AndNot(below | above, eq5F);

                if (outside != Vector128<sbyte>.Zero) return false;

                Vector128<sbyte> shift = Ssse3.Shuffle(lutShift, hiNibbles);
                vector += shift;

                vector += (eq5F & shiftForSlashOrUnderscore);

                //////////////////////////////////////////

                Vector128<short> merge_ab_and_bc = Ssse3.MultiplyAddAdjacent(vector.AsByte(), mergeConstant0);
                Vector128<int> output = Sse2.MultiplyAddAdjacent(merge_ab_and_bc, mergeConstant1);

                vector = Ssse3.Shuffle(output.AsSByte(), shuffleVec);
            }
            else
            {
                throw new NotImplementedException();
            }

            vector.AsByte().StoreUnsafe(ref src);

            return UnsafeBase64.TryDecode32(ref Unsafe.AddByteOffset(ref encoded, 32), ref Unsafe.AddByteOffset(ref src, 12), map);
        }
        else
        {
            return UnsafeBase64.TryDecode128(ref encoded, ref src, map);
        }
    }

    public static bool TryDecode128(ref char encoded, ref byte src, sbyte[] map)
    {
        throw new NotImplementedException();
    }

    public static EncodingStatus TryEncode128(UInt128 value, Span<byte> encoded)
    {
        if (encoded.Length < 22) return EncodingStatus.InvalidDestinationLength;

        VectorEncode128(ref Unsafe.As<UInt128, byte>(ref value), ref MemoryMarshal.GetReference(encoded), Bytes);

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode128(UInt128 value, Span<char> encoded)
    {
        if (encoded.Length < 22) return EncodingStatus.InvalidDestinationLength;

        VectorEncode128(ref Unsafe.As<UInt128, byte>(ref value), ref MemoryMarshal.GetReference(encoded), Chars);

        return EncodingStatus.Done;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode128(UInt128 value, Span<byte> encoded)
    {
        if (encoded.Length < 22) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 22");

        VectorEncode128(ref Unsafe.As<UInt128, byte>(ref value), ref MemoryMarshal.GetReference(encoded), Bytes);
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode128(UInt128 value, Span<char> encoded)
    {
        if (encoded.Length < 22) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 22");

        VectorEncode128(ref Unsafe.As<UInt128, byte>(ref value), ref MemoryMarshal.GetReference(encoded), Chars);
    }

    public static byte[] Encode128ToBytes(UInt128 value)
    {
        var encoded = new byte[22];

        VectorEncode128(ref Unsafe.As<UInt128, byte>(ref value), ref encoded[0], Bytes);

        return encoded;
    }

    public static char[] Encode128ToChars(UInt128 value)
    {
        var encoded = new char[22];

        VectorEncode128(ref Unsafe.As<UInt128, byte>(ref value), ref encoded[0], Chars);

        return encoded;
    }

    public static string Encode128ToString(UInt128 value) => string.Create(22, value, static (chars, value) =>
    {
        VectorEncode128(ref Unsafe.As<UInt128, byte>(ref value), ref MemoryMarshal.GetReference(chars), Chars);
    });

    #endregion Encode128

    #region Decode128

    public static EncodingStatus TryDecode128(ReadOnlySpan<byte> encoded, out UInt128 value)
    {
        value = default;
        if (encoded.Length != 22) return EncodingStatus.InvalidDataLength;

        if (!TryVectorDecode128(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<UInt128, byte>(ref value), Map))
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

        if (!TryVectorDecode128(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<UInt128, byte>(ref value), Map))
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
        if (!UnsafeBase64.TryDecode128(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<UInt128, byte>(ref value), Map, out invalid))
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
        if (!UnsafeBase64.TryDecode128(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<UInt128, byte>(ref value), Map, out invalid))
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

        if (!UnsafeBase64.TryDecode128(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<UInt128, byte>(ref value), Map, out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid byte");

        return value;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static UInt128 Decode128(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 22) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 22");

        UInt128 value = 0;

        if (!UnsafeBase64.TryDecode128(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<UInt128, byte>(ref value), Map, out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid char");

        return value;
    }

    #endregion Decode128

    #region Encode72

    public static EncodingStatus TryEncode72(ReadOnlySpan<byte> bytes, Span<byte> encoded)
    {
        if (bytes.Length != 9) return EncodingStatus.InvalidDataLength;
        if (encoded.Length < 12) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode72(ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded), Bytes);

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode72(ReadOnlySpan<byte> bytes, Span<char> encoded)
    {
        if (bytes.Length != 9) return EncodingStatus.InvalidDataLength;
        if (encoded.Length < 12) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode72(ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded), Chars);

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode72<T>(T value, Span<byte> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 9) return EncodingStatus.InvalidDataLength;
        if (encoded.Length < 12) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode72(ref Unsafe.As<T, byte>(ref value), ref MemoryMarshal.GetReference(encoded), Bytes);

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode72<T>(T value, Span<char> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 9) return EncodingStatus.InvalidDataLength;
        if (encoded.Length < 12) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode72(ref Unsafe.As<T, byte>(ref value), ref MemoryMarshal.GetReference(encoded), Chars);

        return EncodingStatus.Done;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode72(ReadOnlySpan<byte> bytes, Span<byte> encoded)
    {
        if (bytes.Length != 9) throw new ArgumentOutOfRangeException(nameof(bytes), bytes.Length, "length != 9");
        if (encoded.Length < 12) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 12");

        UnsafeBase64.Encode72(ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded), Bytes);
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode72(ReadOnlySpan<byte> bytes, Span<char> encoded)
    {
        if (bytes.Length != 9) throw new ArgumentOutOfRangeException(nameof(bytes), bytes.Length, "length != 9");
        if (encoded.Length < 12) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 12");

        UnsafeBase64.Encode72(ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded), Chars);
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode72<T>(T value, Span<byte> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 9) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 9");
        if (encoded.Length < 12) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 12");

        UnsafeBase64.Encode72(ref Unsafe.As<T, byte>(ref value), ref MemoryMarshal.GetReference(encoded), Bytes);
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode72<T>(T value, Span<char> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 9) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 9");
        if (encoded.Length < 12) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 12");

        UnsafeBase64.Encode72(ref Unsafe.As<T, byte>(ref value), ref MemoryMarshal.GetReference(encoded), Chars);
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static byte[] Encode72ToBytes<T>(T value) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 9) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 9");

        var encoded = new byte[12];

        UnsafeBase64.Encode72(ref Unsafe.As<T, byte>(ref value), ref encoded[0], Bytes);

        return encoded;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static char[] Encode72ToChars<T>(T value) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 9) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 9");

        var encoded = new char[12];

        UnsafeBase64.Encode72(ref Unsafe.As<T, byte>(ref value), ref encoded[0], Chars);

        return encoded;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static string Encode72ToString<T>(T value) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 9) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 9");

        return string.Create(12, value, static (chars, value) =>
        {
            UnsafeBase64.Encode72(ref Unsafe.As<T, byte>(ref value), ref MemoryMarshal.GetReference(chars), Chars);
        });
    }

    #endregion Encode72

    #region Decode72

    public static EncodingStatus TryDecode72<T>(ReadOnlySpan<byte> encoded, out T value) where T : unmanaged
    {
        value = default;
        if (encoded.Length != 12) return EncodingStatus.InvalidDataLength;
        if (Unsafe.SizeOf<T>() != 9) return EncodingStatus.InvalidDestinationLength;

        if (!UnsafeBase64.TryDecode72(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<T, byte>(ref value), Map))
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

        if (!UnsafeBase64.TryDecode72(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<T, byte>(ref value), Map))
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
        if (!UnsafeBase64.TryDecode72(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<T, byte>(ref value), Map, out invalid))
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
        if (!UnsafeBase64.TryDecode72(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<T, byte>(ref value), Map, out invalid))
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

        if (!UnsafeBase64.TryDecode72(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<T, byte>(ref value), Map, out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "Invalid byte");

        return value;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static T Decode72<T>(ReadOnlySpan<char> encoded) where T : unmanaged
    {
        if (Unsafe.SizeOf<T>() != 9) throw new ArgumentOutOfRangeException(nameof(T), Unsafe.SizeOf<T>(), $"SizeOf<{typeof(T).FullName}> != 9");
        if (encoded.Length != 12) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 12");

        T value = default;

        if (!UnsafeBase64.TryDecode72(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<T, byte>(ref value), Map, out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "Invalid char");

        return value;
    }

    #endregion Decode72

    #region Encode64

    public static EncodingStatus TryEncode64(ReadOnlySpan<byte> bytes, Span<byte> encoded)
    {
        if (bytes.Length != 8) return EncodingStatus.InvalidDataLength;
        if (encoded.Length < 11) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode64(ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded), Bytes);

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode64(ReadOnlySpan<byte> bytes, Span<char> encoded)
    {
        if (bytes.Length != 8) return EncodingStatus.InvalidDataLength;
        if (encoded.Length < 11) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode64(ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded), Chars);

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode64(ulong value, Span<byte> encoded)
    {
        if (encoded.Length < 11) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode64(ref Unsafe.As<ulong, byte>(ref value), ref MemoryMarshal.GetReference(encoded), Bytes);

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode64(ulong value, Span<char> encoded)
    {
        if (encoded.Length < 11) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode64(ref Unsafe.As<ulong, byte>(ref value), ref MemoryMarshal.GetReference(encoded), Chars);

        return EncodingStatus.Done;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode64(ReadOnlySpan<byte> bytes, Span<byte> encoded)
    {
        if (bytes.Length != 8) throw new ArgumentOutOfRangeException(nameof(bytes), bytes.Length, "length != 8");
        if (encoded.Length < 11) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 11");

        UnsafeBase64.Encode64(ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded), Bytes);
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode64(ReadOnlySpan<byte> bytes, Span<char> encoded)
    {
        if (bytes.Length != 8) throw new ArgumentOutOfRangeException(nameof(bytes), bytes.Length, "length != 8");
        if (encoded.Length < 11) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 11");

        UnsafeBase64.Encode64(ref MemoryMarshal.GetReference(bytes), ref MemoryMarshal.GetReference(encoded), Chars);
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode64(ulong value, Span<byte> encoded)
    {
        if (encoded.Length < 11) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 11");

        UnsafeBase64.Encode64(ref Unsafe.As<ulong, byte>(ref value), ref MemoryMarshal.GetReference(encoded), Bytes);
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode64(ulong value, Span<char> encoded)
    {
        if (encoded.Length < 11) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 11");

        UnsafeBase64.Encode64(ref Unsafe.As<ulong, byte>(ref value), ref MemoryMarshal.GetReference(encoded), Chars);
    }

    public static byte[] Encode64ToBytes(ulong value)
    {
        var encoded = new byte[11];

        UnsafeBase64.Encode64(ref Unsafe.As<ulong, byte>(ref value), ref encoded[0], Bytes);

        return encoded;
    }

    public static char[] Encode64ToChars(ulong value)
    {
        var encoded = new char[11];

        UnsafeBase64.Encode64(ref Unsafe.As<ulong, byte>(ref value), ref encoded[0], Chars);

        return encoded;
    }

    public static string Encode64ToString(ulong value) => string.Create(11, value, static (chars, value) =>
    {
        UnsafeBase64.Encode64(ref Unsafe.As<ulong, byte>(ref value), ref MemoryMarshal.GetReference(chars), Chars);
    });

    #endregion Encode64

    #region Decode64

    public static EncodingStatus TryDecode64(ReadOnlySpan<byte> encoded, out ulong value)
    {
        value = default;
        if (encoded.Length != 11) return EncodingStatus.InvalidDataLength;

        if (!UnsafeBase64.TryDecode64(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<ulong, byte>(ref value), Map))
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

        if (!UnsafeBase64.TryDecode64(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<ulong, byte>(ref value), Map))
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
        if (!UnsafeBase64.TryDecode64(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<ulong, byte>(ref value), Map, out invalid))
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
        if (!UnsafeBase64.TryDecode64(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<ulong, byte>(ref value), Map, out invalid))
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

        if (!UnsafeBase64.TryDecode64(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<ulong, byte>(ref value), Map, out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid byte");

        return value;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static ulong Decode64(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 11) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 11");

        ulong value = 0;

        if (!UnsafeBase64.TryDecode64(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<ulong, byte>(ref value), Map, out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid char");

        return value;
    }

    #endregion Decode64

    #region Encode32

    public static EncodingStatus TryEncode32(uint value, Span<byte> encoded)
    {
        if (encoded.Length < 6) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode32(ref Unsafe.As<uint, byte>(ref value), ref MemoryMarshal.GetReference(encoded), Bytes);

        return EncodingStatus.Done;
    }

    public static EncodingStatus TryEncode32(uint value, Span<char> encoded)
    {
        if (encoded.Length < 6) return EncodingStatus.InvalidDestinationLength;

        UnsafeBase64.Encode32(ref Unsafe.As<uint, byte>(ref value), ref MemoryMarshal.GetReference(encoded), Chars);

        return EncodingStatus.Done;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode32(uint value, Span<byte> encoded)
    {
        if (encoded.Length < 6) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 6");

        UnsafeBase64.Encode32(ref Unsafe.As<uint, byte>(ref value), ref MemoryMarshal.GetReference(encoded), Bytes);
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static void Encode32(uint value, Span<char> encoded)
    {
        if (encoded.Length < 6) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length < 6");

        UnsafeBase64.Encode32(ref Unsafe.As<uint, byte>(ref value), ref MemoryMarshal.GetReference(encoded), Chars);
    }

    public static byte[] Encode32ToBytes(uint value)
    {
        var encoded = new byte[6];

        UnsafeBase64.Encode32(ref Unsafe.As<uint, byte>(ref value), ref encoded[0], Bytes);

        return encoded;
    }

    public static char[] Encode32ToChars(uint value)
    {
        var encoded = new char[6];

        UnsafeBase64.Encode32(ref Unsafe.As<uint, byte>(ref value), ref encoded[0], Chars);

        return encoded;
    }

    public static string Encode32ToString(uint value) => string.Create(6, value, static (chars, value) =>
    {
        UnsafeBase64.Encode32(ref Unsafe.As<uint, byte>(ref value), ref MemoryMarshal.GetReference(chars), Chars);
    });

    #endregion Encode32

    #region Decode32

    public static EncodingStatus TryDecode32(ReadOnlySpan<byte> encoded, out uint value)
    {
        value = default;
        if (encoded.Length != 6) return EncodingStatus.InvalidDataLength;

        if (!UnsafeBase64.TryDecode32(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<uint, byte>(ref value), Map))
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

        if (!UnsafeBase64.TryDecode32(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<uint, byte>(ref value), Map))
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
        if (!UnsafeBase64.TryDecode32(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<uint, byte>(ref value), Map, out invalid))
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
        if (!UnsafeBase64.TryDecode32(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<uint, byte>(ref value), Map, out invalid))
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

        if (!UnsafeBase64.TryDecode32(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<uint, byte>(ref value), Map, out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid byte");

        return value;
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    public static uint Decode32(ReadOnlySpan<char> encoded)
    {
        if (encoded.Length != 6) throw new ArgumentOutOfRangeException(nameof(encoded), encoded.Length, "length != 6");

        uint value = 0;

        if (!UnsafeBase64.TryDecode32(ref MemoryMarshal.GetReference(encoded), ref Unsafe.As<uint, byte>(ref value), Map, out var invalid))
            throw new ArgumentOutOfRangeException(nameof(encoded), invalid, "invalid char");

        return value;
    }

    #endregion Decode32

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