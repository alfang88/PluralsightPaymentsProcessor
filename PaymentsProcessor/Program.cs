using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using PaymentsProcessor.Actors;
using PaymentsProcessor.ExternalSystems;
using PaymentsProcessor.Messages;

namespace PaymentsProcessor
{
    class Program
    {
        private static ActorSystem ActorSystem;

        static void Main(string[] args)
        {
            AsyncMain().Wait();
        }

        private static async Task AsyncMain()
        {
            CreateActorSystem();

            IActorRef jobCoordinator = ActorSystem.ActorOf<JobCoordinatorActor>("JobCoordinator");

            var jobTime = Stopwatch.StartNew();

            jobCoordinator.Tell(new ProcessFileMessage("file1.csv"));

            await ActorSystem.WhenTerminated;

            jobTime.Stop();

            Console.WriteLine($"Job completed in {jobTime.ElapsedMilliseconds}ms");
            Console.ReadLine();
        }

        private static void CreateActorSystem()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DemoPaymentGateway>().As<IPaymentGateway>();
            builder.RegisterType<PaymentWorkerActor>();
            var container = builder.Build();

            ActorSystem = Akka.Actor.ActorSystem.Create("PaymentProcessing");

            IDependencyResolver resolver = new AutoFacDependencyResolver(container, ActorSystem);
        }
    }
}
