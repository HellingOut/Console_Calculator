using System.Collections.Generic;

namespace Operations
{
    public sealed class Sum : Operation
    {
        public override string Name => "Сумма";
        public override double Run(IEnumerable<double> Numbers)
        {
            double Output = 0;
            for (ushort i = 0; i < Numbers.Count(); i++)
            {
                Output += Numbers.ElementAt<double>(i);
            }
            return Output;
        }
    }
}