using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicCiphers.Ciphers
{
    class NihilistCipher : GenericCipher
    {
        public static String DefaultKeyString = "zebras russian";

        PolybiusSquare MyPolybiusSquare;

        public NihilistCipher()
        {
            MyPolybiusSquare = new PolybiusSquare();
        }

        protected override CipherKey CheckKeyValidity(String key)
        {
            CipherKey cipherKey = new CipherKey();
            String polybiusSquareKey, encryptionKey;
            String[] temp = key.Split(' ');
            if (temp.Length != 2)
                throw new FormatException("The key introduced for the nihilist cipher doesn't respect the input format!");
            polybiusSquareKey = temp[0];
            encryptionKey = temp[1];

            for (int i = 0; i < polybiusSquareKey.Length; i++)
            {
                if (polybiusSquareKey[i] < 'a' || polybiusSquareKey[i] > 'z')
                    throw new FormatException("The key introduced for the nihilist cipher for the polybius square contains more than alphabet letters!");
            }

            for (int i = 0; i < encryptionKey.Length; i++)
            {
                if (encryptionKey[i] < 'a' || encryptionKey[i] > 'z')
                    throw new FormatException("The key introduced for the nihilist cipher for encryption contains more than alphabet letters!");
            }

            cipherKey.SetStringValue(encryptionKey);
            return cipherKey;

        }


        public override void SetKey(String key)
        {
            this.Key = CheckKeyValidity(key);
            MyPolybiusSquare.CreateCheckerboard(key.Split(' ')[0]);
        }

        public override String Encrypt(String text)
        {
            StringBuilder sb = new StringBuilder();
            int progress = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (!MyPolybiusSquare.ContainsCharacter(text[i]))
                {
                    continue;
                }
                char currentEncryptionKeyChar = Key.StringValue[progress % Key.StringValue.Length],
                    currentClearTextChar = text[i];

                sb.Append(MyPolybiusSquare.GetValueOf(currentClearTextChar) + MyPolybiusSquare.GetValueOf(currentEncryptionKeyChar));
                if (i < text.Length - 1)
                    sb.Append(" ");
                progress++;
            }
            return sb.ToString();
        }
        public override String Decrypt(String text)
        {
            StringBuilder sb = new StringBuilder();
            String[] numbers = text.Split(' ');
            int progress=0;
            for (int i = 0; i < numbers.Length; i++)
            {
                if (!int.TryParse(numbers[i], out int currentNumber))
                    continue;
                char currentEncryptionKeyChar = Key.StringValue[progress % Key.StringValue.Length];
                sb.Append(MyPolybiusSquare.GetElementOfValue(currentNumber - MyPolybiusSquare.GetValueOf(currentEncryptionKeyChar)));
                progress++;
            }
            return sb.ToString();

        }

        
        public override string GetKeyValue()
        {
            return MyPolybiusSquare.Key + " " + Key.StringValue;
        }
    }
}
