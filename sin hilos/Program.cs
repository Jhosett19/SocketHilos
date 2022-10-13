
using System;
using System.Diagnostics;

namespace App
{
    class Program
    {
        public static void ordeBurble(double[] arreglo)
        {
            for (int i = 0; i < arreglo.Length; i++)
            {
                for (int j = 0; j < arreglo.Length - 1; j++)
                {
                    int x = j + 1;
                    if (arreglo[j] > arreglo[x])
                    {
                        double temp = arreglo[j];
                        arreglo[j] = arreglo[x];
                        arreglo[x] = temp;
                    }
                }
            }
        }


        static void Main(String[] args)
        {


            Random rdn = new Random();
            Stopwatch st = new Stopwatch();

            int inputNumber = int.Parse(Console.ReadLine());

            int min = 1;
            int max = 200;
            double[] numeros = new double[inputNumber];

            for (int i = 0; i < inputNumber; i++)
            {
                numeros[i] = rdn.Next(min, max + 1);
            }


            Console.WriteLine("antes de ordenar");
            foreach (var n in numeros)
            {
                Console.WriteLine(n);
            }
            st.Start();

            Console.WriteLine("ordenado");
            ordeBurble(numeros);
            foreach (var n in numeros)
            {
                Console.WriteLine(n);
            }



            st.Stop();
            
            Console.WriteLine("Tiempo Total {0} milisegundos", st.ElapsedMilliseconds);

        }
    }
}
