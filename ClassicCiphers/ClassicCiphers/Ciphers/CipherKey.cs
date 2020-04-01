using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicCiphers.Ciphers
{
    public class CipherKey
    {
        public int NumericalValue { get; set; }
        public String StringValue { get; set; }

        public void SetStringValue(String stringValue)
        {
            StringValue = stringValue;
        }

        public void SetNumericalValue(int numericalValue)
        {
            NumericalValue = numericalValue;
            StringValue = numericalValue.ToString();
        }

        public override String ToString()
        {
            return StringValue;
        }
    }
}
