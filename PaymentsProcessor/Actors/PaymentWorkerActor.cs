using Akka.Actor;
using PaymentsProcessor.ExternalSystems;
using PaymentsProcessor.Messages;

namespace PaymentsProcessor.Actors
{
    internal class PaymentWorkerActor : ReceiveActor
    {
        private readonly IPaymentGateway _paymentGateway;

        public PaymentWorkerActor(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;

            Receive<SendPaymentMessage>(message => HandleSendPayment(message));
            Receive<PaymentReceipt>(message => HandlePaymentReceipt(message));
        }

        private void HandlePaymentReceipt(PaymentReceipt message)
        {
            Sender.Tell(new PaymentSentMessage(message.AccountNumber, message.PaymentConfirmationReceipt));
        }

        private void HandleSendPayment(SendPaymentMessage message)
        {
            _paymentGateway.Pay(message.AccountNumber, message.AmountDecimal).PipeTo(Self, Sender);
        }
    }
}
