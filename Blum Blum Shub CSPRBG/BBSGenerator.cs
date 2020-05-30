using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Blum_Blum_Shub_CSPRBG
{
    class BBSGenerator
    {
        private static BigInteger previousNumber;
        private static BigInteger p;
        private static BigInteger q;
        private static BigInteger m;
        private static bool parametersSet = false;
        private static long currentBit;

        private static bool ValidateParameters(BigInteger p, BigInteger q)
        {
            return true;
        }

        public static bool SetParameters(BigInteger p, BigInteger q)
        {
            if (ValidateParameters(p, q) == false)
                return false;
            m = p * q;
            BBSGenerator.p = p;
            BBSGenerator.q = q;
            Random random = new Random();
            previousNumber = random.Next((int)(m - 1));
            parametersSet = true;

            return true;
        }

        private static char GetRandomBit()
        {
            char randomBit;
            previousNumber = BigInteger.ModPow(previousNumber, 2, m);
            randomBit = (char)BigInteger.ModPow(previousNumber,1,2);
            return randomBit;
        }

        public static String GetRandomBitString(long size)
        {
            StringBuilder bitString = new StringBuilder();
            currentBit = 0;

            while (currentBit < size)
            {
                currentBit++;
                bitString.Append(GetRandomBit());
            }
            //bazinga
            int x;
            return bitString.ToString();
        }
    }
}
