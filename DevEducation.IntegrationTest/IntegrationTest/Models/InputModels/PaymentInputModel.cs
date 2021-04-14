namespace IntegrationTest.Models.InputModels

{
    public class PaymentInputModel
    {
        public decimal Amount { get; set; }
        public string Date { get; set; }
        public string Period { get; set; }
        public int ContractNumber { get; set; }

    }
}
