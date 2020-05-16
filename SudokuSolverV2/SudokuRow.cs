using System;
using System.Collections.Generic;

namespace SudokuSolverV2
{
    internal class SudokuRow : SudokuSmallContainer
    {
        public bool wasChanged { get; set; }
        public SudokuRow(int size)
        {
            container = new List<Field>();
            valuesToSet = new List<int>();
            for (int i = 1; i <= size; i++)
                valuesToSet.Add(i);
            isDone = false;
        }

        public bool inputRow(string input)
        {
            string[] inputValues = input.Split(" ");

            if (!(inputValues.Length == container.Count))
            {
                Console.WriteLine("Zła liczba parametrów. ");
                return false;
            }
            for(int i = 0; i < inputValues.Length; i++)
            {
                int value;
                if (!Int32.TryParse(inputValues[i], out value))
                {
                    Console.WriteLine("Podano parametr nie będący liczbą");
                    return false;
                }
                if (value < 0 && value > container.Count)
                {
                    Console.WriteLine($"Wartość nie mieście się w przedziale od 0 do {container.Count}");
                    return false;
                }
                if (value == 0)
                    continue;
                if (!container[i].Optionalities.Contains(value) )
                {
                    Console.WriteLine($"Wartość {value} nie może zostać wpisana w wyznaczone miejsce.");
                    return false;
                }
                container[i].Value = value;
            }
            return true;
        }
        public override string ToString()
        {
            string output = "|";
            foreach (var field in container)
                output += (field.IsValueSet ? field.Value.ToString() : string.Join(' ', field.Optionalities.ToArray())) + '|';
            return output;
        }
    }
}