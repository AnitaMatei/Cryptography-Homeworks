using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicCiphers.Ciphers
{
    class Playfair : GenericCipher
    {
        public static String DefaultKeyString = "bhu";

        PolybiusSquare MyPolybiusSquare;

        public Playfair()
        {
            MyPolybiusSquare = new PolybiusSquare();
        }

        protected override CipherKey CheckKeyValidity(String key)
        {
            CipherKey cipherKey = new CipherKey();
            for (int i = 0; i < key.Length; i++)
            {
                if (key[i] < 'a' || key[i] > 'z')
                    throw new FormatException("The key introduced for the playfair cipher contains more than alphabet letters!");
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
                sbToReturn = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (!MyPolybiusSquare.ContainsCharacter(text[i]))
                    continue;
                sb.Append(text[i]);
            }
            if (sb.Length % 2 == 1)
            {
                sb.Append('x');
            }
            for (int i = 0; i < sb.Length; i += 2)
            {
                sbToReturn.Append(EncryptCharacterPair(sb[i], sb[i + 1]));
            }

            return sbToReturn.ToString();
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
                int.TryParse(polybiusValues[i].ToString() + polybiusValues[polybiusValues.Length / 2 + i].ToString(), out int value);
                sb.Append(MyPolybiusSquare.GetElementOfValue(value));
            }

            return sb.ToString();

        }

        private StringBuilder EncryptCharacterPair(char x, char y)
        {
            StringBuilder sb = new StringBuilder();
            int xColumnValue = MyPolybiusSquare.GetColumnValueOf(x),
                xLineValue = MyPolybiusSquare.GetLineValueOf(x),
                yColumnValue = MyPolybiusSquare.GetColumnValueOf(y),
                yLineValue = MyPolybiusSquare.GetLineValueOf(y);

            if (xColumnValue == yColumnValue)
            {
                int xEncryptedVal = WrapLineValue(xLineValue + 1) * 10 + xColumnValue;
                int yEncryptedVal = WrapLineValue(yLineValue + 1) * 10 + yColumnValue;
                sb.Append(MyPolybiusSquare.GetElementOfValue(xEncryptedVal));
                sb.Append(MyPolybiusSquare.GetElementOfValue(yEncryptedVal));
            }
            else if (xLineValue == yLineValue)
            {
                int xEncryptedVal = xLineValue * 10 + WrapColumnValue(xColumnValue + 1);
                int yEncryptedVal = yLineValue * 10 + WrapColumnValue(yColumnValue + 1);
                sb.Append(MyPolybiusSquare.GetElementOfValue(xEncryptedVal));
                sb.Append(MyPolybiusSquare.GetElementOfValue(yEncryptedVal));
            }
            else
            {
                int xEncryptedVal = yLineValue * 10 + xColumnValue;
                int yEncryptedVal = xLineValue * 10 + yColumnValue;
                sb.Append(MyPolybiusSquare.GetElementOfValue(xEncryptedVal));
                sb.Append(MyPolybiusSquare.GetElementOfValue(yEncryptedVal));
            }


            return sb;
        }

        private int WrapLineValue(int x)
        {
            if (x > 5)
                return 1;
            else return x + 1;
        }

        private int WrapColumnValue(int x)
        {
            if (x > 6)
                return 1;
            else return x + 1;
        }

        public override string GetKeyValue()
        {
            return Key.StringValue;
        }


    }
}

