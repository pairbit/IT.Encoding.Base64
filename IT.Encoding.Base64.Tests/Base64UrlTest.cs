using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IT.Encoding.Base64.Tests;

using Encoding = System.Text.Encoding;

public class Base64UrlTest
{
    [Test]
    public void Test32()
    {
        Span<byte> buffer = stackalloc byte[sizeof(uint)];
        Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(buffer), uint.MaxValue);

        Assert.That(Convert.ToBase64String(buffer).TrimEnd('='), Is.EqualTo("/////w"));

        Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(buffer), uint.MinValue);
        Assert.That(Convert.ToBase64String(buffer).TrimEnd('='), Is.EqualTo("AAAAAA"));

        Assert.That(Test32(uint.MinValue), Is.EqualTo("AAAAAA"));
        Assert.That(Test32(uint.MaxValue), Is.EqualTo("_____w"));
        Assert.That(Test32(1046820453), Is.EqualTo("ZTZlPg"));
        Assert.That(Test32(1481472373), Is.EqualTo("dXlNWA"));

        var random = Random.Shared;
        for (var i = 0; i < 1000; i++)
        {
            Test32((uint)random.Next());
        }
    }

    [Test]
    public void Test64()
    {
        Assert.That(Test64(ulong.MinValue), Is.EqualTo("AAAAAAAAAAA"));
        Assert.That(Test64(ulong.MaxValue), Is.EqualTo("__________8"));
        Assert.That(Test64(1046820155012380993), Is.EqualTo("QXk7e2INhw4"));
        Assert.That(Test64(14814723746819689979), Is.EqualTo("-027hsF4mM0"));

        var random = Random.Shared;
        for (var i = 0; i < 1000; i++)
        {
            Test64((ulong)random.NextInt64());
        }
    }

    [Test]
    public void Test72()
    {
        Assert.That(Test72(new FileId(ulong.MinValue, byte.MinValue)), Is.EqualTo("AAAAAAAAAAAA"));
        Assert.That(Test72(new FileId(ulong.MaxValue, byte.MaxValue)), Is.EqualTo("____________"));
        Assert.That(Test72(new FileId(2235381257586707384, 1)), Is.EqualTo("uIsFBGmrBR8B"));
        Assert.That(Test72(new FileId(13495389474433244262, 1)), Is.EqualTo("ZpwjsTlBSbsB"));
        Assert.That(Test72(new FileId(10416025566214361379, 0)), Is.EqualTo("IxGY5RAojZAA"));

        var random = Random.Shared;
        for (var i = 0; i < 1000; i++)
        {
            Test72(new FileId((ulong)random.NextInt64(), (byte)random.Next()));
        }
    }

    [Test]
    public void Test128()
    {
        Span<byte> buffer = stackalloc byte[16];
        Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(buffer), UInt128.MaxValue);

        Assert.That(Convert.ToBase64String(buffer).TrimEnd('='), Is.EqualTo("/////////////////////w"));

        Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(buffer), UInt128.MinValue);
        Assert.That(Convert.ToBase64String(buffer).TrimEnd('='), Is.EqualTo("AAAAAAAAAAAAAAAAAAAAAA"));

        var value = new UInt128(10468201550123809991, 12468201550123809992);
        var str = Base64Url.Encode128ToString(value);
        Assert.That(str, Is.EqualTo("yLwaH0DzB63HvFLQ2IVGkQ"));
        Assert.That(Base64Url.Decode128(str), Is.EqualTo(value));

        Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(buffer), value);
        Assert.That(Convert.ToBase64String(buffer).TrimEnd('='), Is.EqualTo(str));

        var random = Random.Shared;
        for (var i = 0; i < 100; i++)
        {
            Test128(new UInt128((ulong)random.NextInt64(), (ulong)random.NextInt64()));
        }
    }

    private static string Test32(uint value)
    {
        var str = Base64Url.Encode32ToString(value);
        Assert.That(str, Is.EqualTo(new string(Base64Url.Encode32ToChars(value))));

        Span<byte> bytes = stackalloc byte[6];
        Assert.That(Base64Url.TryEncode32(value, bytes), Is.EqualTo(EncodingStatus.Done));
        Assert.That(str, Is.EqualTo(Encoding.ASCII.GetString(bytes)));
        bytes.Clear();
        Base64Url.Encode32(value, bytes);
        Assert.That(bytes.SequenceEqual(Base64Url.Encode32ToBytes(value)), Is.True);

        Base64Url.Valid32(bytes);
        Assert.That(Base64Url.IsValid32(bytes), Is.EqualTo(EncodingStatus.Done));
        Assert.That(Base64Url.IsValid32(bytes, out var invalidByte), Is.EqualTo(EncodingStatus.Done));
        Assert.That(invalidByte, Is.EqualTo(default(byte)));

        Span<char> chars = stackalloc char[6];
        Assert.That(Base64Url.TryEncode32(value, chars), Is.EqualTo(EncodingStatus.Done));
        Assert.That(str, Is.EqualTo(new string(chars)));
        chars.Clear();
        Base64Url.Encode32(value, chars);
        Assert.That(str, Is.EqualTo(new string(chars)));

        Base64Url.Valid32(chars);
        Assert.That(Base64Url.IsValid32(chars), Is.EqualTo(EncodingStatus.Done));
        Assert.That(Base64Url.IsValid32(chars, out var invalidChar), Is.EqualTo(EncodingStatus.Done));
        Assert.That(invalidChar, Is.EqualTo(default(char)));

        Assert.That(Base64Url.TryDecode32(bytes, out var decoded), Is.EqualTo(EncodingStatus.Done));
        Assert.That(decoded, Is.EqualTo(value));
        decoded = default;
        Assert.That(Base64Url.TryDecode32(bytes, out decoded, out invalidByte), Is.EqualTo(EncodingStatus.Done));
        Assert.That(decoded, Is.EqualTo(value));
        Assert.That(invalidByte, Is.EqualTo(default(byte)));

        Assert.That(Base64Url.Decode32(bytes), Is.EqualTo(value));
        Assert.That(str, Is.EqualTo(Encoding.ASCII.GetString(bytes)));

        decoded = default;
        Assert.That(Base64Url.TryDecode32(chars, out decoded), Is.EqualTo(EncodingStatus.Done));
        Assert.That(decoded, Is.EqualTo(value));
        decoded = default;
        Assert.That(Base64Url.TryDecode32(chars, out decoded, out invalidChar), Is.EqualTo(EncodingStatus.Done));
        Assert.That(decoded, Is.EqualTo(value));
        Assert.That(invalidChar, Is.EqualTo(default(char)));

        Assert.That(Base64Url.Decode32(chars), Is.EqualTo(value));
        Assert.That(str, Is.EqualTo(new string(chars)));

        Invalid32(bytes, chars);

        Assert.That(Base64Url.TryDecode32(stackalloc byte[5], out decoded), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(uint)));

        Assert.That(Base64Url.TryDecode32(stackalloc byte[7], out decoded), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(uint)));

        Assert.That(Base64Url.TryDecode32(stackalloc byte[5], out decoded, out invalidByte), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(uint)));
        Assert.That(invalidByte, Is.EqualTo(default(byte)));

        Assert.That(Base64Url.TryDecode32(stackalloc byte[7], out decoded, out invalidByte), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(uint)));
        Assert.That(invalidByte, Is.EqualTo(default(byte)));

        Assert.That(Base64Url.TryDecode32(stackalloc char[5], out decoded), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(uint)));

        Assert.That(Base64Url.TryDecode32(stackalloc char[7], out decoded), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(uint)));

        Assert.That(Base64Url.TryDecode32(stackalloc char[5], out decoded, out invalidChar), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(uint)));
        Assert.That(invalidChar, Is.EqualTo(default(char)));

        Assert.That(Base64Url.TryDecode32(stackalloc char[7], out decoded, out invalidChar), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(uint)));
        Assert.That(invalidChar, Is.EqualTo(default(char)));

        return str;
    }

    private static void Invalid32(ReadOnlySpan<byte> bytes, ReadOnlySpan<char> chars)
    {
        Assert.That(bytes.Length, Is.EqualTo(chars.Length));

        Span<byte> buffer = stackalloc byte[sizeof(uint)];
        uint value = default;
        var m = _decodeMap;
        var offset = bytes.Length - 1;
        Span<byte> invalidBytes = stackalloc byte[bytes.Length];
        Span<char> invalidChars = stackalloc char[bytes.Length];
        for (byte b = 0; b < 255; b++)
        {
            if (m[b] != -1) continue;

            bytes.CopyTo(invalidBytes);
            invalidBytes[offset] = b;
            Assert.That(Base64Url.TryDecode32(invalidBytes, out value), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(uint)));

            Assert.That(Base64Url.TryDecode32(invalidBytes, out value, out var invalidByte), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(uint)));
            Assert.That(invalidByte, Is.EqualTo(b));

            chars.CopyTo(invalidChars);
            invalidChars[offset] = (char)b;
            Assert.That(Base64Url.TryDecode32(invalidChars, out value), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(uint)));

            Assert.That(Base64Url.TryDecode32(invalidChars, out value, out var invalidChar), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(uint)));
            Assert.That(invalidChar, Is.EqualTo((char)b));

            if (--offset < 0) offset = bytes.Length - 1;
        }
        offset = 256;
        for (int i = 0; i < invalidChars.Length; i++)
        {
            chars.CopyTo(invalidChars);
            invalidChars[i] = (char)offset;
            Assert.That(Base64Url.TryDecode32(invalidChars, out value), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(uint)));

            Assert.That(Base64Url.TryDecode32(invalidChars, out value, out var invalidChar), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(uint)));
            Assert.That(invalidChar, Is.EqualTo((char)offset));
            offset++;
        }
    }

    private static string Test64(ulong value)
    {
        var str = Base64Url.Encode64ToString(value);
        Assert.That(str, Is.EqualTo(new string(Base64Url.Encode64ToChars(value))));

        Span<byte> bytes = stackalloc byte[11];
        Assert.That(Base64Url.TryEncode64(value, bytes), Is.EqualTo(EncodingStatus.Done));
        Assert.That(str, Is.EqualTo(Encoding.ASCII.GetString(bytes)));
        bytes.Clear();
        Base64Url.Encode64(value, bytes);
        Assert.That(bytes.SequenceEqual(Base64Url.Encode64ToBytes(value)), Is.True);

        Base64Url.Valid64(bytes);
        Assert.That(Base64Url.IsValid64(bytes), Is.EqualTo(EncodingStatus.Done));
        Assert.That(Base64Url.IsValid64(bytes, out var invalidByte), Is.EqualTo(EncodingStatus.Done));
        Assert.That(invalidByte, Is.EqualTo(default(byte)));

        Span<char> chars = stackalloc char[11];
        Assert.That(Base64Url.TryEncode64(value, chars), Is.EqualTo(EncodingStatus.Done));
        Assert.That(str, Is.EqualTo(new string(chars)));
        chars.Clear();
        Base64Url.Encode64(value, chars);
        Assert.That(str, Is.EqualTo(new string(chars)));

        Base64Url.Valid64(chars);
        Assert.That(Base64Url.IsValid64(chars), Is.EqualTo(EncodingStatus.Done));
        Assert.That(Base64Url.IsValid64(chars, out var invalidChar), Is.EqualTo(EncodingStatus.Done));
        Assert.That(invalidChar, Is.EqualTo(default(char)));

        Span<byte> buffer = stackalloc byte[sizeof(ulong)];
        Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(buffer), value);

        bytes.Clear();
        Assert.That(Base64Url.TryEncode64(buffer, bytes), Is.EqualTo(EncodingStatus.Done));
        Assert.That(Base64Url.TryDecode64(bytes, out var decoded), Is.EqualTo(EncodingStatus.Done));
        Assert.That(decoded, Is.EqualTo(value));
        decoded = default;
        Assert.That(Base64Url.TryDecode64(bytes, out decoded, out invalidByte), Is.EqualTo(EncodingStatus.Done));
        Assert.That(decoded, Is.EqualTo(value));
        Assert.That(invalidByte, Is.EqualTo(default(byte)));
        bytes.Clear();
        Base64Url.Encode64(buffer, bytes);
        Assert.That(Base64Url.Decode64(bytes), Is.EqualTo(value));
        Assert.That(str, Is.EqualTo(Encoding.ASCII.GetString(bytes)));

        chars.Clear();
        Assert.That(Base64Url.TryEncode64(buffer, chars), Is.EqualTo(EncodingStatus.Done));
        decoded = default;
        Assert.That(Base64Url.TryDecode64(chars, out decoded), Is.EqualTo(EncodingStatus.Done));
        Assert.That(decoded, Is.EqualTo(value));
        decoded = default;
        Assert.That(Base64Url.TryDecode64(chars, out decoded, out invalidChar), Is.EqualTo(EncodingStatus.Done));
        Assert.That(decoded, Is.EqualTo(value));
        Assert.That(invalidChar, Is.EqualTo(default(char)));
        chars.Clear();
        Base64Url.Encode64(buffer, chars);
        Assert.That(Base64Url.Decode64(chars), Is.EqualTo(value));
        Assert.That(str, Is.EqualTo(new string(chars)));

        Invalid64(bytes, chars);

        Assert.That(Base64Url.TryDecode64(stackalloc byte[10], out decoded), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(ulong)));

        Assert.That(Base64Url.TryDecode64(stackalloc byte[12], out decoded), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(ulong)));

        Assert.That(Base64Url.TryDecode64(stackalloc byte[10], out decoded, out invalidByte), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(ulong)));
        Assert.That(invalidByte, Is.EqualTo(default(byte)));

        Assert.That(Base64Url.TryDecode64(stackalloc byte[12], out decoded, out invalidByte), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(ulong)));
        Assert.That(invalidByte, Is.EqualTo(default(byte)));

        Assert.That(Base64Url.TryDecode64(stackalloc char[10], out decoded), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(ulong)));

        Assert.That(Base64Url.TryDecode64(stackalloc char[12], out decoded), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(ulong)));

        Assert.That(Base64Url.TryDecode64(stackalloc char[10], out decoded, out invalidChar), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(ulong)));
        Assert.That(invalidChar, Is.EqualTo(default(char)));

        Assert.That(Base64Url.TryDecode64(stackalloc char[12], out decoded, out invalidChar), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(ulong)));
        Assert.That(invalidChar, Is.EqualTo(default(char)));

        return str;
    }

    private static void Invalid64(ReadOnlySpan<byte> bytes, ReadOnlySpan<char> chars)
    {
        Assert.That(bytes.Length, Is.EqualTo(chars.Length));

        Span<byte> buffer = stackalloc byte[sizeof(ulong)];
        ulong value = default;
        var m = _decodeMap;
        var offset = bytes.Length - 1;
        Span<byte> invalidBytes = stackalloc byte[bytes.Length];
        Span<char> invalidChars = stackalloc char[bytes.Length];
        for (byte b = 0; b < 255; b++)
        {
            if (m[b] != -1) continue;

            bytes.CopyTo(invalidBytes);
            invalidBytes[offset] = b;
            Assert.That(Base64Url.TryDecode64(invalidBytes, out value), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(ulong)));

            Assert.That(Base64Url.TryDecode64(invalidBytes, out value, out var invalidByte), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(ulong)));
            Assert.That(invalidByte, Is.EqualTo(b));

            chars.CopyTo(invalidChars);
            invalidChars[offset] = (char)b;
            Assert.That(Base64Url.TryDecode64(invalidChars, out value), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(ulong)));

            Assert.That(Base64Url.TryDecode64(invalidChars, out value, out var invalidChar), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(ulong)));
            Assert.That(invalidChar, Is.EqualTo((char)b));

            if (--offset < 0) offset = bytes.Length - 1;
        }
        offset = 256;
        for (int i = 0; i < invalidChars.Length; i++)
        {
            chars.CopyTo(invalidChars);
            invalidChars[i] = (char)offset;
            Assert.That(Base64Url.TryDecode64(invalidChars, out value), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(ulong)));

            Assert.That(Base64Url.TryDecode64(invalidChars, out value, out var invalidChar), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(ulong)));
            Assert.That(invalidChar, Is.EqualTo((char)offset));
            offset++;
        }
    }

    private static string Test72<T>(T value) where T : unmanaged
    {
        var str = Base64Url.Encode72ToString(value);
        Assert.That(str, Is.EqualTo(new string(Base64Url.Encode72ToChars(value))));

        Span<byte> bytes = stackalloc byte[12];
        Assert.That(Base64Url.TryEncode72(value, bytes), Is.EqualTo(EncodingStatus.Done));
        Assert.That(str, Is.EqualTo(Encoding.ASCII.GetString(bytes)));
        bytes.Clear();
        Base64Url.Encode72(value, bytes);
        Assert.That(bytes.SequenceEqual(Base64Url.Encode72ToBytes(value)), Is.True);

        Base64Url.Valid72(bytes);
        Assert.That(Base64Url.IsValid72(bytes), Is.EqualTo(EncodingStatus.Done));
        Assert.That(Base64Url.IsValid72(bytes, out var invalidByte), Is.EqualTo(EncodingStatus.Done));
        Assert.That(invalidByte, Is.EqualTo(default(byte)));

        Span<char> chars = stackalloc char[12];
        Assert.That(Base64Url.TryEncode72(value, chars), Is.EqualTo(EncodingStatus.Done));
        Assert.That(str, Is.EqualTo(new string(chars)));
        chars.Clear();
        Base64Url.Encode72(value, chars);
        Assert.That(str, Is.EqualTo(new string(chars)));

        Base64Url.Valid72(chars);
        Assert.That(Base64Url.IsValid72(chars), Is.EqualTo(EncodingStatus.Done));
        Assert.That(Base64Url.IsValid72(chars, out var invalidChar), Is.EqualTo(EncodingStatus.Done));
        Assert.That(invalidChar, Is.EqualTo(default(char)));

        Span<byte> buffer = stackalloc byte[Unsafe.SizeOf<T>()];
        Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(buffer), value);

        bytes.Clear();
        Assert.That(Base64Url.TryEncode72(buffer, bytes), Is.EqualTo(EncodingStatus.Done));
        Assert.That(Base64Url.TryDecode72<T>(bytes, out var decoded), Is.EqualTo(EncodingStatus.Done));
        Assert.That(decoded, Is.EqualTo(value));
        decoded = default;
        Assert.That(Base64Url.TryDecode72(bytes, out decoded, out invalidByte), Is.EqualTo(EncodingStatus.Done));
        Assert.That(decoded, Is.EqualTo(value));
        Assert.That(invalidByte, Is.EqualTo(default(byte)));
        bytes.Clear();
        Base64Url.Encode72(buffer, bytes);
        Assert.That(Base64Url.Decode72<T>(bytes), Is.EqualTo(value));
        Assert.That(str, Is.EqualTo(Encoding.ASCII.GetString(bytes)));

        chars.Clear();
        Assert.That(Base64Url.TryEncode72(buffer, chars), Is.EqualTo(EncodingStatus.Done));
        decoded = default;
        Assert.That(Base64Url.TryDecode72(chars, out decoded), Is.EqualTo(EncodingStatus.Done));
        Assert.That(decoded, Is.EqualTo(value));
        decoded = default;
        Assert.That(Base64Url.TryDecode72(chars, out decoded, out invalidChar), Is.EqualTo(EncodingStatus.Done));
        Assert.That(decoded, Is.EqualTo(value));
        Assert.That(invalidChar, Is.EqualTo(default(char)));
        chars.Clear();
        Base64Url.Encode72(buffer, chars);
        Assert.That(Base64Url.Decode72<T>(chars), Is.EqualTo(value));
        Assert.That(str, Is.EqualTo(new string(chars)));

        Invalid72(bytes, chars);

        Assert.That(Base64Url.TryDecode72(stackalloc byte[11], out decoded), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(T)));

        Assert.That(Base64Url.TryDecode72(stackalloc byte[13], out decoded), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(T)));

        Assert.That(Base64Url.TryDecode72(stackalloc byte[11], out decoded, out invalidByte), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(T)));
        Assert.That(invalidByte, Is.EqualTo(default(byte)));

        Assert.That(Base64Url.TryDecode72(stackalloc byte[13], out decoded, out invalidByte), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(T)));
        Assert.That(invalidByte, Is.EqualTo(default(byte)));

        Assert.That(Base64Url.TryDecode72(stackalloc char[11], out decoded), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(T)));

        Assert.That(Base64Url.TryDecode72(stackalloc char[13], out decoded), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(T)));

        Assert.That(Base64Url.TryDecode72(stackalloc char[11], out decoded, out invalidChar), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(T)));
        Assert.That(invalidChar, Is.EqualTo(default(char)));

        Assert.That(Base64Url.TryDecode72(stackalloc char[13], out decoded, out invalidChar), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(T)));
        Assert.That(invalidChar, Is.EqualTo(default(char)));

        Assert.That(Base64Url.TryDecode72<ulong>(bytes, out var longDest), Is.EqualTo(EncodingStatus.InvalidDestinationLength));
        Assert.That(decoded, Is.EqualTo(default(T)));

        Assert.That(Base64Url.TryDecode72(chars, out longDest), Is.EqualTo(EncodingStatus.InvalidDestinationLength));
        Assert.That(decoded, Is.EqualTo(default(T)));

        Assert.That(Base64Url.TryDecode72(bytes, out longDest, out invalidByte), Is.EqualTo(EncodingStatus.InvalidDestinationLength));
        Assert.That(decoded, Is.EqualTo(default(T)));
        Assert.That(invalidByte, Is.EqualTo(default(byte)));

        Assert.That(Base64Url.TryDecode72(chars, out longDest, out invalidChar), Is.EqualTo(EncodingStatus.InvalidDestinationLength));
        Assert.That(decoded, Is.EqualTo(default(T)));
        Assert.That(invalidByte, Is.EqualTo(default(byte)));

        return str;
    }

    private static void Invalid72(ReadOnlySpan<byte> bytes, ReadOnlySpan<char> chars)
    {
        Assert.That(bytes.Length, Is.EqualTo(chars.Length));

        Span<byte> buffer = stackalloc byte[9];
        FileId value = default;
        var m = _decodeMap;
        var offset = bytes.Length - 1;
        Span<byte> invalidBytes = stackalloc byte[bytes.Length];
        Span<char> invalidChars = stackalloc char[bytes.Length];
        for (byte b = 0; b < 255; b++)
        {
            if (m[b] != -1) continue;

            bytes.CopyTo(invalidBytes);
            invalidBytes[offset] = b;
            Assert.That(Base64Url.TryDecode72(invalidBytes, out value), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(FileId)));

            Assert.That(Base64Url.TryDecode72(invalidBytes, out value, out var invalidByte), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(FileId)));
            Assert.That(invalidByte, Is.EqualTo(b));

            chars.CopyTo(invalidChars);
            invalidChars[offset] = (char)b;
            Assert.That(Base64Url.TryDecode72(invalidChars, out value), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(FileId)));

            Assert.That(Base64Url.TryDecode72(invalidChars, out value, out var invalidChar), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(FileId)));
            Assert.That(invalidChar, Is.EqualTo((char)b));

            if (--offset < 0) offset = bytes.Length - 1;
        }
        offset = 256;
        for (int i = 0; i < invalidChars.Length; i++)
        {
            chars.CopyTo(invalidChars);
            invalidChars[i] = (char)offset;
            Assert.That(Base64Url.TryDecode72(invalidChars, out value), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(FileId)));

            Assert.That(Base64Url.TryDecode72(invalidChars, out value, out var invalidChar), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(FileId)));
            Assert.That(invalidChar, Is.EqualTo((char)offset));
            offset++;
        }
    }

    private static string Test128(UInt128 value)
    {
        var str = Base64Url.Encode128ToString(value);
        Assert.That(str, Is.EqualTo(new string(Base64Url.Encode128ToChars(value))));

        Span<byte> bytes = stackalloc byte[22];
        Assert.That(Base64Url.TryEncode128(value, bytes), Is.EqualTo(EncodingStatus.Done));
        Assert.That(str, Is.EqualTo(Encoding.ASCII.GetString(bytes)));
        bytes.Clear();
        Base64Url.Encode128(value, bytes);
        Assert.That(bytes.SequenceEqual(Base64Url.Encode128ToBytes(value)), Is.True);

        Base64Url.Valid128(bytes);
        Assert.That(Base64Url.IsValid128(bytes), Is.EqualTo(EncodingStatus.Done));
        Assert.That(Base64Url.IsValid128(bytes, out var invalidByte), Is.EqualTo(EncodingStatus.Done));
        Assert.That(invalidByte, Is.EqualTo(default(byte)));

        Span<char> chars = stackalloc char[22];
        Assert.That(Base64Url.TryEncode128(value, chars), Is.EqualTo(EncodingStatus.Done));
        Assert.That(str, Is.EqualTo(new string(chars)));
        chars.Clear();
        Base64Url.Encode128(value, chars);
        Assert.That(str, Is.EqualTo(new string(chars)));

        Base64Url.Valid128(chars);
        Assert.That(Base64Url.IsValid128(chars), Is.EqualTo(EncodingStatus.Done));
        Assert.That(Base64Url.IsValid128(chars, out var invalidChar), Is.EqualTo(EncodingStatus.Done));
        Assert.That(invalidChar, Is.EqualTo(default(char)));

        Assert.That(Base64Url.TryDecode128(bytes, out var decoded), Is.EqualTo(EncodingStatus.Done));
        Assert.That(decoded, Is.EqualTo(value));
        decoded = default;
        Assert.That(Base64Url.TryDecode128(bytes, out decoded, out invalidByte), Is.EqualTo(EncodingStatus.Done));
        Assert.That(decoded, Is.EqualTo(value));
        Assert.That(invalidByte, Is.EqualTo(default(byte)));

        Assert.That(Base64Url.Decode128(bytes), Is.EqualTo(value));
        Assert.That(str, Is.EqualTo(Encoding.ASCII.GetString(bytes)));

        decoded = default;
        Assert.That(Base64Url.TryDecode128(chars, out decoded), Is.EqualTo(EncodingStatus.Done));
        Assert.That(decoded, Is.EqualTo(value));
        decoded = default;
        Assert.That(Base64Url.TryDecode128(chars, out decoded, out invalidChar), Is.EqualTo(EncodingStatus.Done));
        Assert.That(decoded, Is.EqualTo(value));
        Assert.That(invalidChar, Is.EqualTo(default(char)));

        Assert.That(Base64Url.Decode128(chars), Is.EqualTo(value));
        Assert.That(str, Is.EqualTo(new string(chars)));

        Invalid128(bytes, chars);

        Assert.That(Base64Url.TryDecode128(stackalloc byte[21], out decoded), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(UInt128)));

        Assert.That(Base64Url.TryDecode128(stackalloc byte[23], out decoded), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(UInt128)));

        Assert.That(Base64Url.TryDecode128(stackalloc byte[21], out decoded, out invalidByte), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(UInt128)));
        Assert.That(invalidByte, Is.EqualTo(default(byte)));

        Assert.That(Base64Url.TryDecode128(stackalloc byte[23], out decoded, out invalidByte), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(UInt128)));
        Assert.That(invalidByte, Is.EqualTo(default(byte)));

        Assert.That(Base64Url.TryDecode128(stackalloc char[21], out decoded), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(UInt128)));

        Assert.That(Base64Url.TryDecode128(stackalloc char[23], out decoded), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(UInt128)));

        Assert.That(Base64Url.TryDecode128(stackalloc char[21], out decoded, out invalidChar), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(UInt128)));
        Assert.That(invalidChar, Is.EqualTo(default(char)));

        Assert.That(Base64Url.TryDecode128(stackalloc char[23], out decoded, out invalidChar), Is.EqualTo(EncodingStatus.InvalidDataLength));
        Assert.That(decoded, Is.EqualTo(default(UInt128)));
        Assert.That(invalidChar, Is.EqualTo(default(char)));

        return str;
    }

    private static void Invalid128(ReadOnlySpan<byte> bytes, ReadOnlySpan<char> chars)
    {
        Assert.That(bytes.Length, Is.EqualTo(chars.Length));

        Span<byte> buffer = stackalloc byte[16];
        UInt128 value = default;
        var m = _decodeMap;
        var offset = bytes.Length - 1;
        Span<byte> invalidBytes = stackalloc byte[bytes.Length];
        Span<char> invalidChars = stackalloc char[bytes.Length];
        for (byte b = 0; b < 255; b++)
        {
            if (m[b] != -1) continue;

            bytes.CopyTo(invalidBytes);
            invalidBytes[offset] = b;
            Assert.That(Base64Url.TryDecode128(invalidBytes, out value), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(UInt128)));

            Assert.That(Base64Url.TryDecode128(invalidBytes, out value, out var invalidByte), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(UInt128)));
            Assert.That(invalidByte, Is.EqualTo(b));

            chars.CopyTo(invalidChars);
            invalidChars[offset] = (char)b;
            Assert.That(Base64Url.TryDecode128(invalidChars, out value), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(UInt128)));

            Assert.That(Base64Url.TryDecode128(invalidChars, out value, out var invalidChar), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(UInt128)));
            Assert.That(invalidChar, Is.EqualTo((char)b));

            if (--offset < 0) offset = bytes.Length - 1;
        }
        offset = 256;
        for (int i = 0; i < invalidChars.Length; i++)
        {
            chars.CopyTo(invalidChars);
            invalidChars[i] = (char)offset;
            Assert.That(Base64Url.TryDecode128(invalidChars, out value), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(UInt128)));

            Assert.That(Base64Url.TryDecode128(invalidChars, out value, out var invalidChar), Is.EqualTo(EncodingStatus.InvalidData));
            Assert.That(value, Is.EqualTo(default(UInt128)));
            Assert.That(invalidChar, Is.EqualTo((char)offset));
            offset++;
        }
    }

    private static readonly sbyte[] _decodeMap = [
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
}