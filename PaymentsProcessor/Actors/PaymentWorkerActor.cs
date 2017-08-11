using System;
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

            Receive<SendPaymentMessage>(message => SendPayment(message));
        }

        private async void SendPayment(SendPaymentMessage message)
        {
            var sender = Sender;

            var result = await _paymentGateway.Pay(message.AccountNumber, message.AmountDecimal);

            sender.Tell(new PaymentSentMessage(result.AccountNumber, result.PaymentConfirmationReceipt));
        }
    }
}
