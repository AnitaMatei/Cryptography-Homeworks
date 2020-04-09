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
                if (key[i] < 'a' || key[i] > 'z')
                    throw new FormatException("The key introduced for the bifid cipher contains more than alphabet letters!");
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
            LinkedList<int> spaces = new LinkedList<int>();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] < 'a' || text[i] > 'z')
                {
                    if (text[i] == ' ')
                        spaces.AddLast(i);
                    continue;
                }

                firstHalf.Append(MyPolybiusSquare.GetLineValueOf(text[i]));
                secondHalf.Append(MyPolybiusSquare.GetColumnValueOf(text[i]));
            }

            String polybiusNumbers = firstHalf.Append(secondHalf).ToString();
            int j = 0;
            for (int i = 0; i < polybiusNumbers.Length / 2; i++)
            {
                if (j < spaces.Count && i == spaces.ElementAt(j) - j)
                {
                    sb.Append(" ");
                    j++;
                }
                int.TryParse(polybiusNumbers.Substring(i * 2, 2), out int value);
                sb.Append(MyPolybiusSquare.GetElementOfValue(value));
            }

            return sb.ToString();
        }
        public override String Decrypt(String text)
        {
            StringBuilder sb = new StringBuilder(),
                polybiusValues = new StringBuilder();
            LinkedList<int> spaces = new LinkedList<int>();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] < 'a' || text[i] > 'z')
                {
                    if (text[i] == ' ')
                        spaces.AddLast(i);
                    continue;
                }
                polybiusValues.Append(MyPolybiusSquare.GetValueOf(text[i]));
            }

            int j = 0;
            for (int i = 0; i < polybiusValues.Length / 2; i++)
            {
                //the spaces positions are offset by the amount of previous spaces found, 
                //so we deduct the number of found spaces from the current space's position
                if (j < spaces.Count && i == spaces.ElementAt(j) - j)
                {
                    sb.Append(" ");
                    j++;
                }
                int.TryParse(polybiusValues[i].ToString()+polybiusValues[polybiusValues.Length/2+i].ToString(), out int value);
                sb.Append(MyPolybiusSquare.GetElementOfValue(value));
            }

            return sb.ToString();

        }
        public override string GetKeyValue()
        {
            return Key.StringValue;
        }
    }
}
