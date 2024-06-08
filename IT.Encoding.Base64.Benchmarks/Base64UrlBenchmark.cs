using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IT.Encoding.Base64.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class Base64UrlBenchmark
{
    private Guid _guid;
    private string _encodedString = null!;
    private byte[] _encodedBytes = null!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _guid = Guid.NewGuid();
        _encodedString = EncodeToString_Simple();
        _encodedBytes = EncodeToBytes_Simple();
    }

    #region EncodeToString

    //[Benchmark]
    public string EncodeToString_Convert()
        => Convert.ToBase64String(_guid.ToByteArray()).Replace("/", "_").Replace("+", "-").Replace("=", "");

    //[Benchmark]
    public string EncodeToString_Simple() => SimpleEncodeToString(_guid);

    //[Benchmark]
    public string EncodeToString_gfoidl() => gfoidlEncodeToString(_guid);

    //[Benchmark]
    public string EncodeToString_IT_Vector() => VectorEncodeToString(_guid);

    //[Benchmark]
    public string EncodeToString_IT_NoVector() => NoVectorEncodeToString(_guid);

    #endregion EncodeToString

    #region EncodeToBytes

    [Benchmark]
    public byte[] EncodeToBytes_Simple() => SimpleEncodeToBytes(_guid);

    //[Benchmark]
    public byte[] EncodeToBytes_gfoidl() => gfoidlEncodeToBytes(_guid);

    //[Benchmark]
    public byte[] EncodeToBytes_IT_Vector() => VectorEncodeToBytes(_guid);

    //[Benchmark]
    public byte[] EncodeToBytes_IT_NoVector() => NoVectorEncodeToBytes(_guid);

    #endregion EncodeToBytes

    #region DecodeFromString

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
        for (int i = 0; i < 1000; i++)
        {
            GlobalSetup();

            var str = EncodeToString_Convert();
            if (!str.Equals(EncodeToString_Simple())) throw new InvalidOperationException(nameof(EncodeToString_Simple));
            if (!str.Equals(EncodeToString_gfoidl())) throw new InvalidOperationException(nameof(EncodeToString_gfoidl));
            if (!str.Equals(EncodeToString_IT_NoVector())) throw new InvalidOperationException(nameof(EncodeToString_IT_NoVector));
            if (!str.Equals(EncodeToString_IT_Vector())) throw new InvalidOperationException(nameof(EncodeToString_IT_Vector));

            var bytes = EncodeToBytes_Simple();
            if (!bytes.SequenceEqual(EncodeToBytes_gfoidl())) throw new InvalidOperationException(nameof(EncodeToBytes_gfoidl));
            if (!bytes.SequenceEqual(EncodeToBytes_IT_NoVector())) throw new InvalidOperationException(nameof(EncodeToBytes_IT_NoVector));
            if (!bytes.SequenceEqual(EncodeToBytes_IT_Vector())) throw new InvalidOperationException(nameof(EncodeToBytes_IT_Vector));

            if (!System.Text.Encoding.UTF8.GetString(bytes).Equals(str)) throw new InvalidOperationException();

            var guid = _guid;
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
        Base64Url.VectorEncode128(ref Unsafe.As<Guid, byte>(ref value), ref MemoryMarshal.GetReference(chars), Base64Url.Chars);
    });

    private static byte[] VectorEncodeToBytes(Guid value)
    {
        var encodedBytes = new byte[22];
        Base64Url.VectorEncode128(ref Unsafe.As<Guid, byte>(ref value), ref encodedBytes[0], Base64Url.Bytes);
        return encodedBytes;
    }

    private static Guid VectorDecodeFromString(string encoded)
    {
        Guid guid = default;
        Base64Url.TryVectorDecode128(ref MemoryMarshal.GetReference(encoded.AsSpan()), ref Unsafe.As<Guid, byte>(ref guid), Base64Url.Map);
        return guid;
    }

    private static Guid VectorDecodeFromBytes(byte[] encoded)
    {
        Guid guid = default;
        Base64Url.TryVectorDecode128(ref MemoryMarshal.GetReference(encoded.AsSpan()), ref Unsafe.As<Guid, byte>(ref guid), Base64Url.Map);
        return guid;
    }

    private static string NoVectorEncodeToString(Guid value) => string.Create(22, value, static (chars, value) =>
    {
        UnsafeBase64.Encode128(ref Unsafe.As<Guid, byte>(ref value), ref MemoryMarshal.GetReference(chars), Base64Url.Chars);
    });

    private static byte[] NoVectorEncodeToBytes(Guid value)
    {
        var encodedBytes = new byte[22];
        UnsafeBase64.Encode128(ref Unsafe.As<Guid, byte>(ref value), ref encodedBytes[0], Base64Url.Bytes);
        return encodedBytes;
    }

    private static Guid NoVectorDecodeFromString(string encoded)
    {
        Guid guid = default;
        UnsafeBase64.TryDecode128(ref MemoryMarshal.GetReference(encoded.AsSpan()), ref Unsafe.As<Guid, byte>(ref guid), Base64Url.Map);
        return guid;
    }

    private static Guid NoVectorDecodeFromBytes(byte[] encoded)
    {
        Guid guid = default;
        UnsafeBase64.TryDecode128(ref MemoryMarshal.GetReference(encoded.AsSpan()), ref Unsafe.As<Guid, byte>(ref guid), Base64Url.Map);
        return guid;
    }
}