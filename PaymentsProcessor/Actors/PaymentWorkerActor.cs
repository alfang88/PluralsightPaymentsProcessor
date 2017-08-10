using System;
using Akka.Actor;
using PaymentsProcessor.ExternalSystems;
using PaymentsProcessor.Messages;

namespace PaymentsProcessor.Actors
{
    internal class PaymentWorkerActor : ReceiveActor, IWithUnboundedStash
    {
        private readonly IPaymentGateway _paymentGateway;
        public IStash Stash { get; set; }
        private ICancelable _unstashSchedule;

        public PaymentWorkerActor(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;

            Receive<SendPaymentMessage>(message => SendPayment(message));
            Receive<ProcessStashedPaymentsMessage>(message => HandleUnstash());
        }

        private void HandleUnstash()
        {
            if (PeakTimeDemoSimulator.IsPeakHours)
                return;
            Console.WriteLine("Not in peak time. Unstashing...");
            Stash.UnstashAll();
        }

        private void SendPayment(SendPaymentMessage message)
        {
            if (message.AmountDecimal > 100 && PeakTimeDemoSimulator.IsPeakHours)
            {
                Console.WriteLine($"Stashing payment message for {message.FirstName} {message.LastName}");

                Stash.Stash();
            }
            else
            {
                Console.WriteLine($"Sending payment for {message.FirstName} {message.LastName}");
                _paymentGateway.Pay(message.AccountNumber, message.AmountDecimal);
                Sender.Tell(new PaymentSentMessage(message.AccountNumber));
            }
        }

        #region Lifecycle hooks

        protected override void PreStart()
        {
            _unstashSchedule = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(1),
                Self,
                new ProcessStashedPaymentsMessage(),
                Self);
        }

        protected override void PostStop()
        {
            _unstashSchedule.Cancel();
        }

        #endregion
    }
}
