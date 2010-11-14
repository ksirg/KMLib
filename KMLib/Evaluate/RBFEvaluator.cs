﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnAnalytics.LinearAlgebra;
using KMLib.Kernels;

namespace KMLib.Evaluate
{

    /// <summary>
    /// Rerpesents evaluator for RBF kernel, use some optimization
    /// is faster for RBF than <see cref="SequentialEvalutor"/>
    /// </summary>
    public class RBFEvaluator : EvaluatorBase<SparseVector>
    {
        LinearKernel linKernel;

        float gamma = 0.5f;
        public RBFEvaluator(float gamma)
        {
            linKernel = new LinearKernel();
            this.gamma = gamma;

        }
       
        /// <summary>
        /// Predicts the specified elements.
        /// </summary>
        /// <remarks>Use some optimization for RBF kernel</remarks>
        /// <param name="elements">The elements.</param>
        /// <returns>array of predicted labels</returns>
        public override float[] Predict(SparseVector[] elements)
        {

            linKernel.ProblemElements = TrainedModel.SupportElements;

            linKernel.Init();

            float[] predictions = new float[elements.Length];


            for (int i = 0; i < elements.Length; i++)
            {
                float x1Squere = linKernel.Product(elements[i], elements[i]);
                float sum = 0;

                int index = -1;

                for (int k = 0; k < TrainedModel.SupportElementsIndexes.Length; k++)
                {
                    //support vector squere
                    float x2Squere = linKernel.DiagonalDotCache[k];

                    float dot = linKernel.Product(elements[i], TrainedModel.SupportElements[k]);

                    float rbfVal = (float)Math.Exp(-gamma * (x1Squere + x2Squere - 2 * dot));


                    index = TrainedModel.SupportElementsIndexes[k];
                    sum += TrainedModel.Alpha[index] * TrainningProblem.Labels[index] * rbfVal;
                }
                sum -= TrainedModel.Rho;
                predictions[i] = sum > 0 ? 1 : -1;
            }

            return predictions;

        }

       
    }
}