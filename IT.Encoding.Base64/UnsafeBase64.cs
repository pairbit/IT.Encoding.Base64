using System;
using System.Runtime.CompilerServices;

namespace IT.Encoding.Base64;

public static class UnsafeBase64
{
    #region Encode128

    public static void Encode128(byte[] map, ref byte src, ref byte encoded)
    {
        Encode24(map, ref src, ref encoded);
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 4));
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 6), ref Unsafe.AddByteOffset(ref encoded, 8));
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 9), ref Unsafe.AddByteOffset(ref encoded, 12));
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 12), ref Unsafe.AddByteOffset(ref encoded, 16));
        Encode8(map, ref Unsafe.AddByteOffset(ref src, 15), ref Unsafe.AddByteOffset(ref encoded, 20));
    }

    public static void Encode128(char[] map, ref byte src, ref char encoded)
    {
        Encode24(map, ref src, ref encoded);
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 8));
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 6), ref Unsafe.AddByteOffset(ref encoded, 16));
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 9), ref Unsafe.AddByteOffset(ref encoded, 24));
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 12), ref Unsafe.AddByteOffset(ref encoded, 32));
        Encode8(map, ref Unsafe.AddByteOffset(ref src, 15), ref Unsafe.AddByteOffset(ref encoded, 40));
    }

    #endregion Encode128

    #region Decode128

    public static bool TryDecode128(sbyte[] map, ref byte encoded, ref byte src) =>
        TryDecode24(map, ref encoded, ref src) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3)) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 6)) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 12), ref Unsafe.AddByteOffset(ref src, 9)) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 12)) &&
        TryDecode8(map, ref Unsafe.AddByteOffset(ref encoded, 20), ref Unsafe.AddByteOffset(ref src, 15));

    public static bool TryDecode128(sbyte[] map, ref char encoded, ref byte src) =>
        TryDecode24(map, ref encoded, ref src) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3)) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 6)) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 24), ref Unsafe.AddByteOffset(ref src, 9)) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 32), ref Unsafe.AddByteOffset(ref src, 12)) &&
        TryDecode8(map, ref Unsafe.AddByteOffset(ref encoded, 40), ref Unsafe.AddByteOffset(ref src, 15));

    public static bool TryDecode128(sbyte[] map, ref byte encoded, ref byte src, out byte invalid) =>
        TryDecode24(map, ref encoded, ref src, out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3), out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 6), out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 12), ref Unsafe.AddByteOffset(ref src, 9), out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 12), out invalid) &&
        TryDecode8(map, ref Unsafe.AddByteOffset(ref encoded, 20), ref Unsafe.AddByteOffset(ref src, 15), out invalid);

    public static bool TryDecode128(sbyte[] map, ref char encoded, ref byte src, out char invalid) =>
        TryDecode24(map, ref encoded, ref src, out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3), out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 6), out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 24), ref Unsafe.AddByteOffset(ref src, 9), out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 32), ref Unsafe.AddByteOffset(ref src, 12), out invalid) &&
        TryDecode8(map, ref Unsafe.AddByteOffset(ref encoded, 40), ref Unsafe.AddByteOffset(ref src, 15), out invalid);

    #endregion Decode128

    #region IsValid128

    public static bool IsValid128(sbyte[] map, ref byte encoded) =>
        IsValid24(map, ref encoded) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 4)) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8)) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 12)) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 16)) &&
        IsValid8(map, ref Unsafe.AddByteOffset(ref encoded, 20));

    public static bool IsValid128(sbyte[] map, ref char encoded) =>
        IsValid24(map, ref encoded) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8)) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 16)) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 24)) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 32)) &&
        IsValid8(map, ref Unsafe.AddByteOffset(ref encoded, 40));

    public static bool IsValid128(sbyte[] map, ref byte encoded, out byte invalid) =>
        IsValid24(map, ref encoded, out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 4), out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8), out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 12), out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 16), out invalid) &&
        IsValid8(map, ref Unsafe.AddByteOffset(ref encoded, 20), out invalid);

    public static bool IsValid128(sbyte[] map, ref char encoded, out char invalid) =>
        IsValid24(map, ref encoded, out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8), out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 16), out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 24), out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 32), out invalid) &&
        IsValid8(map, ref Unsafe.AddByteOffset(ref encoded, 40), out invalid);

    #endregion IsValid128

    #region Encode120

    public static void Encode120(byte[] map, ref byte src, ref byte encoded)
    {
        Encode24(map, ref src, ref encoded);
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 4));
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 6), ref Unsafe.AddByteOffset(ref encoded, 8));
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 9), ref Unsafe.AddByteOffset(ref encoded, 12));
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 12), ref Unsafe.AddByteOffset(ref encoded, 16));
    }

    public static void Encode120(char[] map, ref byte src, ref char encoded)
    {
        Encode24(map, ref src, ref encoded);
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 8));
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 6), ref Unsafe.AddByteOffset(ref encoded, 16));
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 9), ref Unsafe.AddByteOffset(ref encoded, 24));
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 12), ref Unsafe.AddByteOffset(ref encoded, 32));
    }

    #endregion Encode120

    #region Decode120

    public static bool TryDecode120(sbyte[] map, ref byte encoded, ref byte src) =>
        TryDecode24(map, ref encoded, ref src) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3)) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 6)) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 12), ref Unsafe.AddByteOffset(ref src, 9)) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 12));

    public static bool TryDecode120(sbyte[] map, ref char encoded, ref byte src) =>
        TryDecode24(map, ref encoded, ref src) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3)) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 6)) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 24), ref Unsafe.AddByteOffset(ref src, 9)) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 32), ref Unsafe.AddByteOffset(ref src, 12));

    public static bool TryDecode120(sbyte[] map, ref byte encoded, ref byte src, out byte invalid) =>
        TryDecode24(map, ref encoded, ref src, out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3), out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 6), out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 12), ref Unsafe.AddByteOffset(ref src, 9), out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 12), out invalid);

    public static bool TryDecode120(sbyte[] map, ref char encoded, ref byte src, out char invalid) =>
        TryDecode24(map, ref encoded, ref src, out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3), out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 6), out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 24), ref Unsafe.AddByteOffset(ref src, 9), out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 32), ref Unsafe.AddByteOffset(ref src, 12), out invalid);

    #endregion Decode120

    #region IsValid120

    public static bool IsValid120(sbyte[] map, ref byte encoded) =>
        IsValid24(map, ref encoded) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 4)) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8)) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 12)) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 16));

    public static bool IsValid120(sbyte[] map, ref char encoded) =>
        IsValid24(map, ref encoded) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8)) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 16)) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 24)) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 32));

    public static bool IsValid120(sbyte[] map, ref byte encoded, out byte invalid) =>
        IsValid24(map, ref encoded, out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 4), out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8), out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 12), out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 16), out invalid);

    public static bool IsValid120(sbyte[] map, ref char encoded, out char invalid) =>
        IsValid24(map, ref encoded, out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8), out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 16), out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 24), out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 32), out invalid);

    #endregion IsValid120

    #region Encode96

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int128 Encode96ToInt128(byte[] map, ref byte src)
        => throw new NotImplementedException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Guid Encode96ToGuid(byte[] map, ref byte src)
    {
        throw new NotImplementedException();
    }

    public static void Encode96(byte[] map, ref byte src, ref byte encoded)
    {
        Encode24(map, ref src, ref encoded);
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 4));
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 6), ref Unsafe.AddByteOffset(ref encoded, 8));
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 9), ref Unsafe.AddByteOffset(ref encoded, 12));
    }

    public static void Encode96(char[] map, ref byte src, ref char encoded)
    {
        Encode24(map, ref src, ref encoded);
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 8));
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 6), ref Unsafe.AddByteOffset(ref encoded, 16));
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 9), ref Unsafe.AddByteOffset(ref encoded, 24)); ;
    }

    #endregion Encode96

    #region Decode96

    public static bool TryDecode96(sbyte[] map, ref byte encoded, ref byte src) =>
        TryDecode24(map, ref encoded, ref src) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3)) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 6)) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 12), ref Unsafe.AddByteOffset(ref src, 9));

    public static bool TryDecode96(sbyte[] map, ref char encoded, ref byte src) =>
        TryDecode24(map, ref encoded, ref src) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3)) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 6)) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 24), ref Unsafe.AddByteOffset(ref src, 9));

    public static bool TryDecode96(sbyte[] map, ref byte encoded, ref byte src, out byte invalid) =>
        TryDecode24(map, ref encoded, ref src, out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3), out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 6), out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 12), ref Unsafe.AddByteOffset(ref src, 9), out invalid);

    public static bool TryDecode96(sbyte[] map, ref char encoded, ref byte src, out char invalid) =>
        TryDecode24(map, ref encoded, ref src, out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3), out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 6), out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 24), ref Unsafe.AddByteOffset(ref src, 9), out invalid);

    #endregion Decode96

    #region IsValid96

    public static bool IsValid96(sbyte[] map, ref byte encoded) =>
        IsValid24(map, ref encoded) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 4)) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8)) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 12));

    public static bool IsValid96(sbyte[] map, ref char encoded) =>
        IsValid24(map, ref encoded) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8)) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 16)) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 24));

    public static bool IsValid96(sbyte[] map, ref byte encoded, out byte invalid) =>
        IsValid24(map, ref encoded, out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 4), out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8), out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 12), out invalid);

    public static bool IsValid96(sbyte[] map, ref char encoded, out char invalid) =>
        IsValid24(map, ref encoded, out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8), out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 16), out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 24), out invalid);

    #endregion IsValid96

    #region Encode72

    public static void Encode72(byte[] map, ref byte src, ref byte encoded)
    {
        Encode24(map, ref src, ref encoded);
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 4));
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 6), ref Unsafe.AddByteOffset(ref encoded, 8));
    }

    public static void Encode72(char[] map, ref byte src, ref char encoded)
    {
        Encode24(map, ref src, ref encoded);
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 8));
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 6), ref Unsafe.AddByteOffset(ref encoded, 16));
    }

    #endregion Encode72

    #region Decode72

    public static bool TryDecode72(sbyte[] map, ref byte encoded, ref byte src) =>
        TryDecode24(map, ref encoded, ref src) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3)) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 6));

    public static bool TryDecode72(sbyte[] map, ref char encoded, ref byte src) =>
        TryDecode24(map, ref encoded, ref src) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3)) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 6));

    public static bool TryDecode72(sbyte[] map, ref byte encoded, ref byte src, out byte invalid) =>
        TryDecode24(map, ref encoded, ref src, out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3), out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 6), out invalid);

    public static bool TryDecode72(sbyte[] map, ref char encoded, ref byte src, out char invalid) =>
        TryDecode24(map, ref encoded, ref src, out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3), out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 6), out invalid);

    #endregion Decode72

    #region IsValid72

    public static bool IsValid72(sbyte[] map, ref byte encoded) =>
        IsValid24(map, ref encoded) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 4)) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8));

    public static bool IsValid72(sbyte[] map, ref char encoded) =>
        IsValid24(map, ref encoded) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8)) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 16));

    public static bool IsValid72(sbyte[] map, ref byte encoded, out byte invalid) =>
        IsValid24(map, ref encoded, out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 4), out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8), out invalid);

    public static bool IsValid72(sbyte[] map, ref char encoded, out char invalid) =>
        IsValid24(map, ref encoded, out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8), out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 16), out invalid);

    #endregion IsValid72

    #region Encode64

    public static void Encode64(byte[] map, ref byte src, ref byte encoded)
    {
        Encode24(map, ref src, ref encoded);
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 4));
        Encode16(map, ref Unsafe.AddByteOffset(ref src, 6), ref Unsafe.AddByteOffset(ref encoded, 8));
    }

    public static void Encode64(char[] map, ref byte src, ref char encoded)
    {
        Encode24(map, ref src, ref encoded);
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 8));
        Encode16(map, ref Unsafe.AddByteOffset(ref src, 6), ref Unsafe.AddByteOffset(ref encoded, 16));
    }

    #endregion Encode64

    #region Decode64

    public static bool TryDecode64(sbyte[] map, ref byte encoded, ref byte src) =>
        TryDecode24(map, ref encoded, ref src) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3)) &&
        TryDecode16(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 6));

    public static bool TryDecode64(sbyte[] map, ref char encoded, ref byte src) =>
        TryDecode24(map, ref encoded, ref src) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3)) &&
        TryDecode16(map, ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 6));

    public static bool TryDecode64(sbyte[] map, ref byte encoded, ref byte src, out byte invalid) =>
        TryDecode24(map, ref encoded, ref src, out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3), out invalid) &&
        TryDecode16(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 6), out invalid);

    public static bool TryDecode64(sbyte[] map, ref char encoded, ref byte src, out char invalid) =>
        TryDecode24(map, ref encoded, ref src, out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3), out invalid) &&
        TryDecode16(map, ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 6), out invalid);

    #endregion Decode64

    #region IsValid64

    public static bool IsValid64(sbyte[] map, ref byte encoded) =>
        IsValid24(map, ref encoded) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 4)) &&
        IsValid16(map, ref Unsafe.AddByteOffset(ref encoded, 8));

    public static bool IsValid64(sbyte[] map, ref char encoded) =>
        IsValid24(map, ref encoded) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8)) &&
        IsValid16(map, ref Unsafe.AddByteOffset(ref encoded, 16));

    public static bool IsValid64(sbyte[] map, ref byte encoded, out byte invalid) =>
        IsValid24(map, ref encoded, out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 4), out invalid) &&
        IsValid16(map, ref Unsafe.AddByteOffset(ref encoded, 8), out invalid);

    public static bool IsValid64(sbyte[] map, ref char encoded, out char invalid) =>
        IsValid24(map, ref encoded, out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8), out invalid) &&
        IsValid16(map, ref Unsafe.AddByteOffset(ref encoded, 16), out invalid);

    #endregion IsValid64

    #region Encode48

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Encode48ToInt64(byte[] map, ref byte src)
    {
        throw new NotImplementedException();
    }

    public static void Encode48(byte[] map, ref byte src, ref byte encoded)
    {
        Encode24(map, ref src, ref encoded);
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 4));
    }

    public static void Encode48(char[] map, ref byte src, ref char encoded)
    {
        Encode24(map, ref src, ref encoded);
        Encode24(map, ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 8));
    }

    #endregion Encode48

    #region Decode48

    public static bool TryDecode48(sbyte[] map, ref byte encoded, ref byte src) =>
        TryDecode24(map, ref encoded, ref src) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3));

    public static bool TryDecode48(sbyte[] map, ref char encoded, ref byte src) =>
        TryDecode24(map, ref encoded, ref src) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3));

    public static bool TryDecode48(sbyte[] map, ref byte encoded, ref byte src, out byte invalid) =>
        TryDecode24(map, ref encoded, ref src, out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3), out invalid);

    public static bool TryDecode48(sbyte[] map, ref char encoded, ref byte src, out char invalid) =>
        TryDecode24(map, ref encoded, ref src, out invalid) &&
        TryDecode24(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3), out invalid);

    #endregion Decode48

    #region IsValid48

    public static bool IsValid48(sbyte[] map, ref byte encoded) =>
        IsValid24(map, ref encoded) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 4));

    public static bool IsValid48(sbyte[] map, ref char encoded) =>
        IsValid24(map, ref encoded) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8));

    public static bool IsValid48(sbyte[] map, ref byte encoded, out byte invalid) =>
        IsValid24(map, ref encoded, out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 4), out invalid);

    public static bool IsValid48(sbyte[] map, ref char encoded, out char invalid) =>
        IsValid24(map, ref encoded, out invalid) &&
        IsValid24(map, ref Unsafe.AddByteOffset(ref encoded, 8), out invalid);

    #endregion IsValid48

    #region Encode32

    public static void Encode32(byte[] map, ref byte src, ref byte encoded)
    {
        Encode24(map, ref src, ref encoded);
        Encode8(map, ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 4));
    }

    public static void Encode32(char[] map, ref byte src, ref char encoded)
    {
        Encode24(map, ref src, ref encoded);
        Encode8(map, ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 8));
    }

    #endregion Encode32

    #region Decode32

    public static bool TryDecode32(sbyte[] map, ref byte encoded, ref byte src) =>
        TryDecode24(map, ref encoded, ref src) &&
        TryDecode8(map, ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3));

    public static bool TryDecode32(sbyte[] map, ref char encoded, ref byte src) =>
        TryDecode24(map, ref encoded, ref src) &&
        TryDecode8(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3));

    public static bool TryDecode32(sbyte[] map, ref byte encoded, ref byte src, out byte invalid) =>
        TryDecode24(map, ref encoded, ref src, out invalid) &&
        TryDecode8(map, ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3), out invalid);

    public static bool TryDecode32(sbyte[] map, ref char encoded, ref byte src, out char invalid) =>
        TryDecode24(map, ref encoded, ref src, out invalid) &&
        TryDecode8(map, ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3), out invalid);

    #endregion Decode32

    #region IsValid32

    public static bool IsValid32(sbyte[] map, ref byte encoded) =>
        IsValid24(map, ref encoded) &&
        IsValid8(map, ref Unsafe.AddByteOffset(ref encoded, 4));

    public static bool IsValid32(sbyte[] map, ref char encoded) =>
        IsValid24(map, ref encoded) &&
        IsValid8(map, ref Unsafe.AddByteOffset(ref encoded, 8));

    public static bool IsValid32(sbyte[] map, ref byte encoded, out byte invalid) =>
        IsValid24(map, ref encoded, out invalid) &&
        IsValid8(map, ref Unsafe.AddByteOffset(ref encoded, 4), out invalid);

    public static bool IsValid32(sbyte[] map, ref char encoded, out char invalid) =>
        IsValid24(map, ref encoded, out invalid) &&
        IsValid8(map, ref Unsafe.AddByteOffset(ref encoded, 8), out invalid);

    #endregion IsValid32

    #region Encode24

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Encode24ToInt32(byte[] map, ref byte src)
    {
        int i = src << 16 | Unsafe.AddByteOffset(ref src, 1) << 8 | Unsafe.AddByteOffset(ref src, 2);
        return map[i >> 18] | map[i >> 12 & 0x3F] << 8 | map[i >> 6 & 0x3F] << 16 | map[i & 0x3F] << 24;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode24(byte[] map, ref byte src, ref byte encoded)
    {
        int i = src << 16 | Unsafe.AddByteOffset(ref src, 1) << 8 | Unsafe.AddByteOffset(ref src, 2);
        encoded = map[i >> 18];
        Unsafe.AddByteOffset(ref encoded, 1) = map[i >> 12 & 0x3F];
        Unsafe.AddByteOffset(ref encoded, 2) = map[i >> 6 & 0x3F];
        Unsafe.AddByteOffset(ref encoded, 3) = map[i & 0x3F];

        //slowly
        //Unsafe.As<byte, int>(ref encoded) = map[i >> 18] | map[i >> 12 & 0x3F] << 8 | map[i >> 6 & 0x3F] << 16 | map[i & 0x3F] << 24;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode24(char[] map, ref byte src, ref char encoded)
    {
        int i = src << 16 | Unsafe.AddByteOffset(ref src, 1) << 8 | Unsafe.AddByteOffset(ref src, 2);
        encoded = map[i >> 18];
        Unsafe.AddByteOffset(ref encoded, 2) = map[i >> 12 & 0x3F];
        Unsafe.AddByteOffset(ref encoded, 4) = map[i >> 6 & 0x3F];
        Unsafe.AddByteOffset(ref encoded, 6) = map[i & 0x3F];
    }

    #endregion Encode24

    #region Decode24

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDecode24(sbyte[] map, ref byte encoded, ref byte src)
    {
        var val = map[encoded] << 18 | map[Unsafe.AddByteOffset(ref encoded, 1)] << 12 |
                  map[Unsafe.AddByteOffset(ref encoded, 2)] << 6 | (int)map[Unsafe.AddByteOffset(ref encoded, 3)];
        if (val < 0) return false;

        src = (byte)(val >> 16);
        Unsafe.AddByteOffset(ref src, 1) = (byte)(val >> 8);
        Unsafe.AddByteOffset(ref src, 2) = (byte)val;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDecode24(sbyte[] map, ref char encoded, ref byte src)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 2);
        var i2 = Unsafe.AddByteOffset(ref encoded, 4);
        var i3 = Unsafe.AddByteOffset(ref encoded, 6);
        if (((encoded | i1 | i2 | i3) & 0xffffff00) != 0) return false;

        var val = map[encoded] << 18 | map[i1] << 12 | map[i2] << 6 | (int)map[i3];
        if (val < 0) return false;

        src = (byte)(val >> 16);
        Unsafe.AddByteOffset(ref src, 1) = (byte)(val >> 8);
        Unsafe.AddByteOffset(ref src, 2) = (byte)val;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDecode24(sbyte[] map, ref byte encoded, ref byte src, out byte invalid)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 1);
        var i2 = Unsafe.AddByteOffset(ref encoded, 2);
        var i3 = Unsafe.AddByteOffset(ref encoded, 3);
        var val = map[encoded] << 18 | map[i1] << 12 | map[i2] << 6 | (int)map[i3];
        if (val < 0)
        {
            invalid = map[encoded] < 0 ? encoded : map[i1] < 0 ? i1 : map[i2] < 0 ? i2 : i3;
            return false;
        }

        src = (byte)(val >> 16);
        Unsafe.AddByteOffset(ref src, 1) = (byte)(val >> 8);
        Unsafe.AddByteOffset(ref src, 2) = (byte)val;
        invalid = default;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDecode24(sbyte[] map, ref char encoded, ref byte src, out char invalid)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 2);
        var i2 = Unsafe.AddByteOffset(ref encoded, 4);
        var i3 = Unsafe.AddByteOffset(ref encoded, 6);
        if (((encoded | i1 | i2 | i3) & 0xffffff00) != 0)
        {
            invalid = encoded > 255 ? encoded : i1 > 255 ? i1 : i2 > 255 ? i2 : i3;
            return false;
        }

        var val = map[encoded] << 18 | map[i1] << 12 | map[i2] << 6 | (int)map[i3];
        if (val < 0)
        {
            invalid = map[encoded] < 0 ? encoded : map[i1] < 0 ? i1 : map[i2] < 0 ? i2 : i3;
            return false;
        }

        src = (byte)(val >> 16);
        Unsafe.AddByteOffset(ref src, 1) = (byte)(val >> 8);
        Unsafe.AddByteOffset(ref src, 2) = (byte)val;
        invalid = default;
        return true;
    }

    #endregion Decode24

    #region IsValid24

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid24(sbyte[] map, ref byte encoded)
        => (map[encoded] << 18 | map[Unsafe.AddByteOffset(ref encoded, 1)] << 12 |
            map[Unsafe.AddByteOffset(ref encoded, 2)] << 6 | (int)map[Unsafe.AddByteOffset(ref encoded, 3)]) >= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid24(sbyte[] map, ref char encoded)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 2);
        var i2 = Unsafe.AddByteOffset(ref encoded, 4);
        var i3 = Unsafe.AddByteOffset(ref encoded, 6);
        if (((encoded | i1 | i2 | i3) & 0xffffff00) != 0) return false;
        return (map[encoded] << 18 | map[i1] << 12 | map[i2] << 6 | (int)map[i3]) >= 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid24(sbyte[] map, ref byte encoded, out byte invalid)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 1);
        var i2 = Unsafe.AddByteOffset(ref encoded, 2);
        var i3 = Unsafe.AddByteOffset(ref encoded, 3);
        if ((map[encoded] << 18 | map[i1] << 12 | map[i2] << 6 | (int)map[i3]) < 0)
        {
            invalid = map[encoded] < 0 ? encoded : map[i1] < 0 ? i1 : map[i2] < 0 ? i2 : i3;
            return false;
        }
        invalid = default;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid24(sbyte[] map, ref char encoded, out char invalid)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 2);
        var i2 = Unsafe.AddByteOffset(ref encoded, 4);
        var i3 = Unsafe.AddByteOffset(ref encoded, 6);
        if (((encoded | i1 | i2 | i3) & 0xffffff00) != 0)
        {
            invalid = encoded > 255 ? encoded : i1 > 255 ? i1 : i2 > 255 ? i2 : i3;
            return false;
        }
        if ((map[encoded] << 18 | map[i1] << 12 | map[i2] << 6 | (int)map[i3]) < 0)
        {
            invalid = map[encoded] < 0 ? encoded : map[i1] < 0 ? i1 : map[i2] < 0 ? i2 : i3;
            return false;
        }
        invalid = default;
        return true;
    }

    #endregion IsValid24

    #region Encode16

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode16(byte[] map, ref byte src, ref byte encoded)
    {
        int i = src << 16 | Unsafe.AddByteOffset(ref src, 1) << 8;
        encoded = map[i >> 18];
        Unsafe.AddByteOffset(ref encoded, 1) = map[i >> 12 & 0x3F];
        Unsafe.AddByteOffset(ref encoded, 2) = map[i >> 6 & 0x3F];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode16(char[] map, ref byte src, ref char encoded)
    {
        int i = src << 16 | Unsafe.AddByteOffset(ref src, 1) << 8;
        encoded = map[i >> 18];
        Unsafe.AddByteOffset(ref encoded, 2) = map[i >> 12 & 0x3F];
        Unsafe.AddByteOffset(ref encoded, 4) = map[i >> 6 & 0x3F];
    }

    #endregion Encode16

    #region Decode16

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDecode16(sbyte[] map, ref byte encoded, ref byte src)
    {
        var val = map[encoded] << 18 | map[Unsafe.AddByteOffset(ref encoded, 1)] << 12 |
                  map[Unsafe.AddByteOffset(ref encoded, 2)] << 6;
        if (val < 0) return false;

        src = (byte)(val >> 16);
        Unsafe.AddByteOffset(ref src, 1) = (byte)(val >> 8);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDecode16(sbyte[] map, ref char encoded, ref byte src)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 2);
        var i2 = Unsafe.AddByteOffset(ref encoded, 4);
        if (((encoded | i1 | i2) & 0xffffff00) != 0) return false;

        var val = map[encoded] << 18 | map[i1] << 12 | map[i2] << 6;
        if (val < 0) return false;

        src = (byte)(val >> 16);
        Unsafe.AddByteOffset(ref src, 1) = (byte)(val >> 8);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDecode16(sbyte[] map, ref byte encoded, ref byte src, out byte invalid)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 1);
        var i2 = Unsafe.AddByteOffset(ref encoded, 2);
        var val = map[encoded] << 18 | map[i1] << 12 | map[i2] << 6;
        if (val < 0)
        {
            invalid = map[encoded] < 0 ? encoded : map[i1] < 0 ? i1 : i2;
            return false;
        }

        src = (byte)(val >> 16);
        Unsafe.AddByteOffset(ref src, 1) = (byte)(val >> 8);
        invalid = default;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDecode16(sbyte[] map, ref char encoded, ref byte src, out char invalid)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 2);
        var i2 = Unsafe.AddByteOffset(ref encoded, 4);
        if (((encoded | i1 | i2) & 0xffffff00) != 0)
        {
            invalid = encoded > 255 ? encoded : i1 > 255 ? i1 : i2;
            return false;
        }

        var val = map[encoded] << 18 | map[i1] << 12 | map[i2] << 6;
        if (val < 0)
        {
            invalid = map[encoded] < 0 ? encoded : map[i1] < 0 ? i1 : i2;
            return false;
        }

        src = (byte)(val >> 16);
        Unsafe.AddByteOffset(ref src, 1) = (byte)(val >> 8);
        invalid = default;
        return true;
    }

    #endregion Decode16

    #region IsValid16

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid16(sbyte[] map, ref byte encoded)
        => (map[encoded] << 18 | map[Unsafe.AddByteOffset(ref encoded, 1)] << 12 |
            map[Unsafe.AddByteOffset(ref encoded, 2)] << 6) >= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid16(sbyte[] map, ref char encoded)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 2);
        var i2 = Unsafe.AddByteOffset(ref encoded, 4);
        if (((encoded | i1 | i2) & 0xffffff00) != 0) return false;

        return (map[encoded] << 18 | map[i1] << 12 | map[i2] << 6) >= 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid16(sbyte[] map, ref byte encoded, out byte invalid)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 1);
        var i2 = Unsafe.AddByteOffset(ref encoded, 2);
        if ((map[encoded] << 18 | map[i1] << 12 | map[i2] << 6) < 0)
        {
            invalid = map[encoded] < 0 ? encoded : map[i1] < 0 ? i1 : i2;
            return false;
        }
        invalid = default;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid16(sbyte[] map, ref char encoded, out char invalid)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 2);
        var i2 = Unsafe.AddByteOffset(ref encoded, 4);
        if (((encoded | i1 | i2) & 0xffffff00) != 0)
        {
            invalid = encoded > 255 ? encoded : i1 > 255 ? i1 : i2;
            return false;
        }
        if ((map[encoded] << 18 | map[i1] << 12 | map[i2] << 6) < 0)
        {
            invalid = map[encoded] < 0 ? encoded : map[i1] < 0 ? i1 : i2;
            return false;
        }
        invalid = default;
        return true;
    }

    #endregion IsValid16

    #region Encode8

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short Encode8ToInt16(byte[] map, ref byte src)
    {
        int i = src << 8;
        return (short)(map[i >> 10] | map[i >> 4 & 0x3F] << 8);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode8(byte[] map, ref byte src, ref byte encoded)
    {
        int i = src << 8;
        encoded = map[i >> 10];
        Unsafe.AddByteOffset(ref encoded, 1) = map[i >> 4 & 0x3F];

        //slowly
        //encoded = map[(src & 0xfc) >> 2];
        //Unsafe.AddByteOffset(ref encoded, 1) = map[(src & 0x03) << 4];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode8(char[] map, ref byte src, ref char encoded)
    {
        int i = src << 8;
        encoded = map[i >> 10];
        Unsafe.AddByteOffset(ref encoded, 2) = map[i >> 4 & 0x3F];

        //encoded = map[(src & 0xfc) >> 2];
        //Unsafe.AddByteOffset(ref encoded, 2) = map[(src & 0x03) << 4];
    }

    #endregion Encode8

    #region Decode8

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDecode8(sbyte[] map, ref byte encoded, ref byte src)
    {
        var val = map[encoded] << 18 | map[Unsafe.AddByteOffset(ref encoded, 1)] << 12;
        if (val < 0) return false;

        src = (byte)(val >> 16);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDecode8(sbyte[] map, ref char encoded, ref byte src)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 2);
        if (((encoded | i1) & 0xffffff00) != 0) return false;

        //TODO: Testing ???
        //if ((encoded | i1) > 255) return false;

        var val = map[encoded] << 18 | map[i1] << 12;
        if (val < 0) return false;

        src = (byte)(val >> 16);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDecode8(sbyte[] map, ref byte encoded, ref byte src, out byte invalid)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 1);
        var val = map[encoded] << 18 | map[i1] << 12;
        if (val < 0)
        {
            invalid = map[encoded] < 0 ? encoded : i1;
            return false;
        }

        src = (byte)(val >> 16);
        invalid = default;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDecode8(sbyte[] map, ref char encoded, ref byte src, out char invalid)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 2);
        if (((encoded | i1) & 0xffffff00) != 0)
        {
            invalid = encoded > 255 ? encoded : i1;
            return false;
        }

        var val = map[encoded] << 18 | map[i1] << 12;
        if (val < 0)
        {
            invalid = map[encoded] < 0 ? encoded : i1;
            return false;
        }

        src = (byte)(val >> 16);
        invalid = default;
        return true;
    }

    #endregion Decode8

    #region IsValid8

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid8(sbyte[] map, ref byte encoded)
        => (map[encoded] << 18 | map[Unsafe.AddByteOffset(ref encoded, 1)] << 12) >= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid8(sbyte[] map, ref char encoded)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 2);
        if (((encoded | i1) & 0xffffff00) != 0) return false;

        //TODO: Testing ???
        //if ((encoded | i1) > 255) return false;

        return (map[encoded] << 18 | map[i1] << 12) >= 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid8(sbyte[] map, ref byte encoded, out byte invalid)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 1);
        if ((map[encoded] << 18 | map[i1] << 12) < 0)
        {
            invalid = map[encoded] < 0 ? encoded : i1;
            return false;
        }
        invalid = default;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid8(sbyte[] map, ref char encoded, out char invalid)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 2);
        if (((encoded | i1) & 0xffffff00) != 0)
        {
            invalid = encoded > 255 ? encoded : i1;
            return false;
        }
        if ((map[encoded] << 18 | map[i1] << 12) < 0)
        {
            invalid = map[encoded] < 0 ? encoded : i1;
            return false;
        }
        invalid = default;
        return true;
    }

    #endregion IsValid8

    public static void ToChar176(ref byte by, ref char ch)
    {
        ch = (char)by;
        Unsafe.AddByteOffset(ref ch, 2) = (char)Unsafe.AddByteOffset(ref by, 1);
        Unsafe.AddByteOffset(ref ch, 4) = (char)Unsafe.AddByteOffset(ref by, 2);
        Unsafe.AddByteOffset(ref ch, 6) = (char)Unsafe.AddByteOffset(ref by, 3);
        Unsafe.AddByteOffset(ref ch, 8) = (char)Unsafe.AddByteOffset(ref by, 4);
        Unsafe.AddByteOffset(ref ch, 10) = (char)Unsafe.AddByteOffset(ref by, 5);
        Unsafe.AddByteOffset(ref ch, 12) = (char)Unsafe.AddByteOffset(ref by, 6);
        Unsafe.AddByteOffset(ref ch, 14) = (char)Unsafe.AddByteOffset(ref by, 7);
        Unsafe.AddByteOffset(ref ch, 16) = (char)Unsafe.AddByteOffset(ref by, 8);
        Unsafe.AddByteOffset(ref ch, 18) = (char)Unsafe.AddByteOffset(ref by, 9);
        Unsafe.AddByteOffset(ref ch, 20) = (char)Unsafe.AddByteOffset(ref by, 10);
        Unsafe.AddByteOffset(ref ch, 22) = (char)Unsafe.AddByteOffset(ref by, 11);
        Unsafe.AddByteOffset(ref ch, 24) = (char)Unsafe.AddByteOffset(ref by, 12);
        Unsafe.AddByteOffset(ref ch, 26) = (char)Unsafe.AddByteOffset(ref by, 13);
        Unsafe.AddByteOffset(ref ch, 28) = (char)Unsafe.AddByteOffset(ref by, 14);
        Unsafe.AddByteOffset(ref ch, 30) = (char)Unsafe.AddByteOffset(ref by, 15);
        Unsafe.AddByteOffset(ref ch, 32) = (char)Unsafe.AddByteOffset(ref by, 16);
        Unsafe.AddByteOffset(ref ch, 34) = (char)Unsafe.AddByteOffset(ref by, 17);
        Unsafe.AddByteOffset(ref ch, 36) = (char)Unsafe.AddByteOffset(ref by, 18);
        Unsafe.AddByteOffset(ref ch, 38) = (char)Unsafe.AddByteOffset(ref by, 19);
        Unsafe.AddByteOffset(ref ch, 40) = (char)Unsafe.AddByteOffset(ref by, 20);
        Unsafe.AddByteOffset(ref ch, 42) = (char)Unsafe.AddByteOffset(ref by, 21);
    }
}