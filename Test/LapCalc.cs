namespace LapCalculations{
    internal static class LapCalc{
        internal static Boolean LapCompleted(ref short currentLap, ref short previousLap)
        {
            if ((currentLap > 0) && (currentLap > previousLap))
            {
                return true;
            }
            return false;
        }

        internal static void RecordDistance(float MetersPerSecond, TimeSpan ElapsedTime)
        {


        }
    }
}