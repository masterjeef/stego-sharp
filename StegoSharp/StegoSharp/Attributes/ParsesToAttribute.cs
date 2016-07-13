using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegoSharp.Attributes
{
    public class ParsesToAttribute : Attribute
    {

        public Type Type { get; set; }

        public ParsesToAttribute(Type type)
        {
            Type = type;
        }

    }
}
