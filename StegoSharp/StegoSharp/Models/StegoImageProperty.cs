namespace StegoSharp.Models
{

    using StegoSharp.Enums;
    using System.Drawing.Imaging;

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
