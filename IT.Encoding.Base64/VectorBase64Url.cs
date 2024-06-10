using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

namespace IT.Encoding.Base64;

public static class VectorBase64Url
{
    public static void Encode128(ref byte src, ref byte encoded)
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

            var map = Base64Url.Bytes;
            UnsafeBase64.Encode24(map, ref Unsafe.AddByteOffset(ref src, 12), ref Unsafe.AddByteOffset(ref encoded, 16));
            UnsafeBase64.Encode8(map, ref Unsafe.AddByteOffset(ref src, 15), ref Unsafe.AddByteOffset(ref encoded, 20));
        }
        else
        {
            UnsafeBase64.Encode128(Base64Url.Bytes, ref src, ref encoded);
        }
    }

    public static void Encode128(ref byte src, ref char encoded)
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

            var map = Base64Url.Chars;
            UnsafeBase64.Encode24(map, ref Unsafe.AddByteOffset(ref src, 12), ref Unsafe.AddByteOffset(ref encoded, 32));
            UnsafeBase64.Encode8(map, ref Unsafe.AddByteOffset(ref src, 15), ref Unsafe.AddByteOffset(ref encoded, 40));
        }
        else
        {
            UnsafeBase64.Encode128(Base64Url.Chars, ref src, ref encoded);
        }
    }

    public static bool TryDecode128(ref byte encoded, ref byte src)
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
                if (!IsValid128(vector, hiNibbles, eq5F))
                    return false;
                vector = Decode128(vector, hiNibbles, eq5F);
            }
            else
            {
                throw new NotImplementedException();
            }
            vector.AsByte().StoreUnsafe(ref src);
            return UnsafeBase64.TryDecode32(Base64Url.Map, ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 12));
        }
        else
        {
            return UnsafeBase64.TryDecode128(Base64Url.Map, ref encoded, ref src);
        }
    }

    public static bool TryDecode128(ref char encoded, ref byte src)
    {
        if (BitConverter.IsLittleEndian && Vector128.IsHardwareAccelerated && (Ssse3.IsSupported || AdvSimd.Arm64.IsSupported))
        {
            Vector128<sbyte> vector;
            if (Ssse3.IsSupported)
            {
                vector = xSse2.LoadUnsafe(ref encoded).AsSByte();
                Vector128<sbyte> maskSlashOrUnderscore = Vector128.Create((sbyte)0x5F);//_
                Vector128<sbyte> hiNibbles = Vector128.ShiftRightLogical(vector.AsInt32(), 4).AsSByte() & maskSlashOrUnderscore;
                Vector128<sbyte> eq5F = Vector128.Equals(vector, maskSlashOrUnderscore);
                if (!IsValid128(vector, hiNibbles, eq5F))
                    return false;
                vector = Decode128(vector, hiNibbles, eq5F);
            }
            else
            {
                throw new NotImplementedException();
            }
            vector.AsByte().StoreUnsafe(ref src);
            return UnsafeBase64.TryDecode32(Base64Url.Map, ref Unsafe.AddByteOffset(ref encoded, 32), ref Unsafe.AddByteOffset(ref src, 12));
        }
        else
        {
            return UnsafeBase64.TryDecode128(Base64Url.Map, ref encoded, ref src);
        }
    }

    public static bool TryDecode128(ref byte encoded, ref byte src, out byte invalid)
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
                if (!IsValid128(vector, hiNibbles, eq5F))
                {
                    invalid = UnsafeBase64.GetInvalid(Base64Url.Map, ref encoded, 16);
                    return false;
                }
                vector = Decode128(vector, hiNibbles, eq5F);
            }
            else
            {
                throw new NotImplementedException();
            }
            vector.AsByte().StoreUnsafe(ref src);
            return UnsafeBase64.TryDecode32(Base64Url.Map, ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 12), out invalid);
        }
        else
        {
            return UnsafeBase64.TryDecode128(Base64Url.Map, ref encoded, ref src, out invalid);
        }
    }

    public static bool TryDecode128(ref char encoded, ref byte src, out char invalid)
    {
        if (BitConverter.IsLittleEndian && Vector128.IsHardwareAccelerated && (Ssse3.IsSupported || AdvSimd.Arm64.IsSupported))
        {
            Vector128<sbyte> vector;
            if (Ssse3.IsSupported)
            {
                vector = xSse2.LoadUnsafe(ref encoded).AsSByte();
                Vector128<sbyte> maskSlashOrUnderscore = Vector128.Create((sbyte)0x5F);//_
                Vector128<sbyte> hiNibbles = Vector128.ShiftRightLogical(vector.AsInt32(), 4).AsSByte() & maskSlashOrUnderscore;
                Vector128<sbyte> eq5F = Vector128.Equals(vector, maskSlashOrUnderscore);
                if (!IsValid128(vector, hiNibbles, eq5F))
                {
                    invalid = UnsafeBase64.GetInvalid(Base64Url.Map, ref encoded, 32);
                    return false;
                }
                vector = Decode128(vector, hiNibbles, eq5F);
            }
            else
            {
                throw new NotImplementedException();
            }
            vector.AsByte().StoreUnsafe(ref src);
            return UnsafeBase64.TryDecode32(Base64Url.Map, ref Unsafe.AddByteOffset(ref encoded, 32), ref Unsafe.AddByteOffset(ref src, 12), out invalid);
        }
        else
        {
            return UnsafeBase64.TryDecode128(Base64Url.Map, ref encoded, ref src, out invalid);
        }
    }

    public static bool IsValid128(ref byte encoded)
    {
        if (BitConverter.IsLittleEndian && Vector128.IsHardwareAccelerated && (Ssse3.IsSupported || AdvSimd.Arm64.IsSupported))
        {
            if (Ssse3.IsSupported)
            {
                if (!IsValid128(Vector128.LoadUnsafe(ref encoded).AsSByte()))
                    return false;
            }
            else
            {
                throw new NotImplementedException();
            }
            return UnsafeBase64.IsValid32(Base64Url.Map, ref Unsafe.AddByteOffset(ref encoded, 16));
        }
        else
        {
            return UnsafeBase64.IsValid128(Base64Url.Map, ref encoded);
        }
    }

    public static bool IsValid128(ref char encoded)
    {
        if (BitConverter.IsLittleEndian && Vector128.IsHardwareAccelerated && (Ssse3.IsSupported || AdvSimd.Arm64.IsSupported))
        {
            if (Ssse3.IsSupported)
            {
                if (!IsValid128(xSse2.LoadUnsafe(ref encoded).AsSByte()))
                    return false;
            }
            else
            {
                throw new NotImplementedException();
            }
            return UnsafeBase64.IsValid32(Base64Url.Map, ref Unsafe.AddByteOffset(ref encoded, 32));
        }
        else
        {
            return UnsafeBase64.IsValid128(Base64Url.Map, ref encoded);
        }
    }

    public static bool IsValid128(ref byte encoded, out byte invalid)
    {
        if (BitConverter.IsLittleEndian && Vector128.IsHardwareAccelerated && (Ssse3.IsSupported || AdvSimd.Arm64.IsSupported))
        {
            if (Ssse3.IsSupported)
            {
                if (!IsValid128(Vector128.LoadUnsafe(ref encoded).AsSByte()))
                {
                    invalid = UnsafeBase64.GetInvalid(Base64Url.Map, ref encoded, 16);
                    return false;
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            return UnsafeBase64.IsValid32(Base64Url.Map, ref Unsafe.AddByteOffset(ref encoded, 16), out invalid);
        }
        else
        {
            return UnsafeBase64.IsValid128(Base64Url.Map, ref encoded, out invalid);
        }
    }

    public static bool IsValid128(ref char encoded, out char invalid)
    {
        if (BitConverter.IsLittleEndian && Vector128.IsHardwareAccelerated && (Ssse3.IsSupported || AdvSimd.Arm64.IsSupported))
        {
            if (Ssse3.IsSupported)
            {
                if (!IsValid128(xSse2.LoadUnsafe(ref encoded).AsSByte()))
                {
                    invalid = UnsafeBase64.GetInvalid(Base64Url.Map, ref encoded, 32);
                    return false;
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            return UnsafeBase64.IsValid32(Base64Url.Map, ref Unsafe.AddByteOffset(ref encoded, 32), out invalid);
        }
        else
        {
            return UnsafeBase64.IsValid128(Base64Url.Map, ref encoded, out invalid);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsValid128(Vector128<sbyte> vector)
    {
        Vector128<sbyte> maskSlashOrUnderscore = Vector128.Create((sbyte)0x5F);//_
        Vector128<sbyte> hiNibbles = Vector128.ShiftRightLogical(vector.AsInt32(), 4).AsSByte() & maskSlashOrUnderscore;
        Vector128<sbyte> eq5F = Vector128.Equals(vector, maskSlashOrUnderscore);
        return IsValid128(vector, hiNibbles, eq5F);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsValid128(Vector128<sbyte> vector, Vector128<sbyte> hiNibbles, Vector128<sbyte> eq5F)
    {
        // Take care as arguments are flipped in order!
        //Vector128<sbyte> outside = Sse2.AndNot(eq5F, below | above);
        return Vector128.AndNot(
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
            ), hiNibbles)), eq5F) == Vector128<sbyte>.Zero;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector128<sbyte> Decode128(Vector128<sbyte> vector, Vector128<sbyte> hiNibbles, Vector128<sbyte> eq5F)
        => Ssse3.Shuffle(
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