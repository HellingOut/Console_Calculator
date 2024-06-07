using System.Collections.Generic;

namespace Operations
{
    public sealed class Division : Operation
    {
        public override string Name => "Деление";
        public static bool IsNumbersValid(IEnumerable<double> Numbers)
        {
            for (ushort i = 1; i < Numbers.Count(); i++)
            {
                if (Numbers.ElementAt(i) == 0)
                    return false;
            }
            return true;
        }

        public override double Run(IEnumerable<double> Numbers)
        {
            double Output = Numbers.ElementAt<double>(0);
            for (ushort i = 1; i < Numbers.Count(); i++)
            {
                Output /= Numbers.ElementAt(i);
            }
            return Output;
        }
    }
}