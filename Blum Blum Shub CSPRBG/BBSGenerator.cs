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

        private static bool IsPrime(BigInteger x)
        {
            if (x <= 1)
                return false;
            if (x == 2)
                return true;
            for (BigInteger i = 3; i < BigInteger.Divide(x, 2); i = BigInteger.Add(i, 1))
                if (BigInteger.ModPow(x, 1, i) == 0)
                    return false;
            return true;
        }

        private static bool IsBloomPrime(BigInteger x)
        {
            return IsPrime(x) && BigInteger.ModPow(x, 1, 4) == 3;
        }

        private static bool ValidateParameters(BigInteger p, BigInteger q)
        {
            //check if blum primes
            return IsBloomPrime(p) && IsBloomPrime(q);
        }

        public static bool SetParameters(BigInteger p, BigInteger q)
        {
            if (ValidateParameters(p, q) == false)
                return false;
            m = BigInteger.Multiply(p, q);
            BBSGenerator.p = p;
            BBSGenerator.q = q;
            parametersSet = true;
            return true;
        }
        
        private static BigInteger GenerateRandomBigInteger(int length)
        {
            BigInteger x;
            Random random = new Random();
            Byte[] randomBytes = new byte[length];
            random.NextBytes(randomBytes);
            x = new BigInteger(randomBytes);
            return x;
        }

        private static void GenerateSeed()
        {
            previousNumber = GenerateRandomBigInteger(m.ToByteArray().Length - 1);
        }

        private static Byte GetRandomBit()
        {
            previousNumber = BigInteger.ModPow(previousNumber, 2, m);
            return (Byte)BigInteger.ModPow(previousNumber, 1, 2);
        }
    
        private static BigInteger NextBlumPrime(BigInteger x)
        {
            while (true)
            {
                x = BigInteger.Add(x, 1);

                if (IsBloomPrime(x))
                    return x;
            }
        }

        private static void GenerateParameters()
        {
            String baseValue = "100000";
            p = NextBlumPrime(BigInteger.Parse(baseValue));
            q = NextBlumPrime(p);
            m = BigInteger.Multiply(p, q);
        }

        public static Byte[] GetRandomByteArray(long size)
        {
            if (!parametersSet)
                GenerateParameters();

            GenerateSeed();
            StringBuilder bitString = new StringBuilder();
            currentBit = 0;
            Byte[] byteArray = new Byte[size];

            while (currentBit < size)
            {
                Byte myByte=0;
                for (int i = 0; i < 8; i++)
                {
                    myByte += GetRandomBit();
                    myByte <<= 1;
                }
                byteArray[currentBit] = myByte;
                currentBit++;
            }
            return byteArray;
        }
    }
}
