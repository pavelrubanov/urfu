using System;
using System.Collections.Generic;

// Каждый документ — это список токенов. То есть List<string>.
// Вместо этого будем использовать псевдоним DocumentTokens.
// Это поможет избежать сложных конструкций:
// вместо List<List<string>> будет List<DocumentTokens>
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism;

public class LevenshteinCalculator
{
    public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documentsList)
    {
        var comparisonResults = new List<ComparisonResult>();
        for (var i = 0; i < documentsList.Count; i++)
        {
            for (var j = i + 1; j < documentsList.Count; j++)
            {
                comparisonResults.Add(CalculateLevenshteinDistance(documentsList[i], documentsList[j]));
            }
        }
        return comparisonResults;
    }

    public static ComparisonResult CalculateLevenshteinDistance(DocumentTokens firstDocument, DocumentTokens secondDocument)
    {
        var distanceMatrix = new double[firstDocument.Count + 1, secondDocument.Count + 1];
        InitializeFirstRow(distanceMatrix, firstDocument.Count);
        InitializeFirstColumn(distanceMatrix, secondDocument.Count);

        for (var i = 1; i <= firstDocument.Count; i++)
        {
            for (var j = 1; j <= secondDocument.Count; j++)
            {
                distanceMatrix[i, j] = CalculateMinimumDistance(distanceMatrix, firstDocument, secondDocument, i, j);
            }
        }
        return new ComparisonResult(firstDocument, secondDocument, distanceMatrix[firstDocument.Count, secondDocument.Count]);
    }

    private static void InitializeFirstRow(double[,] distanceMatrix, int rowCount)
    {
        for (var i = 0; i <= rowCount; i++)
        {
            distanceMatrix[i, 0] = i;
        }
    }

    private static void InitializeFirstColumn(double[,] distanceMatrix, int columnCount)
    {
        for (var j = 0; j <= columnCount; j++)
        {
            distanceMatrix[0, j] = j;
        }
    }

    private static double CalculateMinimumDistance(double[,] distanceMatrix, DocumentTokens firstDocument, DocumentTokens secondDocument, int rowIndex, int columnIndex)
    {
        if (firstDocument[rowIndex - 1] == secondDocument[columnIndex - 1])
        {
            return distanceMatrix[rowIndex - 1, columnIndex - 1];
        }

        var distance = Math.Min(TokenDistanceCalculator.GetTokenDistance(firstDocument[rowIndex - 1], secondDocument[columnIndex - 1]) + distanceMatrix[rowIndex - 1, columnIndex - 1], 1 + distanceMatrix[rowIndex, columnIndex - 1]);
        return Math.Min(distanceMatrix[rowIndex - 1, columnIndex] + 1, distance);
    }
}


