# Non-Linear Equations Solver

Library to provide and easy-to-use interface to solve non-linear systems of equations using variants of the Newton-Raphson method.

# Table of contents
- [Description](#description)
- [Tests](#tests)

## Description <a name="description"></a>

To use this library you esentially need 2 things: 
* Function to be solved (defined by a ``` Func<Vector<double>, Vector<double>> ```).
* Stiffness, also called derivative (defined by ``` Func<Vector<double>, Matrix<double>> ```).
Once you have both, you need to create a solver using the builder I provide (check the tests to see how it works). Basically, the builder allows you to define all the aspects of your solver. The aspects are:
* ``` Solver.Builder.Solve( int degreesOfFreedom, Func<Vector<double>, Vector<double>> structure, Func<Vector<double>, Matrix<double>> stiffness ) ``` : Allows the definition of the problem to solve.
* ``` Solver.Builder.Under( Func<Vector<double>, Vector<double>> referenceLoad) ``` : Defines the external load to equilibrate.
* ``` Solver.Builder.UntilTolerancesReached( double displacement, double equilibrium, double energy ) ``` : Define tolerances to satisfy.
* ``` Solver.Builder.WithMaximumCorrectionIterations( int maximumCorrectionIterations ) ``` : Limits the number of corrections on each iteration.
* ``` Solver.Builder.UsingStandardNewtonRaphsonScheme( double loadFactorIncrement ) ``` : Defines the iteration scheme as standard Newton-Raphson.
* ``` Solver.Builder.UsingWorkControlScheme( double work ) ``` : Defines the iteration scheme as a variation of the Newton-Raphson called work-control scheme.
* ``` Solver.Builder.UsingArcLengthScheme( double radius ) ``` : Defines the iteration scheme as a variation of the Newton-Raphson called Arc-Length scheme.
    * ``` .NormalizeLoadWith( double beta ) ``` : Normalize external load with a factor in case you are working with different units.
    * ``` .WithRestoringMethodInCorrectionPhase() ``` : Defines the correction scheme as Restoring method.
    * ``` .WithAngleMethodInCorrectionPhase() ``` : Defines the correction scheme as Angle method.

## Tests <a name="tests"></a>

```c#
[Test]
public void SolveLinearFunction() {
	Vector<double> Function( Vector<double> u ) {
	    return new DenseVector ( 2 ) { [0] = u[0], [1] = u[0] + 2 * u[1] };
	}
	Matrix<double> Stiffness( Vector<double> u ) {
	    return new DenseMatrix ( 2, 2 ) { [0, 0] = 1, [1, 0] = 1, [1, 1] = 2 };
	}
	DenseVector force = new DenseVector ( 2 ) { [0] = 1, [1] = 3 };
	Solver solver = Solver.Builder
	    .Solve ( 2, Function, Stiffness )
	    .Under ( force )
	    .Build ( );
	List<LoadState> states = solver.Broadcast ( ).Take ( 2 ).ToList ( );
	AssertSolutionsAreCorrect ( Function, force, states );
}

[Test]
public void SolveLinearFunctionSmallIncrements() {
	Vector<double> Function( Vector<double> u ) {
	    return new DenseVector ( 2 ) { [0] = u[0], [1] = u[0] + 2 * u[1] };
	}
	Matrix<double> Stiffness( Vector<double> u ) {
	    return new DenseMatrix ( 2, 2 ) { [0, 0] = 1, [1, 0] = 1, [1, 1] = 2 };
	}
	DenseVector force = new DenseVector ( 2 ) { [0] = 1, [1] = 3 };
	double inc = 1e-2;
	Solver solver = Solver.Builder
	    .Solve ( 2, Function, Stiffness )
	    .Under ( force )
	    .UsingStandardNewtonRaphsonScheme ( inc )
	    .Build ( );
	List<LoadState> states = solver.Broadcast ( ).TakeWhile ( s => s.Lambda <= 1 ).ToList ( );
	Assert.AreEqual ( (int)(1 / inc) - 1, states.Count );
	AssertSolutionsAreCorrect ( Function, force, states );
}

[Test]
public void SolveLinearFunctionArcLength() {
	Vector<double> Function( Vector<double> u ) {
	    return new DenseVector ( 2 ) { [0] = u[0], [1] = u[0] + 2 * u[1] };
	}
	Matrix<double> Stiffness( Vector<double> u ) {
	    return new DenseMatrix ( 2, 2 ) { [0, 0] = 1, [1, 0] = 1, [1, 1] = 2 };
	}
	DenseVector force = new DenseVector ( 2 ) { [0] = 1, [1] = 3 };
	double radius = 1e-2;
	Solver solver = Solver.Builder
	    .Solve ( 2, Function, Stiffness )
	    .Under ( force )
	    .UsingArcLengthScheme ( radius )
	    .Build ( );
	List<LoadState> states = solver.Broadcast ( ).TakeWhile ( s => s.Lambda <= 1 ).ToList ( );
	AssertSolutionsAreCorrect ( Function, force, states );
}

[Test]
public void SolveQuadraticFunction() {
	Vector<double> Function( Vector<double> u ) {
	    return new DenseVector ( 2 ) {
	        [0] = u[0] * u[0] + 2 * u[1] * u[1],
	        [1] = 2 * u[0] * u[0] + u[1] * u[1]
	    };
	}
	Matrix<double> Stiffness( Vector<double> u ) {
	    return new DenseMatrix ( 2, 2 ) {
	        [0, 0] = 2 * u[0],
	        [0, 1] = 4 * u[1],
	        [1, 0] = 4 * u[0],
	        [1, 1] = 2 * u[1]
	    };
	}
	DenseVector force = new DenseVector ( 2 ) { [0] = 3, [1] = 3 };
	Solver solver = Solver.Builder
	    .Solve ( 2, Function, Stiffness )
	    .Under ( force )
	    .WithInitialConditions ( 0.1, DenseVector.Create ( 2, 0 ), DenseVector.Create ( 2, 1 ) )
	    .Build ( );
	List<LoadState> states = solver.Broadcast ( ).TakeWhile ( x => x.Lambda <= 1 ).ToList ( );
	AssertSolutionsAreCorrect ( Function, force, states );
}

[Test]
public void SolveQuadraticFunctionSmallIncrements() {
	Vector<double> Function( Vector<double> u ) {
	    return new DenseVector ( 2 ) {
	        [0] = u[0] * u[0] + 2 * u[1] * u[1],
	        [1] = 2 * u[0] * u[0] + u[1] * u[1]
	    };
	}
	Matrix<double> Stiffness( Vector<double> u ) {
	    return new DenseMatrix ( 2, 2 ) {
	        [0, 0] = 2 * u[0],
	        [0, 1] = 4 * u[1],
	        [1, 0] = 4 * u[0],
	        [1, 1] = 2 * u[1]
	    };
	}
	DenseVector force = new DenseVector ( 2 ) { [0] = 3, [1] = 3 };
	Solver solver = Solver.Builder
	    .Solve ( 2, Function, Stiffness )
	    .Under ( force )
	    .WithInitialConditions ( 0.1, DenseVector.Create ( 2, 0 ), DenseVector.Create ( 2, 1 ) )
	    .UsingStandardNewtonRaphsonScheme ( 0.01 )
	    .Build ( );
	List<LoadState> states = solver.Broadcast ( ).TakeWhile ( x => x.Lambda <= 1 ).ToList ( );
	AssertSolutionsAreCorrect ( Function, force, states );
}

[Test]
public void SolveQuadraticFunctionArcLength() {
	Vector<double> Function( Vector<double> u ) {
	    return new DenseVector ( 2 ) {
	        [0] = u[0] * u[0] + 2 * u[1] * u[1],
	        [1] = 2 * u[0] * u[0] + u[1] * u[1]
	    };
	}
	Matrix<double> Stiffness( Vector<double> u ) {
	    return new DenseMatrix ( 2, 2 ) {
	        [0, 0] = 2 * u[0],
	        [0, 1] = 4 * u[1],
	        [1, 0] = 4 * u[0],
	        [1, 1] = 2 * u[1]
	    };
	}
	DenseVector force = new DenseVector ( 2 ) { [0] = 3, [1] = 3 };
	Solver solver = Solver.Builder
	    .Solve ( 2, Function, Stiffness )
	    .Under ( force )
	    .WithInitialConditions ( 0.1, DenseVector.Create ( 2, 0 ), DenseVector.Create ( 2, 1 ) )
	    .UsingArcLengthScheme ( 0.05 )
	    .NormalizeLoadWith ( 0.01 )
	    .Build ( );
	List<LoadState> states = solver.Broadcast ( ).TakeWhile ( x => x.Lambda <= 10 ).ToList ( );
	AssertSolutionsAreCorrect ( Function, force, states );
}

void AssertSolutionIsCorrect( Vector<double> solution ) {
	double first = solution.First ( );
	foreach (double d in solution) {
	    Assert.AreEqual ( first, d, numberTolerance );
	}
}
void AssertSolutionsAreCorrect( Func<Vector<double>, Vector<double>> reaction, Vector<double> force, List<LoadState> states ) {
	foreach (LoadState state in states) {
	    AssertSolutionIsCorrect ( state.Displacement );
	    TestUtils.AssertAreCloseEnough ( reaction ( state.Displacement ), state.Lambda * force, tolerance );
	}
}
```
