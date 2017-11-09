# NonLinearEquationsSolver

Library to provide and easy-to-use interface to solve non-linear systems of equations using variants of the Newton-Raphson method.

## Examples

### Example 1

The library can solve any thing that inherits from the interface IFunction. This interface forces you to implement two methods: GetTangentMatrix() and GetImage(). You can see them in the following brief example:
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
As you can see, the GetTangentMatrix() method provides the Jacobian o tangent matrix of your system that the Solver class will use to iterate with the Newton-Raphson method.
Now, you need to define the problem to be solve as in the next part of code:
```c#
public void TestMethod1()
{
    IFunction function = new Function1();
    ISolver solver = new Solver();
    Vector<double> force = DenseVector.Create(2, 1);
    ProblemDefinition input = new ProblemDefinition
    {
        Force = force,
        Function = function,
        FirstLambdaValue = 1,
        LastLambdaValue = 1,
        MaxIncrements = 10,
        MaxIterations = 10,
        Tolerances = new ErrorTolerancesInfo(1e-3, 1e-3, 1e-3, 1e-3),
        Beta = 1,
        InitialApproximation = DenseVector.Create(2, 0.1),
    };
    SolverReport report = solver.Solve(input);

    Vector<double> realSolution = DenseVector.Create(2, 1);
    Assert.IsTrue(report.Convergence && (report.Solution - realSolution).Norm(2) <= 1e-3);
}
```
As you can see, the TestMethod1() does the following:
- Initialize the function to be solved as an IFunction interface (here you can use where system you are willing to study, a finite element model for example).
- Initialize a solver class.
- Defines the ProblemDefinition class, where we define the problem and the parameters for our solver.
- Finally the solver uses the method .Solve() to provide with a SolverReport class where inside there is the solution in case of convergence together with some extra information about the iterations performed.

### Example 2

```c#
public class Function2 : IFunction
{
    public Matrix<double> GetTangentMatrix(Vector<double> u)
    {
        return new DenseMatrix(2, 2)
        {
            [0, 0] = 2 * u[0],
            [1, 1] = 2 * u[1]
        };
    }

    public Vector<double> GetImage(Vector<double> u)
    {
        return new DenseVector(2)
        {
            [0] = Math.Pow(u[0], 2),
            [1] = Math.Pow(u[1], 2)
        };
    }
}
public void TestMethod2()
{
    IFunction function = new Function2();
    ISolver solver = new Solver();
    Vector<double> force = DenseVector.Create(2, 1);
    ProblemDefinition input = new ProblemDefinition
    {
        Force = force,
        Function = function,
        FirstLambdaValue = 1,
        LastLambdaValue = 1,
        MaxIncrements = 10,
        MaxIterations = 10,
        Tolerances = new ErrorTolerancesInfo(1e-3, 1e-3, 1e-3, 1e-3),
        Beta = 1,
        InitialApproximation = DenseVector.Create(2, 0.1)
    };
    SolverReport report = solver.Solve(input);
    Vector<double> realSolution = DenseVector.Create(2, 1);
    Assert.IsTrue(report.Convergence && (report.Solution - realSolution).Norm(2) <= 1e-3);
}
```

### Example 3

```c#
public class Function3 : IFunction
{
    public Matrix<double> GetTangentMatrix(Vector<double> u)
    {
        double x = u[0];
        double y = u[1];
        return new DenseMatrix(2, 2)
        {
            [0, 0] = 2 * x,
            [0, 1] = 1,
            [1, 0] = 1,
            [1, 1] = 2 * y,
        };
    }

    public Vector<double> GetImage(Vector<double> u)
    {
        double x = u[0];
        double y = u[1];
        return new DenseVector(2)
        {
            [0] = Math.Pow(x, 2) + y,
            [1] = Math.Pow(y, 2) + y
        };
    }
}
public void TestMethod3()
{
    IFunction function = new Function3();
    ISolver solver = new Solver();
    Vector<double> force = DenseVector.Create(2, 2);
    ProblemDefinition input = new ProblemDefinition
    {
        Force = force,
        Function = function,
        FirstLambdaValue = 1,
        LastLambdaValue = 1,
        MaxIncrements = 10,
        MaxIterations = 10,
        Tolerances = new ErrorTolerancesInfo(1e-3, 1e-3, 1e-3, 1e-3),
        Beta = 1,
        InitialApproximation = DenseVector.Create(2, 0.1)
    };
    SolverReport report = solver.Solve(input);
    Vector<double> realSolution = DenseVector.Create(2, 1);
    Assert.IsTrue(report.Convergence && (report.Solution - realSolution).Norm(2) <= 1e-3);
}
```
        

### Example 4

```c#
public class Function4 : IFunction
{
    public Matrix<double> GetTangentMatrix(Vector<double> u)
    {
        double x = u[0];
        double y = u[1];
        return new DenseMatrix(2, 2)
        {
            [0, 0] = 2 * x + y,
            [0, 1] = x,
            [1, 0] = 1,
            [1, 1] = 1,
        };
    }

    public Vector<double> GetImage(Vector<double> u)
    {
        double x = u[0];
        double y = u[1];
        return new DenseVector(2)
        {
            [0] = Math.Pow(x, 2) + y * x,
            [1] = x + y
        };
    }
}
public void TestMethod4()
{
    IFunction function = new Function4();
    ISolver solver = new Solver();
    Vector<double> force = DenseVector.Create(2, 2);
    ProblemDefinition input = new ProblemDefinition
    {
        Force = force,
        Function = function,
        FirstLambdaValue = 1,
        LastLambdaValue = 1,
        MaxIncrements = 10,
        MaxIterations = 10,
        Tolerances = new ErrorTolerancesInfo(1e-3, 1e-3, 1e-3, 1e-3),
        Beta = 1,
        InitialApproximation = DenseVector.Create(2, 0.1)
    };
    SolverReport report = solver.Solve(input);
    Vector<double> realSolution = DenseVector.Create(2, 1);
    Assert.IsTrue(report.Convergence && (report.Solution - realSolution).Norm(2) <= 1e-3);
}
```
### Example 5

```c#
public class Function4 : IFunction
{
    public Matrix<double> GetTangentMatrix(Vector<double> u)
    {
        double x = u[0];
        double y = u[1];
        return new DenseMatrix(2, 2)
        {
            [0, 0] = 2 * x + y,
            [0, 1] = x,
            [1, 0] = 1,
            [1, 1] = 1,
        };
    }

    public Vector<double> GetImage(Vector<double> u)
    {
        double x = u[0];
        double y = u[1];
        return new DenseVector(2)
        {
            [0] = Math.Pow(x, 2) + y * x,
            [1] = x + y
        };
    }
}
public void TestMethod5()
{
    IFunction function = new Function4();
    ISolver solver = new Solver();
    Vector<double> force = DenseVector.Create(2, 2);
    ProblemDefinition input = new ProblemDefinition
    {
        Force = force,
        Function = function,
        FirstLambdaValue = 0.1,
        LastLambdaValue = 1,
        MaxIncrements = 10,
        MaxIterations = 10,
        Tolerances = new ErrorTolerancesInfo(1e-3, 1e-3, 1e-3, 1e-3),
        Beta = 1,
        InitialApproximation = DenseVector.Create(2, 0.1)
    };
    SolverReport report = solver.Solve(input);
    Vector<double> realSolution = DenseVector.Create(2, 1);
    Assert.IsTrue(report.Convergence && (report.Solution - realSolution).Norm(2) <= 1e-3);
}
```
