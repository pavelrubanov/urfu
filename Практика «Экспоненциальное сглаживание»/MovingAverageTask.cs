using System.Collections;
using System.Collections.Generic;

namespace yield;

public static class MovingAverageTask
{
	public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
	{
        Queue<double> q = new Queue<double>();
        int co = 0;
        double sum = 0.0;
        foreach (var item in data)
        {
            DataPoint result = new DataPoint(item);
            if (co == windowWidth)
            {
                sum -= q.Dequeue();
                co--;
            }
            q.Enqueue(item.OriginalY);
            co++;
            sum += item.OriginalY;
            yield return result.WithAvgSmoothedY(sum / co);
        }
    }
}
