using System;

namespace PaymentsProcessor
{
    static class PeakTimeDemoSimulator
    {
        private static DateTime _demoStartTime;
        private static int _stayPeakTimeForSeconds;

        public static bool IsPeakHours
        {
            get
            {
                // Simulate being in peak time for a number of seconds after starting the demo
                var elapsedTimeSinceStartingDemo = DateTime.Now.Subtract(_demoStartTime).
                    TotalSeconds;

                return elapsedTimeSinceStartingDemo < _stayPeakTimeForSeconds;
            }
        }

        public static void StartDemo(int stayPeakTimeForSeconds)
        {
            _demoStartTime = DateTime.Now;
            _stayPeakTimeForSeconds = stayPeakTimeForSeconds;
        }
    }
}
