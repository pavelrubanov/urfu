using System;
using System.Numerics;

namespace Program
{
    class Program //LD4 (1590 Шифр Бэкона) РАБОТАЕТ ЗА О(n*n)
                  //вместо требуемого O(n*n*n) из-за правильного использования HashSet
    {
        private const int p = 31; //только строчные латинские
        public static void Main()
        {
            string str = Console.ReadLine();
            int n = str.Length;

            //вычисляем все степени p
            ulong[] p_pow = new ulong[n];
            p_pow[0] = 1;
            for (int i = 1; i < n; i++)
                p_pow[i] = p_pow[i - 1] * p;

            //считаем хэши от всех префиксов
            ulong[] pref_hash = new ulong[n];
            for (int i = 0; i < n; i++)
            {
                pref_hash[i] = Convert.ToUInt64(str[i] - 'a' + 1) * p_pow[i];
                if (i > 0) 
                    pref_hash[i] += pref_hash[i - 1];
            }

            int result = 0;
            for (int len = 1; len <=n; len++)
            {
                HashSet<ulong> len_hash = new HashSet<ulong>(n - len + 1); 
                /*используем hashset, чтобы хранить только уникальные хэши(подстроки)
                 * так как мы изначально зарезервировали место для (n - len + 1) элементов, то
                 * Add будет выполняться за О(1) и не добавлять неуникальные
                 */
                for (int i = 0; i < n - len + 1; i++)
                {
                    ulong cur_hash = pref_hash[i + len - 1];
                    if (i > 0) cur_hash -= pref_hash[i - 1];
                    //приводим все хеши к одной степени
                    cur_hash *= p_pow[n - i - 1];
                    len_hash.Add(cur_hash);
                }
                result += len_hash.Count;
            }

            Console.WriteLine(result);
        }
    }

}