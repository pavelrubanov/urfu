using System;

public class Fibonacci
{
    public static int GetNthFibonacci(int n)
    {
        if (n <= 1)
        {
            return n;
        }

        int[] fibNumbers = new int[n + 1];
        fibNumbers[0] = 0;
        fibNumbers[1] = 1;

        for (int i = 2; i <= n; i++)
        {
            fibNumbers[i] = fibNumbers[i - 1] + fibNumbers[i - 2];
        }

        return fibNumbers[n];
    }

    public static void Main()
    {
        int n = 10;
        int nthFibonacci = GetNthFibonacci(n);

        Console.WriteLine($"The {n}th Fibonacci number is {nthFibonacci}");
    }
}
