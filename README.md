# NonLinearEquationsSolver
Library to provide and easy-to-use interface to solve non-linear systems of equations using variants of the Newton-Raphson method.

## Examples

### Example 1

The library can solve any thing that inherits from the interface ```c# IFunction```
```c#
public class Function1 : IFunction
{
    public Matrix<double> GetTangentMatrix(Vector<double> u)
    {
        return new DenseMatrix(2, 2)
        {
            [0, 0] = 1,
            [1, 1] = 1
        };
    }

    public Vector<double> GetImage(Vector<double> u)
    {
        return new DenseVector(2)
        {
            [0] = u[0],
            [1] = u[1]
        };
    }
}
```
