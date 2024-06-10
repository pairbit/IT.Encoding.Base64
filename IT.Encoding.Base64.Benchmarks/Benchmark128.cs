using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using IT.Encoding.Base64.Tests;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
    public string EncodeToString_gfoidl()
    {
        Span<byte> bytes = stackalloc byte[16];
        Unsafe.As<byte, Guid>(ref MemoryMarshal.GetReference(bytes)) = _guid;

        return gfoidl.Base64.Base64.Url.Encode(bytes);
    }

    //[Benchmark]
    public string EncodeToString_IT_Vector() => string.Create(22, _guid, static (chars, value) =>
    {
        VectorBase64Url.Encode128(ref Unsafe.As<Guid, byte>(ref value), ref MemoryMarshal.GetReference(chars));
    });

    //[Benchmark]
    public string EncodeToString_IT_VectorRef()
    {
        var newStr = new string('\0', 22);
        VectorBase64Url.Encode128(ref Unsafe.As<Guid, byte>(ref _guid), ref Unsafe.AsRef(in newStr.GetPinnableReference()));
        return newStr;
    }

    //[Benchmark]
    public string EncodeToString_IT() => string.Create(22, _guid, static (chars, value) =>
    {
        UnsafeBase64.Encode128(Base64Url.Chars, ref Unsafe.As<Guid, byte>(ref value), ref MemoryMarshal.GetReference(chars));
    });

    //[Benchmark]
    public string EncodeToString_IT_Ref()
    {
        var newStr = new string('\0', 22);
        UnsafeBase64.Encode128(Base64Url.Chars, ref Unsafe.As<Guid, byte>(ref _guid), ref Unsafe.AsRef(in newStr.GetPinnableReference()));
        return newStr;
    }

    //[Benchmark]
    public string EncodedToString_IT()
    {
        var str = new string('\0', 22);
        UnsafeBase64.ToChar176(ref Unsafe.As<Struct176, byte>(ref _encodedStruct), ref Unsafe.AsRef(in str.GetPinnableReference()));
        return str;
    }

    //[Benchmark]
    public string EncodedToString_IT_Vector()
    {
        var str = new string('\0', 22);
        VectorBase64.ToChar176(ref Unsafe.As<Struct176, byte>(ref _encodedStruct), ref Unsafe.AsRef(in str.GetPinnableReference()));
        return str;
    }

    #endregion EncodeToString

    #region EncodeToBytes

    //[Benchmark]
    public byte[] EncodeToBytes_Simple() => SimpleEncodeToBytes(_guid);

    //[Benchmark]
    public byte[] EncodeToBytes_gfoidl()
    {
        Span<byte> guidBytes = stackalloc byte[16];
        Unsafe.As<byte, Guid>(ref MemoryMarshal.GetReference(guidBytes)) = _guid;

        var encodedBytes = new byte[22];
        gfoidl.Base64.Base64.Url.Encode(guidBytes, encodedBytes, out _, out _);

        return encodedBytes;
    }

    //[Benchmark]
    public byte[] EncodeToBytes_IT_Vector()
    {
        var encodedBytes = new byte[22];
        VectorBase64Url.Encode128(ref Unsafe.As<Guid, byte>(ref _guid), ref encodedBytes[0]);
        return encodedBytes;
    }

    //[Benchmark]
    public byte[] EncodeToBytes_IT()
    {
        var encodedBytes = new byte[22];
        UnsafeBase64.Encode128(Base64Url.Bytes, ref Unsafe.As<Guid, byte>(ref _guid), ref encodedBytes[0]);
        return encodedBytes;
    }

    //[Benchmark]
    public Struct176 EncodeToStruct_IT_Vector()
    {
        Struct176 encodedStruct = default;
        VectorBase64Url.Encode128(ref Unsafe.As<Guid, byte>(ref _guid), ref Unsafe.As<Struct176, byte>(ref encodedStruct));
        return encodedStruct;
    }

    #endregion EncodeToBytes

    #region DecodeFromString

    //[Benchmark]
    public Guid DecodeFromString_Convert()
        => Unsafe.As<byte, Guid>(ref Convert.FromBase64String(_encodedString.Replace("_", "/").Replace("-", "+") + "==")[0]);

    //[Benchmark]
    public Guid DecodeFromString_gfoidl()
    {
        Span<byte> buffer = stackalloc byte[16];
        gfoidl.Base64.Base64.Url.Decode(_encodedString, buffer, out _, out _);
        return Unsafe.As<byte, Guid>(ref MemoryMarshal.GetReference(buffer));
    }

    [Benchmark]
    public Guid DecodeFromString_IT_Vector()
    {
        Guid guid = default;
        VectorBase64Url.TryDecode128(ref Unsafe.AsRef(in _encodedString.GetPinnableReference()), ref Unsafe.As<Guid, byte>(ref guid));
        return guid;
    }

    [Benchmark]
    public Guid DecodeFromString_IT()
    {
        Guid guid = default;
        UnsafeBase64.TryDecode128(Base64Url.Map, ref Unsafe.AsRef(in _encodedString.GetPinnableReference()), ref Unsafe.As<Guid, byte>(ref guid));
        return guid;
    }

    #endregion DecodeFromString

    #region DecodeFromBytes

    //[Benchmark]
    public Guid DecodeFromBytes_gfoidl()
    {
        Span<byte> buffer = stackalloc byte[16];
        gfoidl.Base64.Base64.Url.Decode(_encodedBytes, buffer, out _, out _);
        return Unsafe.As<byte, Guid>(ref MemoryMarshal.GetReference(buffer));
    }

    //[Benchmark]
    public Guid DecodeFromBytes_IT_Vector()
    {
        Guid guid = default;
        VectorBase64Url.TryDecode128(ref _encodedBytes[0], ref Unsafe.As<Guid, byte>(ref guid));
        return guid;
    }

    //[Benchmark]
    public Guid DecodeFromBytes_IT()
    {
        Guid guid = default;
        UnsafeBase64.TryDecode128(Base64Url.Map, ref _encodedBytes[0], ref Unsafe.As<Guid, byte>(ref guid));
        return guid;
    }

    #endregion DecodeFromBytes

    public void Test()
    {
        for (int i = 0; i < 100; i++)
        {
            GlobalSetup();

            var str = EncodeToString_Convert();
            if (!str.Equals(EncodeToString_Simple())) throw new InvalidOperationException(nameof(EncodeToString_Simple));
            if (!str.Equals(EncodeToString_gfoidl())) throw new InvalidOperationException(nameof(EncodeToString_gfoidl));
            if (!str.Equals(EncodeToString_IT())) throw new InvalidOperationException(nameof(EncodeToString_IT));
            if (!str.Equals(EncodeToString_IT_Ref())) throw new InvalidOperationException(nameof(EncodeToString_IT_Ref));
            if (!str.Equals(EncodeToString_IT_Vector())) throw new InvalidOperationException(nameof(EncodeToString_IT_Vector));
            if (!str.Equals(EncodeToString_IT_VectorRef())) throw new InvalidOperationException(nameof(EncodeToString_IT_VectorRef));
            if (!str.Equals(EncodedToString_IT())) throw new InvalidOperationException(nameof(EncodedToString_IT));
            if (!str.Equals(EncodedToString_IT_Vector())) throw new InvalidOperationException(nameof(EncodedToString_IT_Vector));

            var bytes = EncodeToBytes_Simple();
            if (!bytes.SequenceEqual(EncodeToBytes_gfoidl())) throw new InvalidOperationException(nameof(EncodeToBytes_gfoidl));
            if (!bytes.SequenceEqual(EncodeToBytes_IT())) throw new InvalidOperationException(nameof(EncodeToBytes_IT));
            if (!bytes.SequenceEqual(EncodeToBytes_IT_Vector())) throw new InvalidOperationException(nameof(EncodeToBytes_IT_Vector));

            if (!System.Text.Encoding.UTF8.GetString(bytes).Equals(str)) throw new InvalidOperationException();

            var guid = _guid;
            if (!guid.Equals(DecodeFromString_Convert())) throw new InvalidOperationException(nameof(DecodeFromString_Convert));
            if (!guid.Equals(DecodeFromString_gfoidl())) throw new InvalidOperationException(nameof(DecodeFromString_gfoidl));
            if (!guid.Equals(DecodeFromString_IT())) throw new InvalidOperationException(nameof(DecodeFromString_IT));
            if (!guid.Equals(DecodeFromString_IT_Vector())) throw new InvalidOperationException(nameof(DecodeFromString_IT_Vector));
            if (!guid.Equals(DecodeFromBytes_gfoidl())) throw new InvalidOperationException(nameof(DecodeFromBytes_gfoidl));
            if (!guid.Equals(DecodeFromBytes_IT())) throw new InvalidOperationException(nameof(DecodeFromBytes_IT));
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
}