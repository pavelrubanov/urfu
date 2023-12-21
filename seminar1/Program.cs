namespace seminar1
{
    internal class Program 
    {
        private int L;
        private int CurrenctCount = 0;
        private string[] FirstFormulas;
        private static string[] operations = { "+", "-", "*" };
        private Dictionary<int[],bool> dictionary = new Dictionary<int[],bool>();

        private IEnumerable<string> MakeNewFormulas(List<string> LastFormulas) //создает новые формулы размера N, попарно перемножая/складывая/вычитая формулы размера 0 (FirstFormulas) и N-1(LastFormulas)
        {
            List<string> formulas = new List<string>();
            for (int i = 0; i < operations.Length; i++)
            {
                foreach (string f1 in LastFormulas)
                    foreach (string f2 in FirstFormulas)
                    {
                        if (CurrenctCount==0 || operations[i]=="+" || operations[i] == "-")
                            formulas.Add( f1 + operations[i] + f2);
                        else
                            formulas.Add("(" + f1 + ")" + operations[i] + f2);
                        if (CurrenctCount == L - 1) yield return formulas[formulas.Count - 1];
                    }        
            }
            CurrenctCount++;
            foreach (var f in MakeNewFormulas(formulas))
                yield return f;
        }

        //НЕ РЕАЛИЗОВАННА:
        private void CalculateAndCheck(string formula) //удаляет эквивалентные (преобразует в List { a0, a1, a2 ... aL } , где 0 * x^0 + a1 * x^1 + a2 * x^2...
        {
            List<int> f = new List<int>();
            int i = CurrenctCount + 1; //первые элементы это (
            if (formula[i]<='2' && formula[i]>='0') f.Add(int.Parse(formula[i].ToString()));
            else
            {
                f.Add(0);
                f.Add(1);
            }
            char LastOperation=' ';
            for (; i<formula.Length;i++)
            {
                if (formula[i]!=')')
                {
                    if (formula[i] == '+' || formula[i] == '-' || formula[i] == '*')
                        LastOperation = formula[i];
                    else if (formula[i] <= '2' && formula[i] >= '0') //не X
                    {
                        if (LastOperation == '+')
                            f[0] += int.Parse(formula[i].ToString());
                        else if (LastOperation == '-')
                            f[0] -= int.Parse(formula[i].ToString());
                        else if (LastOperation == '*')
                            for (int j = 0; j < f.Count; j++) f[j] = f[j] * (int.Parse(formula[i].ToString()));
                    }
                    else
                    {
                        if (LastOperation == '+')
                        {
                            if (f.Count < 2) f.Add(1);
                            else f[1]++;
                        }
                        else if (LastOperation == '-')
                        {
                            if (f.Count < 2) f.Add(-1);
                            else f[1]--;
                        }
                        else if (LastOperation == '*')
                        {
                            int prevA0 = f[0];
                            int lastIndex = f.Count - 1;
                            if (f[lastIndex] == 0) break;
                            f.Add(f[lastIndex]);
                            for (int j = lastIndex; j > 0; j++) f[j] = f[j - 1];
                            f[0] = 0;
                        }
                    }

                }
            }
        }
        public Program(int L, string[] s)
        {
            this.L = L;
            this.FirstFormulas = s;
        }
        static void Main(string[] args)
        {
            string[] s = { "0", "1", "2", "x" };
            Console.WriteLine("Введите L:");
            Program p = new Program(Convert.ToInt32(Console.ReadLine()), s);
            foreach (var f in p.MakeNewFormulas(new List<string>(s)))
                Console.WriteLine(f);
        }
    }
}