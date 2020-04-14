using System;
using System.Text;
using ClassicCiphers.Exceptions;

namespace ClassicCiphers.Ciphers
{
    public class CaesarCipher : GenericCipher
    {

        public static String DefaultKeyString = "21";
        public CaesarCipher()
        {
        }

        /*
         * Checks if the string is only formed of an integer, otherwise throws an exception.
         */
        protected override CipherKey CheckKeyValidity(String key)
        {
            CipherKey cipherKey = new CipherKey();
            if (key.Equals("") || !int.TryParse(key, out int tempKey))
                throw new InvalidKeyFormatException("The key introduced for the caesar cipher is not an integer!");
            cipherKey.SetNumericalValue(tempKey % 26);
            return cipherKey;

        }

        /*
         * Projects a character onto the ascii interval of the alphabet.
         */ 

        private char WrapAlphabetAscii(char x)
        {
            if (x > 'z')
            {
                return (char)('a' + x - 'z' - 1);
            }
            else if (x < 'a')
            {
                return (char)('z' - 'a' + x + 1);
            }
            else return x;
        }

        public override void SetKey(String key)
        {
            this.Key = CheckKeyValidity(key);
        }

        /*
         * To encrypt text the function adds to the ascii value of each character the key given % 26.
         */
        public override String Encrypt(String text)
        {
            StringBuilder sb = new StringBuilder(text);
            for (int i = 0; i < text.Length; i++)
            {
                if (sb[i] > 'z' || sb[i] < 'a')
                    continue;
                sb[i] = WrapAlphabetAscii((char)(text[i] + Key.NumericalValue));
            }
            return sb.ToString();
        }

        /*
         * To decrypt it deducts from the ascii value.
         */
        public override String Decrypt(String text)
        {
            StringBuilder sb = new StringBuilder(text);
            for (int i = 0; i < text.Length; i++)
            {
                if (sb[i] > 'z' || sb[i] < 'a')
                    continue;
                sb[i] = WrapAlphabetAscii((char)(text[i] - Key.NumericalValue));
            }
            return sb.ToString();

        }
        
        public override string GetKeyValue()
        {
            return Key.StringValue;
        }
        public override string ToString()
        {
            return "Caesar";
        }
    }
}