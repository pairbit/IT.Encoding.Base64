using System.Runtime.InteropServices;

namespace IT.Encoding.Base64.Tests;

[StructLayout(LayoutKind.Sequential, Size = 3)]
public readonly struct Struct24
{
    public ushort S0 { get; init; }

    public byte B1 { get; init; }

    public Struct24(ushort s0, byte b1)
    {
        S0 = s0;
        B1 = b1;
    }
}