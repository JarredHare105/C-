using System;

namespace AreaCalculator

{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("What is the Base? ");
            String baseVal = Console.ReadLine();
            double Base = Convert.ToDouble(baseVal);
            Console.WriteLine("What is the Height? ");
            String heightVal = Console.ReadLine();
            double Height = Convert.ToDouble(heightVal);
            double Area = (Base * Height)/2;
            Console.WriteLine("Area = " + Area);
        }
    }
}
