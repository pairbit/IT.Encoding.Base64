using System.Runtime.InteropServices;

namespace IT.Encoding.Base64.Tests;

[StructLayout(LayoutKind.Explicit, Size = 9)]
public readonly struct FileId
{
    [FieldOffset(0)]
    private readonly ulong _hash;

    [FieldOffset(8)]
    private readonly byte _serverId;

    public ulong Hash => _hash;

    public byte ServerId => _serverId;

    public FileId(ulong hash, byte serverId)
    {
        _hash = hash;
        _serverId = serverId;
    }

    public FileId(ulong hash) : this(hash, 0) { }
}