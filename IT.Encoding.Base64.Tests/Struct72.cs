using System.Runtime.InteropServices;

namespace IT.Encoding.Base64.Tests;

[StructLayout(LayoutKind.Sequential, Size = 9)]
public readonly struct Struct72
{
    public ulong L0 { get; init; }

    public byte B1 { get; init; }

    public Struct72(ulong l0, byte b1)
    {
        L0 = l0;
        B1 = b1;
    }
}