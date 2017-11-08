namespace NonLinearEquationsSolver
{
    public enum FailReason
    {
        None,
        MaxIncrementsReached,
        IncrementLoadFailed,
        MaxIterationsReached,
        CorrectionPhaseFailed,
        ArcLengthTooLarge,
    }
}