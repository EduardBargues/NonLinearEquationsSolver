using MathNet.Numerics.LinearAlgebra;
using NonLinearEquationsSolver.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NonLinearEquationsSolver.Correction {
    internal class CorrectionSchemeArcLength : ICorrectionScheme {
        internal double Radius { get; set; }
        internal double Beta { get; set; }
        //internal bool AllowToChange { get; set; }
        internal IDisplacementChooser DisplacementChooser { get; set; }

        internal CorrectionSchemeArcLength( double radius ) {
            Radius = radius;
        }

        public LoadIncrementalState GetCorrection( LoadState state,
                                                   LoadIncrementalState prediction,
                                                   StructureInfo info,
                                                   Vector<double> dut,
                                                   Vector<double> dur ) {
            IEnumerable<LoadIncrementalState> candidates = GetCandidates ( dut, dur, prediction, info );
            return DisplacementChooser.Choose ( info, state, prediction, candidates );
        }

        IEnumerable<LoadIncrementalState> GetCandidates( Vector<double> dut,
                                                         Vector<double> dur,
                                                         LoadIncrementalState prediction,
                                                         StructureInfo info ) {
            double frNormSquare = info.ReferenceLoad.DotProduct ( info.ReferenceLoad );
            double betaSquare = Math.Pow ( Beta, 2 );

            double a = dut.DotProduct ( dut ) +
                       betaSquare * frNormSquare;

            double b = 2 * dur.DotProduct ( dut ) +
                       2 * prediction.IncrementDisplacement.DotProduct ( dut ) +
                       2 * prediction.IncrementLambda * betaSquare * frNormSquare;

            double c = -Math.Pow ( Radius, 2 ) +
                       prediction.IncrementDisplacement.DotProduct ( prediction.IncrementDisplacement ) +
                       dur.DotProduct ( dur ) +
                       2 * prediction.IncrementDisplacement.DotProduct ( dur ) +
                       Math.Pow ( prediction.IncrementLambda, 2 ) * betaSquare * frNormSquare;

            double control = Math.Pow ( b, 2 ) - 4 * a * c;

            if (control < 0)
                throw new Exception ( "Control variable negative. Arc length radius might be too big." );
            return new List<double>
                {
                    (-b + Math.Sqrt(control))/(2*a),
                    (-b - Math.Sqrt(control))/(2*a)
                }
                .Select ( dlambda => new LoadIncrementalState ( dlambda, dur + dlambda * dut ) );
        }
    }
}
