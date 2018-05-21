using MathNet.Numerics.LinearAlgebra;
using NonLinearEquationsSolver.Common;

namespace NonLinearEquationsSolver.Correction {
    internal class CorrectionSchemeWorkControl : ICorrectionScheme {


        internal double WorkIncrement { get; set; }

        public CorrectionSchemeWorkControl( double work ) => this.WorkIncrement = work;

        public LoadIncrementalState GetCorrection( LoadState state,
                                                   LoadIncrementalState prediction,
                                                   StructureInfo info,
                                                   Vector<double> dut,
                                                   Vector<double> dur ) {
            double dlambda = -info.ReferenceLoad.DotProduct ( dur ) / info.ReferenceLoad.DotProduct ( dut );
            return new LoadIncrementalState ( dlambda, dlambda * dut + dur );
        }
    }
}
