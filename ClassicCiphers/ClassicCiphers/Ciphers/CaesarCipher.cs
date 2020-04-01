using System;
using System.Text;

namespace ClassicCiphers.Ciphers
{
    public class CaesarCipher : GenericCipher
    {

        public CaesarCipher()
        {
        }

        protected override CipherKey CheckKeyValidity(String key)
        {
            int tempKey;
            CipherKey cipherKey = new CipherKey();
            if (key.Equals("") || !int.TryParse(key, out tempKey))
                throw new FormatException("The key introduced for the caesar cipher is not an integer!");
            cipherKey.SetNumericalValue(tempKey % 26);
            return cipherKey;

        }

        private char WrapAlphabetAscii(char x)
        {
            if (x > 'z')
            {
                return (char)('a' + x - 'z' - 1);
            }
            else return x;
        }

        public override void SetKey(String key)
        {
            this.Key = CheckKeyValidity(key);
        }

        public override String Encrypt(String text)
        {
            StringBuilder sb = new StringBuilder(text);
            for (int i = 0; i < text.Length; i++)
            {
                if (sb[i] == ' ')
                    continue;
                sb[i] = WrapAlphabetAscii((char)(text[i] + Key.NumericalValue));
            }
            return sb.ToString();
        }
        public override String Decrypt(String text)
        {
            StringBuilder sb = new StringBuilder(text);
            for (int i = 0; i < text.Length; i++)
            {
                sb[i] = WrapAlphabetAscii((char)(text[i] - Key.NumericalValue));
            }
            return sb.ToString();

        }
    }
}