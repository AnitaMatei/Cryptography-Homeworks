using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicCiphers.Ciphers
{
    class PlayfairCipher : GenericCipher
    {
        public static String DefaultKeyString = "bhu";

        PolybiusSquare MyPolybiusSquare;

        public PlayfairCipher()
        {
            MyPolybiusSquare = new PolybiusSquare();
        }

        protected override CipherKey CheckKeyValidity(String key)
        {
            CipherKey cipherKey = new CipherKey();
            for (int i = 0; i < key.Length; i++)
            {
                if (!MyPolybiusSquare.ContainsCharacter(key[i]))
                    throw new FormatException("The key introduced for the playfair cipher contains more than the permitted characters!");
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
            if (text.Length % 2 == 1)
                throw new IndexOutOfRangeException("The input text provided for encryption with playfair has an uneven number of characters!");
            for (int i = 0; i < text.Length; i++)
            {
                if (!MyPolybiusSquare.ContainsCharacter(text[i]))
                    continue;
                sb.Append(text[i]);
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
                sbToReturn = new StringBuilder();
            if (text.Length % 2 == 1)
                throw new IndexOutOfRangeException("The input text provided for decryption with playfair has an uneven number of characters!");
            for (int i = 0; i < text.Length; i++)
            {
                if (!MyPolybiusSquare.ContainsCharacter(text[i]))
                    continue;
                sb.Append(text[i]);
            }

            for (int i = 0; i < sb.Length; i += 2)
            {
                char x = sb[i];
                char y = sb[i + 1];
                sbToReturn.Append(DecryptCharacterPair(x,y));
            }

            return sbToReturn.ToString();
        }

        private StringBuilder EncryptCharacterPair(char x, char y)
        {
            return TransformCharacterPair(x, y, 1);
        }
        private StringBuilder DecryptCharacterPair(char x, char y)
        {
            return TransformCharacterPair(x, y, -1);
        }

        private StringBuilder TransformCharacterPair(char x, char y, int direction)
        {

            StringBuilder sb = new StringBuilder();
            int xColumnValue = MyPolybiusSquare.GetColumnValueOf(x),
                xLineValue = MyPolybiusSquare.GetLineValueOf(x),
                yColumnValue = MyPolybiusSquare.GetColumnValueOf(y),
                yLineValue = MyPolybiusSquare.GetLineValueOf(y);
            if (xColumnValue == yColumnValue)
            {
                int xEncryptedVal = WrapLineValue(xLineValue + direction, direction) * 10 + xColumnValue;
                int yEncryptedVal = WrapLineValue(yLineValue + direction, direction) * 10 + yColumnValue;
                sb.Append(MyPolybiusSquare.GetElementOfValue(xEncryptedVal));
                sb.Append(MyPolybiusSquare.GetElementOfValue(yEncryptedVal));
            }
            else if (xLineValue == yLineValue)
            {
                int xEncryptedVal = xLineValue * 10 + WrapColumnValue(xColumnValue + direction, direction);
                int yEncryptedVal = yLineValue * 10 + WrapColumnValue(yColumnValue + direction, direction);
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

        private int WrapLineValue(int x, int direction)
        {
            if (x > PolybiusSquare.LineCount)
                return 1;
            else if (x < 1)
                return PolybiusSquare.LineCount;
            else return x;
        }

        private int WrapColumnValue(int x, int direction)
        {
            if (x > PolybiusSquare.ColumnCount)
                return 1;
            else if (x < 1)
                return PolybiusSquare.ColumnCount;
            else return x;
        }

        public override string GetKeyValue()
        {
            return Key.StringValue;
        }

        public override string ToString()
        {
            return "Playfair";
        }

    }
}

