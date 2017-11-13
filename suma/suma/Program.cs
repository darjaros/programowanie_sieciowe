using System;
using System.Collections.Generic;
using System.Text;

namespace suma
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("podaj dane:");
            string dane = Console.ReadLine();
            byte ak = 0;
            byte b3 = 0;
            byte b2 = 0;
            byte mask = 1;
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] buf = encoding.GetBytes(dane);
            foreach (byte b in buf)
            {    
                ak ^= b;
                Console.WriteLine(ak);
            }
            for (byte i = 0; i < 8; i++){
                mask = (byte) (1 << i);
                
                b2 = (byte) (ak & mask);
                b3 ^= (byte)(b2 >> i);
                    
            }
            Console.WriteLine(b3);
        }
    }
}