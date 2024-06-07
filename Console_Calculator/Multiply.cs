using System.Collections.Generic;

namespace Operations
{
    public sealed class Multiply : Operation
    {
        public override string Name => "Умножение";
        public override double Run(IEnumerable<double> Numbers)
        {
            double Output = 1;
            for (ushort i = 0; i < Numbers.Count(); i++)
            {
                Output *= Numbers.ElementAt<double>(i);
            }
            return Output;
        }
    }
}