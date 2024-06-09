using System.Runtime.InteropServices;

namespace IT.Encoding.Base64.Tests;

[StructLayout(LayoutKind.Sequential, Size = 9)]
public readonly struct Struct72
{
    public ulong L { get; init; }

    public byte B { get; init; }

    public Struct72(ulong l, byte b)
    {
        L = l;
        B = b;
    }
}