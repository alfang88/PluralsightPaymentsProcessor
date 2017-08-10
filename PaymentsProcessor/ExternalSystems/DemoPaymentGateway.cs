using System;
using System.Threading;

namespace PaymentsProcessor.ExternalSystems
{
    class DemoPaymentGateway : IPaymentGateway
    {
        public void Pay(int accountNumber, decimal amount)
        {
            if (PeakTimeDemoSimulator.IsPeakHours && amount > 100)
            {
                Console.WriteLine(
                    $"Account number {accountNumber} payment takes longer because is peak & > 100");

                Thread.Sleep(2000);
            }
            else
            {
                // Simulate communicating with external payment gateway
                Thread.Sleep(200);
            }
        }
    }
}
