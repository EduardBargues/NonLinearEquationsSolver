using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using NLES.Contracts;
using NLES.Correction.Methods;

namespace NLES.Correction
{
    internal class CorrectionSchemeArcLength : ICorrectionScheme
    {
        internal double Radius { get; set; }
        internal double Beta { get; set; } = 1;
        internal IDisplacementSelector DisplacementSelector { get; set; } = new RestoringMethod();

        internal CorrectionSchemeArcLength(double radius) => Radius = radius;

        public Result<LoadIncrementalState> Correct(LoadState state,
                                                   LoadIncrementalState prediction,
                                                   StructureInfo info,
                                                   Vector<double> dut,
                                                   Vector<double> dur)
        {
            Result<LoadIncrementalState[]> candidates = GetCandidates(dut, dur, prediction, info);

            return candidates.IsSuccess
                ? new Result<LoadIncrementalState>()
                {
                    Value = DisplacementSelector.SelectDisplacement(info, state, prediction, candidates.Value)
                }
                : new Result<LoadIncrementalState>()
                {
                    Errors = candidates.Errors
                };
        }

        Result<LoadIncrementalState[]> GetCandidates(Vector<double> dut,
                                                         Vector<double> dur,
                                                         LoadIncrementalState prediction,
                                                         StructureInfo info)
        {
            var result = new Result<LoadIncrementalState[]>();

            double frNormSquare = info.ReferenceLoad.DotProduct(info.ReferenceLoad);
            double betaSquare = Math.Pow(Beta, 2);

            double a = dut.DotProduct(dut) +
                       betaSquare * frNormSquare;

            double b = 2 * dur.DotProduct(dut) +
                       2 * prediction.IncrementDisplacement.DotProduct(dut) +
                       2 * prediction.IncrementLambda * betaSquare * frNormSquare;

            double c = -Math.Pow(Radius, 2) +
                       prediction.IncrementDisplacement.DotProduct(prediction.IncrementDisplacement) +
                       dur.DotProduct(dur) +
                       2 * prediction.IncrementDisplacement.DotProduct(dur) +
                       Math.Pow(prediction.IncrementLambda, 2) * betaSquare * frNormSquare;

            double control = Math.Pow(b, 2) - 4 * a * c;

            if (control < 0)
            {
                result.Errors.Add(new Error("Control variable negative. Arc length radius might be too big."));
            }
            else
            {
                result.Value = new List<double>
                {
                    (-b + Math.Sqrt(control))/(2*a),
                    (-b - Math.Sqrt(control))/(2*a)
                }
                .Select(dlambda => new LoadIncrementalState(dlambda, dur + dlambda * dut))
                .ToArray();
            }

            return result;
        }
    }
}
