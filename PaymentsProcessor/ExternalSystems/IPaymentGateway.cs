using System.Threading.Tasks;

namespace PaymentsProcessor.ExternalSystems
{
    interface IPaymentGateway
    {
        Task<PaymentReceipt> Pay(int accountNumber, decimal amount);
    }
}
