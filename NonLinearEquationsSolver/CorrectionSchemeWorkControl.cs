using MathNet.Numerics.LinearAlgebra;

namespace NonLinearEquationsSolver {
    internal class CorrectionSchemeWorkControl : ICorrectionScheme {
        internal double WorkIncrement { get; set; }

        public CorrectionSchemeWorkControl( double work ) => WorkIncrement = work;

        public LoadIncrementalStateResult GetCorrection( LoadState state,
                                                   LoadIncrementalState prediction,
                                                   StructureInfo info,
                                                   Vector<double> dut,
                                                   Vector<double> dur ) {
            double dlambda = -info.ReferenceLoad.DotProduct ( dur ) / info.ReferenceLoad.DotProduct ( dut );
            return new LoadIncrementalStateResult ( new LoadIncrementalState ( dlambda, dlambda * dut + dur ), true, "" );
        }
    }
}
