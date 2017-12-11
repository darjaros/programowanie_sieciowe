
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
            string tekst = Console.ReadLine();
            int dane = 0;

            foreach (char i in tekst)
            {
                dane ^= (int)(i);
            }
            long ak = dane;
            long a = 0;
            long b = dane << 2;
            long crc = 0;
            
            long mask = 0b101;
            Console.WriteLine(mask);
            Console.WriteLine(b);
            for (int i = 8; i >= 0; i--){
                if ((b>>(i+2)==1)){
                    a = (mask << i);
                    b ^= a;
                    Console.WriteLine(a);
                    Console.WriteLine(b);
                    Console.WriteLine(i);
                    Console.WriteLine("byłem tu");
                    
                }
                
            }
            crc = dane << 2 ^ b;
            Console.WriteLine(crc);
            Console.ReadLine();
        }
        
    }
}