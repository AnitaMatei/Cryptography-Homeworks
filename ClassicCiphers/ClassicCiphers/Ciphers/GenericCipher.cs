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

        /*
         * Checks if the string inputted as key is valid and then creates and returns a cipherkey object. 
         */
        protected abstract CipherKey CheckKeyValidity(String keyString);

        public abstract void SetKey(String keyString);
        public abstract String Encrypt(String text);
        public abstract String Decrypt(String text);
        /*
         * Function that checks the validity of the input text for a particular cipher.
         * For some ciphers like caesar the input text will always be valid, since any non-alphabet character is just kept as-is.
         */
        public virtual bool CheckInputTextValidity(String text, String mode)
        {
            
            return true;
        }
        public abstract String GetKeyValue();
    }
}
