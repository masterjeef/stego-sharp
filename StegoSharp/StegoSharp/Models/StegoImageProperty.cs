using System.Drawing.Imaging;
using StegoSharp.Enums;

namespace StegoSharp.Models
{

    public class StegoImageProperty
    {

        public PropertyItem PropertyItem { get; set; }

        public PropertyItemType PropertyItemType
        {
            get
            {
                return (PropertyItemType) PropertyItem.Type;
            }
        }

        public byte[] Data
        {
            get
            {
                return PropertyItem.Value;
            }
        }

        public StegoImageProperty(PropertyItem propertyItem)
        {
            PropertyItem = propertyItem;
        }
    }
}
