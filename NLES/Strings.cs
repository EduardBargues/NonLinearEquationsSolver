using System;
using System.Collections.Generic;
using System.Text;

namespace NLES
{
    internal static class Strings
    {
        internal static string MaxNumberOfIterationsReached => "Maximum number of iterations reached.";
        public static string LoadIncrementLargerThanZero => "The load factor increment must be larger than 0.";
        public static string DegreesOfFreedomLargerThanZero => "The system must be at least 1 degree of freedom.";
        public static string ArcLengthRadiusLargerThanZero => "The arc length radius must be larger than 0.";
        public static string WorkControlValueLargerThanZero => "The work control value must be larger than 0.";
        public static string TolerancesLargerThanZero => "Tolerances must be larger than 0.";
        public static string MaximumNumberOfIterationsLargerThanZero =>
            "The maximum number of iterations must be larger than 0.";
    }
}
