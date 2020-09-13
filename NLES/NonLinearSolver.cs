using System.Collections.Generic;
using System.Runtime.CompilerServices;

using NLES.Contracts;
using NLES.Correction;
using NLES.Prediction;

[assembly: InternalsVisibleTo("NLES.Tests")]
namespace NLES
{
    public partial class NonLinearSolver
    {
        internal LoadState State { get; set; }
        internal Predictor Predictor { get; set; } = new Predictor();
        internal Corrector Corrector { get; set; } = new Corrector();
        internal StructureInfo Info { get; set; } = new StructureInfo();

        public static SolverBuilder Builder => new SolverBuilder();

        public IEnumerable<LoadState> Broadcast()
        {
            ILinearSolver mK0 = Info.Stiffness(State.Displacement);
            Vector Dv0 = mK0.Solve(Info.ReferenceLoad);
            double k0 = Info.ReferenceLoad.DotProduct(Dv0);
            while (true)
            {
                LoadIncrementalState prediction = Predictor.Predict(State, k0, Info);
                State = State.Add(prediction);
                Result<LoadIncrementalState> correctionResult = Corrector.Correct(State, prediction, Info);
                if (correctionResult.IsSuccess)
                {
                    State = State.Add(correctionResult.Value);
                    yield return State;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
