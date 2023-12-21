using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace yield;

public static class MovingMaxTask
{
	public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
	{
        double n = 0.0;
        LinkedList<double> wind = new LinkedList<double>();
        Queue<double> q = new Queue<double>();
        foreach (var item in data)
        {
            q.Enqueue(item.OriginalY);
            if (q.Count > windowWidth)
            {
                n = q.Dequeue();
                if (n == wind.First.Value)
                {
                    wind.RemoveFirst();
                }
            }
            while (wind.Count > 0 && item.OriginalY > wind.Last.Value)
            {
                wind.RemoveLast();
            }  
            wind.AddLast(item.OriginalY);
            yield return item.WithMaxY(wind.First.Value);
        }
    }
}