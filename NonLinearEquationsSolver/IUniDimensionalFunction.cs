namespace NonLinearEquationsSolver
{
    public interface IUniDimensionalFunction
    {
        double GetImage(double eps);
        double GetTangent(double eps);
    }
}
