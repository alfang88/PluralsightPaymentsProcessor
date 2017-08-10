using System.Collections.Generic;
using System.IO;
using System.Linq;
using Akka.Actor;
using Akka.DI.Core;
using Akka.Routing;
using PaymentsProcessor.Messages;

namespace PaymentsProcessor.Actors
{
    internal class JobCoordinatorActor : ReceiveActor
    {
        private readonly IActorRef _paymentWorker;
        private int _numberOfRemainingPayments;

        public JobCoordinatorActor()
        {
            _paymentWorker = Context.ActorOf(
                Context.DI().Props<PaymentWorkerActor>().
                WithRouter(FromConfig.Instance), "PaymentWorkers");

            Receive<ProcessFileMessage>(
                message =>
                {
                    StartNewJob(message.FileName);
                });

            Receive<PaymentSentMessage>(
                message =>
                {
                    _numberOfRemainingPayments--;

                    var jobIsComplete = _numberOfRemainingPayments == 0;

                    if (jobIsComplete)
                        Context.System.Terminate();
                });
        }

        private void StartNewJob(string fileName)
        {
            List<SendPaymentMessage> requests = ParseCsvFile(fileName);

            _numberOfRemainingPayments = requests.Count;

            requests.ForEach(sendPaymentMsg => _paymentWorker.Tell(sendPaymentMsg));
        }

        private List<SendPaymentMessage> ParseCsvFile(string fileName)
        {
            var fileLines = File.ReadAllLines(fileName);

            return fileLines.
                Select(fileLine => fileLine.Split(',')).
                Select(values => new SendPaymentMessage(values[0], values[1], int.Parse(values[3]), decimal.Parse(values[2]))).
                ToList();
        }
    }
}
