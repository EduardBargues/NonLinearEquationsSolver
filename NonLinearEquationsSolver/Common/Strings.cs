namespace NonLinearEquationsSolver.Common {
    internal class Strings {
        internal static string ReferenceLoadMustBeDefined = "Reference load must be defined.";
        internal static string ReactionMustBeDefined = "Reaction must be defined";
        internal static string StiffnessLoadMustBeDefined = "Stiffness must be defined";
        public static string SchemeMustBeDefined { get; set; } = "Iteration scheme must be defined";
        public static string ArcLengthRadiusMustBeDefined { get; set; } = "Arc length radius must be defined";
        public static string DisplacementChooserMustBeDefined { get; set; } = "Arc length displacement picker must be defined";
        public static string WorkIncrementMustBeDefined { get; set; } = "Work control value must be defined";
        public static string DefaultLoadFactorIncrementMustBeDefined { get; set; } = "Default load factor increment must be defined";
    }
}