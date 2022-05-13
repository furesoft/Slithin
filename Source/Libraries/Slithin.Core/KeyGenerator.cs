namespace Slithin.Core;

public class KeyGenerator
{
    public static byte[] Generate()
    {
        int keyBits = 4096;
        string keyComment = "mykey";

        var keygen = new SshKeyGenerator.SshKeyGenerator(keyBits);

        Console.WriteLine(keygen.ToPrivateKey());
        Console.WriteLine(keygen.ToRfcPublicKey(keyComment));

        return null;
    }
}
