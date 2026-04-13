namespace DynamicPricing.API.DTOs
{
    public class PriceRuleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RuleType { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public decimal Multiplier { get; set; }
        public string Description { get; set; }
    }

    public class CreatePriceRuleDto
    {
        public string Name { get; set; }
        public string RuleType { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public decimal Multiplier { get; set; }
        public string Description { get; set; }
    }
}