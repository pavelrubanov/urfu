using System;

namespace Program
{ 
    class Program
    {
        public static void Main()
        {
            int[] arr = new int[10];
            for(int i = 0; i < 10; i++)
            {
                arr[i] = i;
            }
            arr = Move(arr, 3);
            foreach(int el in arr)
            {
                Console.WriteLine(el);
            }
        }
        /// <summary>
        /// Циклический сдвиг массива влево на k
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static T[] Move<T>(T[] array, int k) =>
            array.Skip(k).Concat(array.Take(k)).ToArray();
    }
}
