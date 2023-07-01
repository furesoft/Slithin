using System.Text;

namespace Slithin.Core;

public static class KeyGenerator
{
    public static byte[] Generate()
    {
        int keyBits = 4096;

        var keygen = new SshKeyGenerator.SshKeyGenerator(keyBits);

        return Encoding.ASCII.GetBytes(keygen.ToPrivateKey());
    }
}
