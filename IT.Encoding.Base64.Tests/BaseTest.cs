using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace IT.Encoding.Base64.Tests;

public class BaseTest
{
    [Test]
    public void SizeOfTest()
    {
        Assert.That(Unsafe.SizeOf<FileId>, Is.EqualTo(9));
        Assert.That(Unsafe.SizeOf<Struct176>, Is.EqualTo(22));
    }

    [Test]
    public void IntrinsicsTest()
    {
        Assert.That(BitConverter.IsLittleEndian, Is.True);
        Assert.That(Vector128.IsHardwareAccelerated, Is.True);
        Assert.That(Avx2.IsSupported, Is.True);
        Assert.That(Ssse3.IsSupported, Is.True);
        //Assert.That(AdvSimd.Arm64.IsSupported, Is.True);
        
        Assert.That(Vector128<short>.Count, Is.EqualTo(8));
        Assert.That(Vector128<byte>.Count, Is.EqualTo(16));
        Assert.That(Vector256<byte>.Count, Is.EqualTo(32));
    }
}