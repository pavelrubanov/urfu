using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Linq;
using System.Linq.Expressions;

namespace Program;

public class Student
{
    public string LastName { get; set; }
    public string Group { get; set; }
}

public static class IEnumerableExtensions
{
    public static IDictionary<TKey, ICollection<TSource>> GroupBy<TKey, TSource>(this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector)
    {
        var result = new Dictionary<TKey, ICollection<TSource>>(); //?
        foreach(var element in enumerable)
        {
            if (!result.Keys.Contains(keySelector(element)))
            {
                result[keySelector(element)] = new List<TSource>(); //?
            }
            result[keySelector(element)].Add(element);
        }
        return result;
    }
}

class Program
{ 
    public static void Main()
    {
        var students = new List<Student>();
        students.Add(new Student() { Group = "1", LastName = "N1" });
        students.Add(new Student() { Group = "1", LastName = "N2" });
        students.Add(new Student() { Group = "2", LastName = "N3" });
        students.Add(new Student() { Group = "2", LastName = "N4" });
        students.Add(new Student() { Group = "2", LastName = "N5" });
        students.Add(new Student() { Group = "2", LastName = "N6" });
        students.Add(new Student() { Group = "3", LastName = "N7" });
        var result = students.GroupBy(s => s.Group);
        foreach(var key in result.Keys)
        {
            Console.WriteLine(key);
            foreach (var el in result[key])
            {
                Console.WriteLine(el.LastName);
            }
            Console.WriteLine();
        }
    }

}

