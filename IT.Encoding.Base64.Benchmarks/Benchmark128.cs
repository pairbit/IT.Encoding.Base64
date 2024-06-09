using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using IT.Encoding.Base64.Tests;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace IT.Encoding.Base64.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class Benchmark128
{
    private Guid _guid;
    private string _encodedString = null!;
    private byte[] _encodedBytes = null!;
    private Struct176 _encodedStruct = default;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _guid = Guid.NewGuid();
        _encodedString = EncodeToString_Simple();
        _encodedBytes = EncodeToBytes_Simple();
        _encodedStruct = EncodeToStruct_IT_Vector();
    }

    #region EncodeToString

    //[Benchmark]
    public string EncodeToString_Convert()
        => Convert.ToBase64String(_guid.ToByteArray()).Replace("/", "_").Replace("+", "-").Replace("=", "");

    //[Benchmark]
    public string EncodeToString_Simple() => SimpleEncodeToString(_guid);

    //[Benchmark]
    public string EncodeToString_gfoidl() => gfoidlEncodeToString(_guid);

    [Benchmark]
    public string EncodeToString_IT_Vector() => VectorEncodeToString(_guid);

    [Benchmark]
    public string EncodeToString_IT_VectorRef() => VectorEncodeToStringRef(_guid);

    [Benchmark]
    public string EncodeToString_IT_NoVector() => NoVectorEncodeToString(_guid);

    //[Benchmark]
    public string EncodeToString_IT_NoVectorRef() => NoVectorEncodeToStringRef(_guid);
    
    [Benchmark]
    public string EncodedToString_IT() => Base64.ToString(_encodedStruct);

    [Benchmark]
    public string EncodedToString_IT_Vector() => string.Create(22, _encodedStruct, static (chars, encoded) =>
    {
        ref byte b = ref Unsafe.As<Struct176, byte>(ref encoded);
        ref short s = ref Unsafe.As<char, short>(ref MemoryMarshal.GetReference(chars));
        var v = Vector128.LoadUnsafe(ref b);
        Sse2.UnpackLow(v, Vector128<byte>.Zero).AsInt16().StoreUnsafe(ref s);
        Sse2.UnpackHigh(v, Vector128<byte>.Zero).AsInt16().StoreUnsafe(ref s, 8);
        chars[16] = (char)Unsafe.AddByteOffset(ref b, 16);
        chars[17] = (char)Unsafe.AddByteOffset(ref b, 17);
        chars[18] = (char)Unsafe.AddByteOffset(ref b, 18);
        chars[19] = (char)Unsafe.AddByteOffset(ref b, 19);
        chars[20] = (char)Unsafe.AddByteOffset(ref b, 20);
        chars[21] = (char)Unsafe.AddByteOffset(ref b, 21);
    });

    [Benchmark]
    public string EncodedToString_IT_VectorRef()
    {
        var newStr = new string('\0', 22);
        ref char ch = ref Unsafe.AsRef(in newStr.GetPinnableReference());
        ref byte b = ref Unsafe.As<Struct176, byte>(ref _encodedStruct);
        ref short s = ref Unsafe.As<char, short>(ref ch);
        var v = Vector128.LoadUnsafe(ref b);
        Sse2.UnpackLow(v, Vector128<byte>.Zero).AsInt16().StoreUnsafe(ref s);
        Sse2.UnpackHigh(v, Vector128<byte>.Zero).AsInt16().StoreUnsafe(ref s, 8);
        Unsafe.AddByteOffset(ref ch, 32) = (char)Unsafe.AddByteOffset(ref b, 16);
        Unsafe.AddByteOffset(ref ch, 34) = (char)Unsafe.AddByteOffset(ref b, 17);
        Unsafe.AddByteOffset(ref ch, 36) = (char)Unsafe.AddByteOffset(ref b, 18);
        Unsafe.AddByteOffset(ref ch, 38) = (char)Unsafe.AddByteOffset(ref b, 19);
        Unsafe.AddByteOffset(ref ch, 40) = (char)Unsafe.AddByteOffset(ref b, 20);
        Unsafe.AddByteOffset(ref ch, 42) = (char)Unsafe.AddByteOffset(ref b, 21);
        return newStr;
    }

    #endregion EncodeToString

    #region EncodeToBytes

    //[Benchmark]
    public byte[] EncodeToBytes_Simple() => SimpleEncodeToBytes(_guid);

    [Benchmark]
    public byte[] EncodeToBytes_gfoidl() => gfoidlEncodeToBytes(_guid);

    [Benchmark]
    public byte[] EncodeToBytes_IT_Vector() => VectorEncodeToBytes(_guid);
    
    [Benchmark]
    public Struct176 EncodeToStruct_IT_Vector() => VectorEncodeToStruct(_guid);

    [Benchmark]
    public byte[] EncodeToBytes_IT_NoVector() => NoVectorEncodeToBytes(_guid);

    #endregion EncodeToBytes

    #region DecodeFromString

    //[Benchmark]
    public Guid DecodeFromString_Convert()
        => Unsafe.As<byte, Guid>(ref Convert.FromBase64String(_encodedString.Replace("_", "/").Replace("-", "+") + "==")[0]);

    //[Benchmark]
    public Guid DecodeFromString_gfoidl() => gfoidlDecodeFromString(_encodedString);

    //[Benchmark]
    public Guid DecodeFromString_IT_Vector() => VectorDecodeFromString(_encodedString);

    //[Benchmark]
    public Guid DecodeFromString_IT_NoVector() => NoVectorDecodeFromString(_encodedString);

    #endregion DecodeFromString

    #region DecodeFromBytes

    //[Benchmark]
    public Guid DecodeFromBytes_gfoidl() => gfoidlDecodeFromBytes(_encodedBytes);

    //[Benchmark]
    public Guid DecodeFromBytes_IT_Vector() => VectorDecodeFromBytes(_encodedBytes);

    //[Benchmark]
    public Guid DecodeFromBytes_IT_NoVector() => NoVectorDecodeFromBytes(_encodedBytes);

    #endregion DecodeFromBytes

    public void Test()
    {
        for (int i = 0; i < 100; i++)
        {
            GlobalSetup();

            var str = EncodeToString_Convert();
            if (!str.Equals(EncodeToString_Simple())) throw new InvalidOperationException(nameof(EncodeToString_Simple));
            if (!str.Equals(EncodeToString_gfoidl())) throw new InvalidOperationException(nameof(EncodeToString_gfoidl));
            if (!str.Equals(EncodeToString_IT_NoVector())) throw new InvalidOperationException(nameof(EncodeToString_IT_NoVector));
            if (!str.Equals(EncodeToString_IT_NoVectorRef())) throw new InvalidOperationException(nameof(EncodeToString_IT_NoVectorRef));
            if (!str.Equals(EncodeToString_IT_Vector())) throw new InvalidOperationException(nameof(EncodeToString_IT_Vector));
            if (!str.Equals(EncodeToString_IT_VectorRef())) throw new InvalidOperationException(nameof(EncodeToString_IT_VectorRef));
            if (!str.Equals(EncodedToString_IT())) throw new InvalidOperationException(nameof(EncodedToString_IT));
            if (!str.Equals(EncodedToString_IT_Vector())) throw new InvalidOperationException(nameof(EncodedToString_IT_Vector));
            if (!str.Equals(EncodedToString_IT_VectorRef())) throw new InvalidOperationException(nameof(EncodedToString_IT_VectorRef));

            var bytes = EncodeToBytes_Simple();
            if (!bytes.SequenceEqual(EncodeToBytes_gfoidl())) throw new InvalidOperationException(nameof(EncodeToBytes_gfoidl));
            if (!bytes.SequenceEqual(EncodeToBytes_IT_NoVector())) throw new InvalidOperationException(nameof(EncodeToBytes_IT_NoVector));
            if (!bytes.SequenceEqual(EncodeToBytes_IT_Vector())) throw new InvalidOperationException(nameof(EncodeToBytes_IT_Vector));

            if (!System.Text.Encoding.UTF8.GetString(bytes).Equals(str)) throw new InvalidOperationException();

            var guid = _guid;
            if (!guid.Equals(DecodeFromString_Convert())) throw new InvalidOperationException(nameof(DecodeFromString_Convert));
            if (!guid.Equals(DecodeFromString_gfoidl())) throw new InvalidOperationException(nameof(DecodeFromString_gfoidl));
            if (!guid.Equals(DecodeFromString_IT_NoVector())) throw new InvalidOperationException(nameof(DecodeFromString_IT_NoVector));
            if (!guid.Equals(DecodeFromString_IT_Vector())) throw new InvalidOperationException(nameof(DecodeFromString_IT_Vector));
            if (!guid.Equals(DecodeFromBytes_gfoidl())) throw new InvalidOperationException(nameof(DecodeFromBytes_gfoidl));
            if (!guid.Equals(DecodeFromBytes_IT_NoVector())) throw new InvalidOperationException(nameof(DecodeFromBytes_IT_NoVector));
            if (!guid.Equals(DecodeFromBytes_IT_Vector())) throw new InvalidOperationException(nameof(DecodeFromBytes_IT_Vector));
        }
    }

    private const byte PlusByte = (byte)'+';
    private const char DashChar = '-';
    private const byte DashByte = (byte)DashChar;

    private const byte ForwardSlashByte = (byte)'/';
    private const char UnderscoreChar = '_';
    private const byte UnderscoreByte = (byte)UnderscoreChar;

    private static string SimpleEncodeToString(Guid guid) => string.Create(22, guid, static (chars, value) =>
    {
        Span<byte> guidBytes = stackalloc byte[16];
        Unsafe.As<byte, Guid>(ref MemoryMarshal.GetReference(guidBytes)) = value;

        Span<byte> encodedBytes = stackalloc byte[24];
        System.Buffers.Text.Base64.EncodeToUtf8(guidBytes, encodedBytes, out _, out _);

        for (var i = 0; i < 22; i++)
        {
            chars[i] = encodedBytes[i] switch
            {
                ForwardSlashByte => UnderscoreChar,
                PlusByte => DashChar,
                _ => (char)encodedBytes[i],
            };
        }
    });

    private static byte[] SimpleEncodeToBytes(Guid guid)
    {
        Span<byte> guidBytes = stackalloc byte[16];
        Unsafe.As<byte, Guid>(ref MemoryMarshal.GetReference(guidBytes)) = guid;

        Span<byte> encodedBytes = stackalloc byte[24];
        System.Buffers.Text.Base64.EncodeToUtf8(guidBytes, encodedBytes, out _, out _);

        for (var i = 0; i < 22; i++)
        {
            switch (encodedBytes[i])
            {
                case ForwardSlashByte:
                    encodedBytes[i] = UnderscoreByte;
                    break;

                case PlusByte:
                    encodedBytes[i] = DashByte;
                    break;
            }
        }
        var bytes = new byte[22];
        encodedBytes[..22].CopyTo(bytes);
        return bytes;
    }

    private static string gfoidlEncodeToString(Guid value)
    {
        Span<byte> bytes = stackalloc byte[16];
        Unsafe.As<byte, Guid>(ref MemoryMarshal.GetReference(bytes)) = value;

        return gfoidl.Base64.Base64.Url.Encode(bytes);
    }

    private static byte[] gfoidlEncodeToBytes(Guid value)
    {
        Span<byte> guidBytes = stackalloc byte[16];
        Unsafe.As<byte, Guid>(ref MemoryMarshal.GetReference(guidBytes)) = value;

        var encodedBytes = new byte[22];
        gfoidl.Base64.Base64.Url.Encode(guidBytes, encodedBytes, out _, out _);

        return encodedBytes;
    }

    private static Guid gfoidlDecodeFromString(string encoded)
    {
        Span<byte> buffer = stackalloc byte[16];
        gfoidl.Base64.Base64.Url.Decode(encoded, buffer, out _, out _);
        return Unsafe.As<byte, Guid>(ref MemoryMarshal.GetReference(buffer));
    }

    private static Guid gfoidlDecodeFromBytes(byte[] encoded)
    {
        Span<byte> buffer = stackalloc byte[16];
        gfoidl.Base64.Base64.Url.Decode(encoded, buffer, out _, out _);
        return Unsafe.As<byte, Guid>(ref MemoryMarshal.GetReference(buffer));
    }

    private static string VectorEncodeToString(Guid value) => string.Create(22, value, static (chars, value) =>
    {
        VectorBase64Url.Encode128(ref Unsafe.As<Guid, byte>(ref value), ref MemoryMarshal.GetReference(chars));
    });

    //fasters, why?
    private static string VectorEncodeToStringRef(Guid value)
    {
        var newStr = new string('\0', 22);
        VectorBase64Url.Encode128(ref Unsafe.As<Guid, byte>(ref value), ref Unsafe.AsRef(in newStr.GetPinnableReference()));
        return newStr;
    }

    private static byte[] VectorEncodeToBytes(Guid value)
    {
        var encodedBytes = new byte[22];
        VectorBase64Url.Encode128(ref Unsafe.As<Guid, byte>(ref value), ref encodedBytes[0]);
        return encodedBytes;
    }

    private static Struct176 VectorEncodeToStruct(Guid value)
    {
        Struct176 encodedStruct = default;
        VectorBase64Url.Encode128(ref Unsafe.As<Guid, byte>(ref value), ref Unsafe.As<Struct176, byte>(ref encodedStruct));
        return encodedStruct;
    }

    private static Guid VectorDecodeFromString(string encoded)
    {
        Guid guid = default;
        //TODO: ref Unsafe.AsRef(in encoded.GetPinnableReference())
        VectorBase64Url.TryDecode128(ref MemoryMarshal.GetReference(encoded.AsSpan()), ref Unsafe.As<Guid, byte>(ref guid));
        return guid;
    }

    private static Guid VectorDecodeFromBytes(byte[] encoded)
    {
        Guid guid = default;
        //ref encoded[0]
        VectorBase64Url.TryDecode128(ref MemoryMarshal.GetReference(encoded.AsSpan()), ref Unsafe.As<Guid, byte>(ref guid));
        return guid;
    }

    private static string NoVectorEncodeToString(Guid value) => string.Create(22, value, static (chars, value) =>
    {
        UnsafeBase64.Encode128(Base64Url.Chars, ref Unsafe.As<Guid, byte>(ref value), ref MemoryMarshal.GetReference(chars));
    });

    //Its slowly, why????
    private static string NoVectorEncodeToStringRef(Guid value)
    {
        var newStr = new string('\0', 22);
        UnsafeBase64.Encode128(Base64Url.Chars, ref Unsafe.As<Guid, byte>(ref value), ref Unsafe.AsRef(in newStr.GetPinnableReference()));
        return newStr;
    }

    private static byte[] NoVectorEncodeToBytes(Guid value)
    {
        var encodedBytes = new byte[22];
        UnsafeBase64.Encode128(Base64Url.Bytes, ref Unsafe.As<Guid, byte>(ref value), ref encodedBytes[0]);
        return encodedBytes;
    }

    private static Guid NoVectorDecodeFromString(string encoded)
    {
        Guid guid = default;
        UnsafeBase64.TryDecode128(Base64Url.Map, ref MemoryMarshal.GetReference(encoded.AsSpan()), ref Unsafe.As<Guid, byte>(ref guid));
        return guid;
    }

    private static Guid NoVectorDecodeFromBytes(byte[] encoded)
    {
        Guid guid = default;
        UnsafeBase64.TryDecode128(Base64Url.Map, ref MemoryMarshal.GetReference(encoded.AsSpan()), ref Unsafe.As<Guid, byte>(ref guid));
        return guid;
    }
}