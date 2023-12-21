using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask
{
    /// <summary>
    /// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
    /// Медиана списка из четного количества элементов — это среднее арифметическое 
    /// двух серединных элементов списка после сортировки.
    /// </summary>
    /// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
    public static double Median(this IEnumerable<double> items)
    {
        var list = items.ToList();
        var cnt = list.Count;
        if (cnt <= 0) throw new InvalidOperationException();
        list.Sort();
        var di = 2 - (cnt & 1);
        return list.Skip((cnt >> 1) - di + 1).Take(di).Sum() / di;
    }

    /// <returns>
    /// Возвращает последовательность, состоящую из пар соседних элементов.
    /// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
    /// </returns>
    public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
    {
        var prev = default(T);
        foreach (var el in items)
        {
            prev = el;
            break;
        }
        return items.Select(item => (prev, prev = item)).Skip(1);
    }
}