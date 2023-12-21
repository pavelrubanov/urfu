using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable<byte>
    {
        private byte[] arr;
        private int hash;
        private bool hashed = false;
        public int Length { get; }

        public ReadonlyBytes(params byte[] input)
        {
            arr = input;
            try
            {
                Length = input.Length;
            }
            catch (Exception error)
            {
                throw new ArgumentNullException();
            }
        }

        public byte this[int index]
        {
            get
            {
                if (index >= arr.Length || index < 0) throw new IndexOutOfRangeException();
                return arr[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("[");
            if (arr.Length == 0) return "[]";
            for (int i = 0; i < arr.Length; i++)
                result.Append(arr[i].ToString() + ", ");
            result.Remove(result.Length - 2, 2);
            result.Append("]");
            return result.ToString();
        }

        public override bool Equals(object obj1)
        {
            if (obj1 == null) return false;
            if (obj1.GetType() != new ReadonlyBytes().GetType()) return false;
            ReadonlyBytes obj2 = obj1 as ReadonlyBytes;
            if (obj2.Length != arr.Length) return false;
            if (obj1.GetHashCode() != this.GetHashCode()) return false;
            for (int i = 0; i < arr.Length; i++)
            {
                if (obj2[i] != arr[i]) return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                if (hashed) return hash;
                hash = 1000;
                int num = 473;
                for (int i = 0; i < arr.Length; i++)
                {
                    hash *= num;
                    hash ^= arr[i];
                }
                hashed = true;
            }
            return hash;
        }

        public IEnumerator<byte> GetEnumerator()
        {
            foreach (byte element in arr)
                yield return element;
        }
    }

}