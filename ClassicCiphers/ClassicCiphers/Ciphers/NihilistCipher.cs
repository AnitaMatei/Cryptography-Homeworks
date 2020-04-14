using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassicCiphers.Exceptions;

namespace ClassicCiphers.Ciphers
{
    class NihilistCipher : GenericCipher
    {
        public static String DefaultKeyString = "zebras~russian";

        PolybiusSquare MyPolybiusSquare;

        public NihilistCipher()
        {
            MyPolybiusSquare = new PolybiusSquare();
        }

        /*
         * The key for nihilist cipher has 2 components: the key for the polybius square, and the key for encryption, split by the character ~.
         * If either one has characters not contained by the polybius square, the string is invalid.
         */ 
        protected override CipherKey CheckKeyValidity(String key)
        {
            CipherKey cipherKey = new CipherKey();
            String polybiusSquareKey, encryptionKey;
            String[] temp = key.Split('~');
            if (temp.Length != 2)
                throw new InvalidKeyFormatException("The key introduced for the nihilist cipher doesn't respect the input format!");
            polybiusSquareKey = temp[0];
            encryptionKey = temp[1];

            for (int i = 0; i < polybiusSquareKey.Length; i++)
            {
                if (!MyPolybiusSquare.ContainsCharacter(polybiusSquareKey[i]))
                    throw new InvalidKeyFormatException("The key introduced for the nihilist cipher for the polybius square contains more than the permitted characters!");
            }

            for (int i = 0; i < encryptionKey.Length; i++)
            {
                if (!MyPolybiusSquare.ContainsCharacter(encryptionKey[i]))
                    throw new InvalidKeyFormatException("The key introduced for the nihilist cipher for encryption contains more than alphabet letters!");
            }

            cipherKey.SetStringValue(encryptionKey);
            return cipherKey;

        }


        public override void SetKey(String key)
        {
            this.Key = CheckKeyValidity(key);
            MyPolybiusSquare.CreateCheckerboard(key.Split('~')[0]);
        }

        /*
         * In a loop, it takes the current character of the text and the current character of the key (which loops along the length of the text)
         * and adds their values in the polybius square.
         */ 
        public override String Encrypt(String text)
        {
            StringBuilder sb = new StringBuilder();
            int progress = 0;
            for (int i = 0; i < text.Length; i++)
            {
                char currentEncryptionKeyChar = Key.StringValue[progress % Key.StringValue.Length],
                    currentClearTextChar = text[i];

                sb.Append(MyPolybiusSquare.GetValueOf(currentClearTextChar) + MyPolybiusSquare.GetValueOf(currentEncryptionKeyChar));
                if (i < text.Length - 1)
                    sb.Append(" ");
                progress++;
            }
            return sb.ToString();
        }
        /*
         * It splits the text into 2 digit numbers, and from each one the value of the current encryption character will be deducted.
         */ 
        public override String Decrypt(String text)
        {
            StringBuilder sb = new StringBuilder();
            String[] numbers = text.Split(' ');
            int progress = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                int.TryParse(numbers[i], out int currentNumber);
                char currentEncryptionKeyChar = Key.StringValue[progress % Key.StringValue.Length];
                sb.Append(MyPolybiusSquare.GetElementOfValue(currentNumber - MyPolybiusSquare.GetValueOf(currentEncryptionKeyChar)));
                progress++;
            }
            return sb.ToString();

        }
        /*
         * When encrypting, the input text has to contain only characters supported by the polybius square.
         * When decrypting, the input text has to only contain numbers split by a space character.
         */ 
        public override bool CheckInputTextValidity(String text, String mode)
        {
            if (mode.Equals("encrypt"))
            {
                for (int i = 0; i < text.Length; i++)
                    if (!MyPolybiusSquare.ContainsCharacter(text[i]))
                        return false;
            }
            else if (mode.Equals("decrypt"))
            {
                String[] numbers = text.Split(' ');
                for (int i = 0; i < numbers.Length; i++)
                    if (!int.TryParse(numbers[i], out _))
                        return false;
            }

            return true;
        }

        public override string GetKeyValue()
        {
            return MyPolybiusSquare.Key + "~" + Key.StringValue;
        }
        public override string ToString()
        {
            return "Nihilist";
        }
    }
}
