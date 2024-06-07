using System.Collections.Generic;

namespace Operations
{
    public abstract class Operation
    {
        public abstract string Name { get; }
        public abstract double Run(IEnumerable<double> Numbers);
    }
}