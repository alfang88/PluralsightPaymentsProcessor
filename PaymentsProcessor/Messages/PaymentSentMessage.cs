namespace PaymentsProcessor.Messages
{
    internal class PaymentSentMessage
    {
        public int AccountNumer { get; private set; }

        public PaymentSentMessage(int accountNumber)
        {
            AccountNumer = accountNumber;
        }
    }
}
