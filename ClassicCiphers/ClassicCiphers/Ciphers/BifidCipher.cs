using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicCiphers.Ciphers
{
    class BifidCipher : GenericCipher
    {
        public static String DefaultKeyString = "zas";

        PolybiusSquare MyPolybiusSquare;

        public BifidCipher()
        {
            MyPolybiusSquare = new PolybiusSquare();
        }

        protected override CipherKey CheckKeyValidity(String key)
        {
            CipherKey cipherKey = new CipherKey();
            for (int i = 0; i < key.Length; i++)
            {
                if (!MyPolybiusSquare.ContainsCharacter(key[i]))
                    throw new FormatException("The key introduced for the bifid cipher contains more than the permitted characters!");
            }

            cipherKey.SetStringValue(key);
            return cipherKey;

        }


        public override void SetKey(String key)
        {
            this.Key = CheckKeyValidity(key);
            MyPolybiusSquare.CreateCheckerboard(key);
        }

        public override String Encrypt(String text)
        {
            StringBuilder sb = new StringBuilder(),
                firstHalf = new StringBuilder(),
                secondHalf = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (!MyPolybiusSquare.ContainsCharacter(text[i]))
                    continue;

                firstHalf.Append(MyPolybiusSquare.GetLineValueOf(text[i]));
                secondHalf.Append(MyPolybiusSquare.GetColumnValueOf(text[i]));
            }

            String polybiusNumbers = firstHalf.Append(secondHalf).ToString();
            for (int i = 0; i < polybiusNumbers.Length / 2; i++)
            {
                int.TryParse(polybiusNumbers.Substring(i * 2, 2), out int value);
                sb.Append(MyPolybiusSquare.GetElementOfValue(value));
            }

            return sb.ToString();
        }
        public override String Decrypt(String text)
        {
            StringBuilder sb = new StringBuilder(),
                polybiusValues = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (!MyPolybiusSquare.ContainsCharacter(text[i]))
                    continue;

                polybiusValues.Append(MyPolybiusSquare.GetValueOf(text[i]));
            }
            
            for (int i = 0; i < polybiusValues.Length / 2; i++)
            {
                int.TryParse(polybiusValues[i].ToString()+polybiusValues[polybiusValues.Length/2+i].ToString(), out int value);
                sb.Append(MyPolybiusSquare.GetElementOfValue(value));
            }

            return sb.ToString();

        }
        public override string GetKeyValue()
        {
            return Key.StringValue;
        }

        public override string ToString()
        {
            return "Bifid";
        }
    }
}
