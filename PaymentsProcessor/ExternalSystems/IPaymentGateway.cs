namespace PaymentsProcessor.ExternalSystems
{
    interface IPaymentGateway
    {
        void Pay(int accountNumber, decimal amount);
    }
}
