﻿using System;
using System.Text.RegularExpressions;
using Yale.Engine;

namespace Yale.InteractiveConsole
{
    internal class Program
    {
        private readonly ComputeInstance _instance = new ComputeInstance();
        private Regex isValue = new Regex("^[a-zA-Z]+[=][\\w]+$");
        private Regex isExpression = new Regex("[a-zA-Z]+[:].+$");
        private Regex isEvaluate = new Regex("[a-zA-Z]+");

        private void Run()
        {
            Console.WriteLine("Syntax:\nAdd value: x=3 \nAdd expression: square:x^2 \nEvaluate: square\n\n\n");

            while (true)
            {
                var input = Console.ReadLine().Trim();

                if (isValue.IsMatch(input)) AddValue(input);
                else if (isExpression.IsMatch(input)) AddExpression(input);
                else if (isEvaluate.IsMatch(input)) TryEvaluate(input);
            }
        }

        private void TryEvaluate(string input)
        {
            if (_instance.ContainsExpression(input))
            {
                var result = _instance.GetResult(input);
                Console.WriteLine($"Result: {result}");
            }
            else
            {
                Console.WriteLine("Expression not found");
            }
        }

        private void AddExpression(string input)
        {
            var values = input.Split(':');
            _instance.AddExpression(values[0], values[1]);
        }

        private void AddValue(string input)
        {
            var values = input.Split('=');
            var key = values[0];
            var value = values[1];

            if (int.TryParse(value, out var integer))
            {
                _instance.Variables.Add(key, integer);
            }
            else if (double.TryParse(value, out var number))
            {
                _instance.Variables.Add(key, number);
            }
            else
            {
                _instance.Variables.Add(key, value);
            }
        }

        private static void Main(string[] args)
        {
            new Program().Run();
        }
    }
}