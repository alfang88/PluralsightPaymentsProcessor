namespace PaymentsProcessor.Messages
{
    internal class PaymentSentMessage
    {
        public int AccountNumer { get; private set; }
        public string ReceiptReference { get; private set; }

        public PaymentSentMessage(int accountNumber, string receiptReference)
        {
            AccountNumer = accountNumber;
            ReceiptReference = receiptReference;
        }
    }
}
