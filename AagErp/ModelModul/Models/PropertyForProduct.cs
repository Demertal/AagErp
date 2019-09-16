namespace ModelModul.Models
{
    public class PropertyForProduct
    {
        public int PropertyNameId { get; set; }

        public int PropertyValueId { get; set; }

        public PropertyName PropertyName { get; set; }

        public PropertyValue PropertyValue { get; set; }
    }
}
