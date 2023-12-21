using System.Collections.Generic;
using System;

public static class LongestCommonSubsequenceCalculator
{
    public static List<string> Calculate(List<string> firstList, List<string> secondList)
    {
        int[,] optTable = CreateOptimizationTable(firstList, secondList);
        return RestoreAnswer(optTable, firstList, secondList);
    }

    private static int[,] CreateOptimizationTable(List<string> firstList, List<string> secondList)
    {
        int[,] optTable = new int[firstList.Count + 1, secondList.Count + 1];

        for (int i = firstList.Count - 1; i >= 0; i--)
        {
            for (int j = secondList.Count - 1; j >= 0; j--)
            {
                if (firstList[i] == secondList[j])
                {
                    optTable[i, j] = optTable[i + 1, j + 1] + 1;
                }
                else
                {
                    optTable[i, j] = Math.Max(optTable[i + 1, j], optTable[i, j + 1]);
                }
            }
        }

        return optTable;
    }

    private static List<string> RestoreAnswer(int[,] optTable, List<string> firstList, List<string> secondList)
    {
        List<string> result = new List<string>();
        int indexFirst = 0;
        int indexSecond = 0;

        while (indexFirst < firstList.Count && indexSecond < secondList.Count)
        {
            if (firstList[indexFirst] == secondList[indexSecond])
            {
                result.Add(firstList[indexFirst]);
                indexFirst++;
                indexSecond++;
            }
            else if (optTable[indexFirst, indexSecond] == optTable[indexFirst + 1, indexSecond])
            {
                indexFirst++;
            }
            else
            {
                indexSecond++;
            }
        }

        return result;
    }
}
