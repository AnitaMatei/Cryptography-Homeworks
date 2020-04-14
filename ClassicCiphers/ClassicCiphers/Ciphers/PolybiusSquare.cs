using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicCiphers.Ciphers
{
    class PolybiusSquare
    {
        Dictionary<char, int> Checkerboard;
        static char[] SpecialCharacters = new char[28] { ' ', '!', '"', '#', '$','%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', ':', ';', '<',
            '=','>','?','@','[','\\',']','^','_' };
        static char[] NumberCharacters = new char[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        public static int ColumnCount = 8;
        public static int LineCount = 8;
        public String Key { get; set; }

        public PolybiusSquare()
        {
            Checkerboard = new Dictionary<char, int>();
        }

        /*
         * Generates an extended version of the polybius square, where first comes the key, then
         * the alphabet, then special characters and then the numbers
         */
        public void CreateCheckerboard(String key)
        {
            Key = key;
            int position = 11;
            char element;
            for (int i = 0; i < key.Length; i++)
            {
                element = key[i];
                AddToCheckerboard(element, ref position);
            }
            for (int i = 0; i < 26; i++)
            {
                element = (char)('a' + i);
                AddToCheckerboard(element, ref position);
            }
            for (int i = 0; i < NumberCharacters.Length; i++)
            {
                element = NumberCharacters[i];
                AddToCheckerboard(element, ref position);
            }
            for (int i = 0; i < SpecialCharacters.Length; i++)
            {
                element = SpecialCharacters[i];
                AddToCheckerboard(element, ref position);
            }
        }

        /*
         * It checks if the character is already in the map, if it isn't it adds it and advances the position in the square 
         */
        private void AddToCheckerboard(char element, ref int position)
        {
            if (Checkerboard.ContainsKey(element))
                return;
            Checkerboard.Add(element, position);
            AdvancePosition(ref position);

        }

        /*
         * Advances the position by one and projects it onto the shape of the polybius square.
         */
        private void AdvancePosition(ref int position)
        {
            if (position % 10 != 0 && (position % 10) % ColumnCount == 0)
                position += 11 - ColumnCount;
            else position++;
        }

        public bool ContainsCharacter(char character)
        {
            foreach (char tempChar in SpecialCharacters)
            {
                if (character == tempChar)
                    return true;
            }
            foreach (char tempChar in NumberCharacters)
            {
                if (character == tempChar)
                    return true;
            }
            if (character >= 'a' && character <= 'z')
                return true;
            return false;
        }

        public int GetValueOf(char element)
        {
            return Checkerboard[element];
        }

        public int GetLineValueOf(char element)
        {
            return GetValueOf(element) / 10;
        }
        public int GetColumnValueOf(char element)
        {
            return GetValueOf(element) % 10;
        }

        public char GetElementOfValue(int value)
        {
            return Checkerboard.FirstOrDefault(x => x.Value == value).Key;
        }
    }

}

