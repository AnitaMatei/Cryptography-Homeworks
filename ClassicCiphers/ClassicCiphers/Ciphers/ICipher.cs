using System;

namespace ClassicCiphers.Ciphers
{
    interface ICipher
    {
        void SetKey(String keyString);
        String Encrypt(String text);
        String Decrypt(String text);

    }
}