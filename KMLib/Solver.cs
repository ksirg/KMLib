﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KMLib.Kernels;

namespace KMLib
{
    public abstract class Solver<TProblemElement>
    {
        public Problem<TProblemElement> problem;
        public float C;
        public IKernel<TProblemElement> kernel;


        public Solver(Problem<TProblemElement> problem, IKernel<TProblemElement> kernel, float C)
        {
            this.problem = problem;
            this.kernel = kernel;
            this.C = C;
        }

        public abstract Model<TProblemElement> ComputeModel();


       // public abstract void Init() { }
        
    }
}