using System.Runtime.CompilerServices;

namespace IT.Encoding.Base64;

public static class UnsafeBase64
{
    #region Encode128

    public static void Encode128(ref byte src, ref byte encoded, byte[] map)
    {
        Encode24(ref src, ref encoded, map);
        Encode24(ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 4), map);
        Encode24(ref Unsafe.AddByteOffset(ref src, 6), ref Unsafe.AddByteOffset(ref encoded, 8), map);
        Encode24(ref Unsafe.AddByteOffset(ref src, 9), ref Unsafe.AddByteOffset(ref encoded, 12), map);
        Encode24(ref Unsafe.AddByteOffset(ref src, 12), ref Unsafe.AddByteOffset(ref encoded, 16), map);
        Encode8(ref Unsafe.AddByteOffset(ref src, 15), ref Unsafe.AddByteOffset(ref encoded, 20), map);
    }

    public static void Encode128(ref byte src, ref char encoded, char[] map)
    {
        Encode24(ref src, ref encoded, map);
        Encode24(ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 8), map);
        Encode24(ref Unsafe.AddByteOffset(ref src, 6), ref Unsafe.AddByteOffset(ref encoded, 16), map);
        Encode24(ref Unsafe.AddByteOffset(ref src, 9), ref Unsafe.AddByteOffset(ref encoded, 24), map);
        Encode24(ref Unsafe.AddByteOffset(ref src, 12), ref Unsafe.AddByteOffset(ref encoded, 32), map);
        Encode8(ref Unsafe.AddByteOffset(ref src, 15), ref Unsafe.AddByteOffset(ref encoded, 40), map);
    }

    #endregion Encode128

    #region Decode128

    public static bool TryDecode128(ref byte encoded, ref byte src, sbyte[] map) =>
        TryDecode24(ref encoded, ref src, map) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3), map) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 6), map) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 12), ref Unsafe.AddByteOffset(ref src, 9), map) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 12), map) &&
        TryDecode8(ref Unsafe.AddByteOffset(ref encoded, 20), ref Unsafe.AddByteOffset(ref src, 15), map);

    public static bool TryDecode128(ref char encoded, ref byte src, sbyte[] map) =>
        TryDecode24(ref encoded, ref src, map) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3), map) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 6), map) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 24), ref Unsafe.AddByteOffset(ref src, 9), map) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 32), ref Unsafe.AddByteOffset(ref src, 12), map) &&
        TryDecode8(ref Unsafe.AddByteOffset(ref encoded, 40), ref Unsafe.AddByteOffset(ref src, 15), map);

    public static bool TryDecode128(ref byte encoded, ref byte src, sbyte[] map, out byte invalid) =>
        TryDecode24(ref encoded, ref src, map, out invalid) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3), map, out invalid) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 6), map, out invalid) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 12), ref Unsafe.AddByteOffset(ref src, 9), map, out invalid) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 12), map, out invalid) &&
        TryDecode8(ref Unsafe.AddByteOffset(ref encoded, 20), ref Unsafe.AddByteOffset(ref src, 15), map, out invalid);

    public static bool TryDecode128(ref char encoded, ref byte src, sbyte[] map, out char invalid) =>
        TryDecode24(ref encoded, ref src, map, out invalid) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3), map, out invalid) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 6), map, out invalid) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 24), ref Unsafe.AddByteOffset(ref src, 9), map, out invalid) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 32), ref Unsafe.AddByteOffset(ref src, 12), map, out invalid) &&
        TryDecode8(ref Unsafe.AddByteOffset(ref encoded, 40), ref Unsafe.AddByteOffset(ref src, 15), map, out invalid);

    #endregion Decode128

    #region IsValid128

    public static bool IsValid128(ref byte encoded, sbyte[] map) =>
        IsValid24(ref encoded, map) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 4), map) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 8), map) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 12), map) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 16), map) &&
        IsValid8(ref Unsafe.AddByteOffset(ref encoded, 20), map);

    public static bool IsValid128(ref char encoded, sbyte[] map) =>
        IsValid24(ref encoded, map) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 8), map) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 16), map) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 24), map) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 32), map) &&
        IsValid8(ref Unsafe.AddByteOffset(ref encoded, 40), map);

    public static bool IsValid128(ref byte encoded, sbyte[] map, out byte invalid) =>
        IsValid24(ref encoded, map, out invalid) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 4), map, out invalid) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 8), map, out invalid) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 12), map, out invalid) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 16), map, out invalid) &&
        IsValid8(ref Unsafe.AddByteOffset(ref encoded, 20), map, out invalid);

    public static bool IsValid128(ref char encoded, sbyte[] map, out char invalid) =>
        IsValid24(ref encoded, map, out invalid) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 8), map, out invalid) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 16), map, out invalid) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 24), map, out invalid) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 32), map, out invalid) &&
        IsValid8(ref Unsafe.AddByteOffset(ref encoded, 40), map, out invalid);

    #endregion IsValid128

    #region Encode72

    public static void Encode72(ref byte src, ref byte encoded, byte[] map)
    {
        Encode24(ref src, ref encoded, map);
        Encode24(ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 4), map);
        Encode24(ref Unsafe.AddByteOffset(ref src, 6), ref Unsafe.AddByteOffset(ref encoded, 8), map);
    }

    public static void Encode72(ref byte src, ref char encoded, char[] map)
    {
        Encode24(ref src, ref encoded, map);
        Encode24(ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 8), map);
        Encode24(ref Unsafe.AddByteOffset(ref src, 6), ref Unsafe.AddByteOffset(ref encoded, 16), map);
    }

    #endregion Encode72

    #region Decode72

    public static bool TryDecode72(ref byte encoded, ref byte src, sbyte[] map) =>
        TryDecode24(ref encoded, ref src, map) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3), map) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 6), map);

    public static bool TryDecode72(ref char encoded, ref byte src, sbyte[] map) =>
        TryDecode24(ref encoded, ref src, map) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3), map) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 6), map);

    public static bool TryDecode72(ref byte encoded, ref byte src, sbyte[] map, out byte invalid) =>
        TryDecode24(ref encoded, ref src, map, out invalid) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3), map, out invalid) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 6), map, out invalid);

    public static bool TryDecode72(ref char encoded, ref byte src, sbyte[] map, out char invalid) =>
        TryDecode24(ref encoded, ref src, map, out invalid) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3), map, out invalid) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 6), map, out invalid);

    #endregion Decode72

    #region IsValid72

    public static bool IsValid72(ref byte encoded, sbyte[] map) =>
        IsValid24(ref encoded, map) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 4), map) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 8), map);

    public static bool IsValid72(ref char encoded, sbyte[] map) =>
        IsValid24(ref encoded, map) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 8), map) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 16), map);

    public static bool IsValid72(ref byte encoded, sbyte[] map, out byte invalid) =>
        IsValid24(ref encoded, map, out invalid) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 4), map, out invalid) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 8), map, out invalid);

    public static bool IsValid72(ref char encoded, sbyte[] map, out char invalid) =>
        IsValid24(ref encoded, map, out invalid) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 8), map, out invalid) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 16), map, out invalid);

    #endregion IsValid72

    #region Encode64

    public static void Encode64(ref byte src, ref byte encoded, byte[] map)
    {
        Encode24(ref src, ref encoded, map);
        Encode24(ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 4), map);
        Encode16(ref Unsafe.AddByteOffset(ref src, 6), ref Unsafe.AddByteOffset(ref encoded, 8), map);
    }

    public static void Encode64(ref byte src, ref char encoded, char[] map)
    {
        Encode24(ref src, ref encoded, map);
        Encode24(ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 8), map);
        Encode16(ref Unsafe.AddByteOffset(ref src, 6), ref Unsafe.AddByteOffset(ref encoded, 16), map);
    }

    #endregion Encode64

    #region Decode64

    public static bool TryDecode64(ref byte encoded, ref byte src, sbyte[] map) =>
        TryDecode24(ref encoded, ref src, map) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3), map) &&
        TryDecode16(ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 6), map);

    public static bool TryDecode64(ref char encoded, ref byte src, sbyte[] map) =>
        TryDecode24(ref encoded, ref src, map) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3), map) &&
        TryDecode16(ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 6), map);

    public static bool TryDecode64(ref byte encoded, ref byte src, sbyte[] map, out byte invalid) =>
        TryDecode24(ref encoded, ref src, map, out invalid) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3), map, out invalid) &&
        TryDecode16(ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 6), map, out invalid);

    public static bool TryDecode64(ref char encoded, ref byte src, sbyte[] map, out char invalid) =>
        TryDecode24(ref encoded, ref src, map, out invalid) &&
        TryDecode24(ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3), map, out invalid) &&
        TryDecode16(ref Unsafe.AddByteOffset(ref encoded, 16), ref Unsafe.AddByteOffset(ref src, 6), map, out invalid);

    #endregion Decode64

    #region IsValid64

    public static bool IsValid64(ref byte encoded, sbyte[] map) =>
        IsValid24(ref encoded, map) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 4), map) &&
        IsValid16(ref Unsafe.AddByteOffset(ref encoded, 8), map);

    public static bool IsValid64(ref char encoded, sbyte[] map) =>
        IsValid24(ref encoded, map) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 8), map) &&
        IsValid16(ref Unsafe.AddByteOffset(ref encoded, 16), map);

    public static bool IsValid64(ref byte encoded, sbyte[] map, out byte invalid) =>
        IsValid24(ref encoded, map, out invalid) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 4), map, out invalid) &&
        IsValid16(ref Unsafe.AddByteOffset(ref encoded, 8), map, out invalid);

    public static bool IsValid64(ref char encoded, sbyte[] map, out char invalid) =>
        IsValid24(ref encoded, map, out invalid) &&
        IsValid24(ref Unsafe.AddByteOffset(ref encoded, 8), map, out invalid) &&
        IsValid16(ref Unsafe.AddByteOffset(ref encoded, 16), map, out invalid);

    #endregion IsValid64

    #region Encode32

    public static void Encode32(ref byte src, ref byte encoded, byte[] map)
    {
        Encode24(ref src, ref encoded, map);
        Encode8(ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 4), map);
    }

    public static void Encode32(ref byte src, ref char encoded, char[] map)
    {
        Encode24(ref src, ref encoded, map);
        Encode8(ref Unsafe.AddByteOffset(ref src, 3), ref Unsafe.AddByteOffset(ref encoded, 8), map);
    }

    #endregion Encode32

    #region Decode32

    public static bool TryDecode32(ref byte encoded, ref byte src, sbyte[] map) =>
        TryDecode24(ref encoded, ref src, map) &&
        TryDecode8(ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3), map);

    public static bool TryDecode32(ref char encoded, ref byte src, sbyte[] map) =>
        TryDecode24(ref encoded, ref src, map) &&
        TryDecode8(ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3), map);

    public static bool TryDecode32(ref byte encoded, ref byte src, sbyte[] map, out byte invalid) =>
        TryDecode24(ref encoded, ref src, map, out invalid) &&
        TryDecode8(ref Unsafe.AddByteOffset(ref encoded, 4), ref Unsafe.AddByteOffset(ref src, 3), map, out invalid);

    public static bool TryDecode32(ref char encoded, ref byte src, sbyte[] map, out char invalid) =>
        TryDecode24(ref encoded, ref src, map, out invalid) &&
        TryDecode8(ref Unsafe.AddByteOffset(ref encoded, 8), ref Unsafe.AddByteOffset(ref src, 3), map, out invalid);

    #endregion Decode32

    #region IsValid32

    public static bool IsValid32(ref byte encoded, sbyte[] map) =>
        IsValid24(ref encoded, map) &&
        IsValid8(ref Unsafe.AddByteOffset(ref encoded, 4), map);

    public static bool IsValid32(ref char encoded, sbyte[] map) =>
        IsValid24(ref encoded, map) &&
        IsValid8(ref Unsafe.AddByteOffset(ref encoded, 8), map);

    public static bool IsValid32(ref byte encoded, sbyte[] map, out byte invalid) =>
        IsValid24(ref encoded, map, out invalid) &&
        IsValid8(ref Unsafe.AddByteOffset(ref encoded, 4), map, out invalid);

    public static bool IsValid32(ref char encoded, sbyte[] map, out char invalid) =>
        IsValid24(ref encoded, map, out invalid) &&
        IsValid8(ref Unsafe.AddByteOffset(ref encoded, 8), map, out invalid);

    #endregion IsValid32

    #region Encode24

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode24(ref byte src, ref byte encoded, byte[] map)
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
    public static void Encode24(ref byte src, ref char encoded, char[] map)
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
    public static bool TryDecode24(ref byte encoded, ref byte src, sbyte[] map)
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
    public static bool TryDecode24(ref char encoded, ref byte src, sbyte[] map)
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
    public static bool TryDecode24(ref byte encoded, ref byte src, sbyte[] map, out byte invalid)
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
    public static bool TryDecode24(ref char encoded, ref byte src, sbyte[] map, out char invalid)
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
    public static bool IsValid24(ref byte encoded, sbyte[] map)
        => (map[encoded] << 18 | map[Unsafe.AddByteOffset(ref encoded, 1)] << 12 |
            map[Unsafe.AddByteOffset(ref encoded, 2)] << 6 | (int)map[Unsafe.AddByteOffset(ref encoded, 3)]) >= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid24(ref char encoded, sbyte[] map)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 2);
        var i2 = Unsafe.AddByteOffset(ref encoded, 4);
        var i3 = Unsafe.AddByteOffset(ref encoded, 6);
        if (((encoded | i1 | i2 | i3) & 0xffffff00) != 0) return false;
        return (map[encoded] << 18 | map[i1] << 12 | map[i2] << 6 | (int)map[i3]) >= 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid24(ref byte encoded, sbyte[] map, out byte invalid)
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
    public static bool IsValid24(ref char encoded, sbyte[] map, out char invalid)
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
    public static void Encode16(ref byte src, ref byte encoded, byte[] map)
    {
        int i = src << 16 | Unsafe.AddByteOffset(ref src, 1) << 8;
        encoded = map[i >> 18];
        Unsafe.AddByteOffset(ref encoded, 1) = map[i >> 12 & 0x3F];
        Unsafe.AddByteOffset(ref encoded, 2) = map[i >> 6 & 0x3F];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode16(ref byte src, ref char encoded, char[] map)
    {
        int i = src << 16 | Unsafe.AddByteOffset(ref src, 1) << 8;
        encoded = map[i >> 18];
        Unsafe.AddByteOffset(ref encoded, 2) = map[i >> 12 & 0x3F];
        Unsafe.AddByteOffset(ref encoded, 4) = map[i >> 6 & 0x3F];
    }

    #endregion Encode16

    #region Decode16

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDecode16(ref byte encoded, ref byte src, sbyte[] map)
    {
        var val = map[encoded] << 18 | map[Unsafe.AddByteOffset(ref encoded, 1)] << 12 |
                  map[Unsafe.AddByteOffset(ref encoded, 2)] << 6;
        if (val < 0) return false;

        src = (byte)(val >> 16);
        Unsafe.AddByteOffset(ref src, 1) = (byte)(val >> 8);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDecode16(ref char encoded, ref byte src, sbyte[] map)
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
    public static bool TryDecode16(ref byte encoded, ref byte src, sbyte[] map, out byte invalid)
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
    public static bool TryDecode16(ref char encoded, ref byte src, sbyte[] map, out char invalid)
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
    public static bool IsValid16(ref byte encoded, sbyte[] map)
        => (map[encoded] << 18 | map[Unsafe.AddByteOffset(ref encoded, 1)] << 12 |
            map[Unsafe.AddByteOffset(ref encoded, 2)] << 6) >= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid16(ref char encoded, sbyte[] map)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 2);
        var i2 = Unsafe.AddByteOffset(ref encoded, 4);
        if (((encoded | i1 | i2) & 0xffffff00) != 0) return false;

        return (map[encoded] << 18 | map[i1] << 12 | map[i2] << 6) >= 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid16(ref byte encoded, sbyte[] map, out byte invalid)
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
    public static bool IsValid16(ref char encoded, sbyte[] map, out char invalid)
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
    public static void Encode8(ref byte src, ref byte encoded, byte[] map)
    {
        int i = src << 8;
        encoded = map[i >> 10];
        Unsafe.AddByteOffset(ref encoded, 1) = map[i >> 4 & 0x3F];

        //slowly
        //encoded = map[(src & 0xfc) >> 2];
        //Unsafe.AddByteOffset(ref encoded, 1) = map[(src & 0x03) << 4];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode8(ref byte src, ref char encoded, char[] map)
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
    public static bool TryDecode8(ref byte encoded, ref byte src, sbyte[] map)
    {
        var val = map[encoded] << 18 | map[Unsafe.AddByteOffset(ref encoded, 1)] << 12;
        if (val < 0) return false;

        src = (byte)(val >> 16);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDecode8(ref char encoded, ref byte src, sbyte[] map)
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
    public static bool TryDecode8(ref byte encoded, ref byte src, sbyte[] map, out byte invalid)
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
    public static bool TryDecode8(ref char encoded, ref byte src, sbyte[] map, out char invalid)
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
    public static bool IsValid8(ref byte encoded, sbyte[] map)
        => (map[encoded] << 18 | map[Unsafe.AddByteOffset(ref encoded, 1)] << 12) >= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid8(ref char encoded, sbyte[] map)
    {
        var i1 = Unsafe.AddByteOffset(ref encoded, 2);
        if (((encoded | i1) & 0xffffff00) != 0) return false;

        //TODO: Testing ???
        //if ((encoded | i1) > 255) return false;

        return (map[encoded] << 18 | map[i1] << 12) >= 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid8(ref byte encoded, sbyte[] map, out byte invalid)
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
    public static bool IsValid8(ref char encoded, sbyte[] map, out char invalid)
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
}