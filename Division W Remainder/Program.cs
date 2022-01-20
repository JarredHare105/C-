using System;

namespace DivisionWRemainder

{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("How many eggs are there? ");
            String eggVal = Console.ReadLine();
            int eggs = Convert.ToInt32(eggVal);
            Console.WriteLine("How many people are eating? ");
            String peopleVal = Console.ReadLine();
            int people = Convert.ToInt32(peopleVal);
            int eggsSplit = eggs / people;
            int remainder = eggs % people;
            Console.WriteLine("Each person gets " + eggsSplit + " egg(s) " + "there is " + remainder + " egg(s) leftover");

        }
    }
}
