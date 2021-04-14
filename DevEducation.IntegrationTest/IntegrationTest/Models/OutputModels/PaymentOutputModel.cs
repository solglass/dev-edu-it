namespace IntegrationTest.Models.OutputModels
{
    public class PaymentOutputModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Date { get; set; }
        public string Period { get; set; }
        public int ContractNumber { get; set; }        
        public AuthorOutputModel User { get; set; }

    }
}
