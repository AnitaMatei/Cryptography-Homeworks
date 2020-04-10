﻿using System;
using System.Text;

namespace ClassicCiphers.Ciphers
{
    public class CaesarCipher : GenericCipher
    {

        public static String DefaultKeyString = "21";
        public CaesarCipher()
        {
        }

        protected override CipherKey CheckKeyValidity(String key)
        {
            CipherKey cipherKey = new CipherKey();
            if (key.Equals("") || !int.TryParse(key, out int tempKey))
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

        //private bool ReplaceSpaceCharacter(ref StringBuilder sb, char character)
        //{
        //    if (character == ' ')
        //    {
        //        sb.Append(SpaceReplacement);
        //        return true;
        //    }
        //    return false;
        //}
        public override string GetKeyValue()
        {
            return Key.StringValue;
        }
    }
}