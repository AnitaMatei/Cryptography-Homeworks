using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicCiphers.Ciphers
{
    public abstract class GenericCipher : ICipher
    {
        protected CipherKey Key;

        protected abstract CipherKey CheckKeyValidity(String keyString);

        public abstract void SetKey(String keyString);
        public abstract String Encrypt(String text);
        public abstract String Decrypt(String text);
        public abstract String GetKeyValue();
    }
}
