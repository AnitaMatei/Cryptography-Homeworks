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
        public String Key { get; set; }

        public PolybiusSquare()
        {
            Checkerboard = new Dictionary<char, int>();
        }

        public void CreateCheckerboard(String key)
        {
            Key = key;
            int position = 11;
            char element;
            for (int i = 0; i < key.Length; i++)
            {
                element = key[i];
                if (Checkerboard.ContainsKey(element))
                    continue;

                AddToCheckerboard(element, position);
                AdvancePosition(ref position);
            }

            for (int i = 0; i < 25; i++)
            {
                element = (char)('a' + i);
                if (Checkerboard.ContainsKey(element))
                    continue;

                AddToCheckerboard(element, position);
                AdvancePosition(ref position);
            }
        }

        private void AddToCheckerboard(char element, int position)
        {
            Checkerboard.Add(element, position);
            if (element == 'i')
                Checkerboard.Add('j', position);
            else if (element == 'j')
                Checkerboard.Add('i', position);

        }
        private void AdvancePosition(ref int position)
        {
            if (position % 10 != 0 && position % 5 == 0)
                position += 6;
            else position++;
        }
        public int GetValueOf(char element)
        {
            return Checkerboard[element];
        }

        public char GetElementOfValue(int value)
        {
            return Checkerboard.FirstOrDefault(x => x.Value == value).Key;
        }
    }

}

