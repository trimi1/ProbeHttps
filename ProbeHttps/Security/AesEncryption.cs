using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace ProbeHttps.Security;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class AesEncryption
{
    private readonly byte[] _key;

    public AesEncryption(string key)
    {
        _key = Convert.FromBase64String(key);
    }
    
    public byte[] Encrypt(string plainText)
    {
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
    
        var iv = new byte[12];
        new Random().NextBytes(iv);
    
        var keyBytes = _key;
    
        var cipher = new GcmBlockCipher(new AesEngine());
        var parameters = new AeadParameters(new KeyParameter(keyBytes), 128, iv, Encoding.UTF8.GetBytes("ProtocolVersion1"));
        cipher.Init(true, parameters);
    
        var cipherBytes = new byte[cipher.GetOutputSize(plainBytes.Length)];
        var len = cipher.ProcessBytes(plainBytes, 0, plainBytes.Length, cipherBytes, 0);
        cipher.DoFinal(cipherBytes, len);
    
        var cipherText = new byte[iv.Length + cipherBytes.Length];
        Array.Copy(iv, 0, cipherText, 0, iv.Length);
        Array.Copy(cipherBytes, 0, cipherText, iv.Length, cipherBytes.Length);
    
        return cipherText;
    }
    
    public string Decrypt(byte[] cipherText)
    {
        var iv = cipherText.Take(12).ToArray();
        var actualCipherText = cipherText.Skip(12).ToArray();
    
        var keyBytes = _key;
    
        var cipher = new GcmBlockCipher(new AesEngine());
        
        var parameters = new AeadParameters(new KeyParameter(keyBytes), 128, iv, Encoding.UTF8.GetBytes("ProtocolVersion1"));
        cipher.Init(false, parameters);
    
        var plainBytes = new byte[cipher.GetOutputSize(actualCipherText.Length)];
        var len = cipher.ProcessBytes(actualCipherText, 0, actualCipherText.Length, plainBytes, 0);
        cipher.DoFinal(plainBytes, len);
    
        return Encoding.UTF8.GetString(plainBytes);
    }
}