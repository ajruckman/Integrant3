using System;

namespace Integrant.Element
{
    public class BitUsage : Attribute
    {
        public BitUsage(params string[] permitted)
        {
            Permitted = permitted;
        }

        public string[] Permitted { get; }
    }
}