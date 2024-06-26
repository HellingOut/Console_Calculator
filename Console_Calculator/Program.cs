﻿using System.Text.RegularExpressions;
using CalcConsole;
using Castle.Facilities.Startable;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Operations;
using static System.Net.Mime.MediaTypeNames;

public interface IOperation {
    public string name { get; }
}

public interface IOperationProvider
{
    public IEnumerable<Operation> Get();
}

public interface IMenu<out T>
{
    public IMenu<T> Show();
    public IMenuItemSelector<T> ItemSelector { get; }
}

public interface IMenuItemSelector<out T>
{
    public T Select();
}

public interface IOperationMenuItemSelector : IMenuItemSelector<Operation>
{

}

public interface IMenuItemSelectorProvider
{
    public int GetMenuItemId();
}

internal class LocalInstaller : IWindsorInstaller
{
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        container.Register(
            Component.For<IWindsorContainer>().Instance(container),
            Component.For<Application>()
                     .StartUsingMethod("Run"),
            /* Будет но позже
            Component.For<IOperationMenuItemSelector>()
                     .ImplementedBy<OperationMenuItemSelector>()
                     .LifestyleTransient(),
            Component.For<IMenuItemSelectorProvider>()
                     .ImplementedBy<OperationMenuItemSelectorView>()
                     .LifestyleTransient(),
            Component.For<IOperationProvider>()
                     .ImplementedBy<OperationProvider>(),

            Component.For<IMenu<IOperation>>()
                     .ImplementedBy<OperationMenu>()
                     .LifestyleTransient(),
            */
            Component.For<Operation>()
                     .ImplementedBy<Sum>(),
            Component.For<Operation>()
                     .ImplementedBy<Substraction>(),
            Component.For<Operation>()
                     .ImplementedBy<Multiply>(),
            Component.For<Operation>()
                     .ImplementedBy<Division>()
        );
    }
}

public class Application
{
    public Application(
        IMenu<IOperation> menu)
    {
        this.menu = menu;
    }

    private IMenu<IOperation> menu;

    public void Run()
    {
        Operation operation = (Operation)menu.Show().ItemSelector.Select();
        List<double> Numbers = new List<double>();
        Numbers.Add(15.6);
        Numbers.Add(100);
        double result = operation.Run(Numbers);
        Console.WriteLine($"Результат: {result}");
    }
}

internal class Program
{
    private static void Main()
    {
        const ushort MaxAction = 4;
        Console.WriteLine("Выберите действие: ");
        Console.WriteLine("0. Сложение");
        Console.WriteLine("1. Вычитание");
        Console.WriteLine("2. Умножение");
        Console.WriteLine("3. Деление");
        Console.WriteLine("4. Возведение в степень");
        ushort CurrentVariant = GetInput.GetShort(MaxAction);
        ushort NumbersCount = 1;
        switch (CurrentVariant)
        {
            case 0:
                Console.Write("Введите количество слагаемых (1 по умолчанию): ");
                NumbersCount = GetInput.GetShort(255);
                Sum sum_object = new Sum();
                double sum = sum_object.Run(GetInput.GetNumList(NumbersCount));
                Console.Write("Сумма всех чисел равна: " + sum.ToString());
                break;
            case 1:
                Console.Write("Введите количество операндов (1 по умолчанию): ");
                NumbersCount = GetInput.GetShort(255);
                Substraction substraction_object = new Substraction();
                double sub = substraction_object.Run(GetInput.GetNumList(NumbersCount));
                Console.Write("Разность всех чисел равна: " + sub.ToString());
                break;
            case 2:
                Console.Write("Введите количество множителей (1 по умолчанию): ");
                NumbersCount = GetInput.GetShort(255);
                Multiply multiply_object = new Multiply();
                double multiply = multiply_object.Run(GetInput.GetNumList(NumbersCount));
                Console.Write("Произведение всех чисел равна: " + multiply.ToString());
                break;
            case 3:
                Console.Write("Введите количество делителей (1 по умолчанию): ");
                NumbersCount = GetInput.GetShort(255);
                Division division_object = new Division();
                IEnumerable<double> Numbers = GetInput.GetNumList(NumbersCount);
                if (Division.IsNumbersValid(Numbers))
                {
                    Console.WriteLine("В числах обнаружен 0. На ноль делить нельзя.");
                }
                else
                {
                    double div = division_object.Run(Numbers);
                    Console.Write("Частное всех чисел равно: " + div);
                }
                break;
        }
        Console.WriteLine("\nВсе действия выполнены. Чтобы выйти нажмите любую клавишу");
        Console.ReadKey();
    }
}

namespace CalcConsole
{
    class GetInput
    {
        static bool TryReadNumber(string num)
        {
            float parsed_num = 0;
            return float.TryParse(num, out parsed_num);
        }
        public static ushort GetShort(ushort Max)
        {
            string pattern = "^[0-9]";
            string num = Console.ReadLine();
            Regex rg = new Regex(pattern);
            if (rg.IsMatch(num) && UInt64.Parse(num) < Max)
                return UInt16.Parse(num);
            else
                Console.WriteLine("Неправильно.");
            return GetShort(Max);
        }
        public static double GetNum()
        {
            string num = Console.ReadLine();
            if (TryReadNumber(num))
                return double.Parse(num);
            else
                return GetNum();
        }
        public static IEnumerable<double> GetNumList(ushort NumbersCount)
        {
            List<double> Numbers = new List<double>();
            for (ushort i = 0; i < NumbersCount; i++)
            {
                Console.Write("Введите число " + i.ToString() + ": ");
                double num = GetInput.GetNum();
                Numbers.Add(num);
            }
            return Numbers;
        }
    }
}