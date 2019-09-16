namespace ModelModul.Models
{
    public class CounterpartyType
    {
        public ETypeCounterparties Id { get; set; }

        public string Description { get; set; }

        public CounterpartyType() { }

        public CounterpartyType(ETypeCounterparties type, string description)
        {
            Id = type;
            Description = description;
        }
    }
}
