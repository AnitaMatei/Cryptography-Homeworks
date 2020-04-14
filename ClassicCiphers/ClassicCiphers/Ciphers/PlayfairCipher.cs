using System;
using System.Text;
using ClassicCiphers.Exceptions;

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

        /*
         * Checks if the key string contains characters contained by the polybius square.
         */
        protected override CipherKey CheckKeyValidity(String key)
        {
            CipherKey cipherKey = new CipherKey();
            for (int i = 0; i < key.Length; i++)
            {
                if (!MyPolybiusSquare.ContainsCharacter(key[i]))
                    throw new InvalidKeyFormatException("The key introduced for the playfair cipher contains more than the permitted characters!");
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
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i += 2)
            {
                sb.Append(EncryptCharacterPair(text[i], text[i + 1]));
            }

            return sb.ToString();
        }
        public override String Decrypt(String text)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < text.Length; i += 2)
            {
                sb.Append(DecryptCharacterPair(text[i], text[i + 1]));
            }

            return sb.ToString();
        }

        private StringBuilder EncryptCharacterPair(char x, char y)
        {
            return TransformCharacterPair(x, y, 1);
        }
        private StringBuilder DecryptCharacterPair(char x, char y)
        {
            return TransformCharacterPair(x, y, -1);
        }

        /*
         * Transforms a pair of characters based on the rules of the playfair cipher. 
         * The direction argument represents wether the pair is being encrypted or decrypted.
         */ 
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
        /*
         * In order for playfair to play nice with other ciphers, the number of characters of any text provided needs to be even.
         * If the text contains a character not contained by the polybius square, the text is invalid.
         */
        public override bool CheckInputTextValidity(String text, String mode)
        {
            if (text.Length % 2 == 1)
                return false;
            for (int i = 0; i < text.Length; i++)
                if (!MyPolybiusSquare.ContainsCharacter(text[i]))
                    return false;
            return true;
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

