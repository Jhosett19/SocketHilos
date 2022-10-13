using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Threading;

public class Mensajes
{
    public void MetodoComplejo()
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine("Escribiendo desde ==> Metodo Complejo");
            Thread.Sleep(1000);
        }
    }

    public void MetodoFacil()
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine("Escribiendo desde ==> Metodo Facil");
            Thread.Sleep(800);
        }
    }
    public void MetodoParametros(object parametros)
    {
        Int32[] numeros = (Int32[])parametros;
        Int32 resultado = numeros[0] + numeros[1];
        Console.Write("Suma de Variables: ");
        Console.WriteLine(resultado);
    }
    public void MyParametrizedMethod(string p, int i)
    {
        Console.WriteLine(p);
        Console.WriteLine(i);
    }
}


public class Ejemplo
{
    public int[] vector;
    public static void Main()
    {
        /*
        Mensajes msg = new Mensajes();

        Thread th1 = new Thread(new ThreadStart(msg.MetodoComplejo));
        Thread th2 = new Thread(new ThreadStart(msg.MetodoFacil));
        Thread th3 = new Thread(new ParameterizedThreadStart(msg.MetodoParametros));

        string param1 = "hello";
        int param2 = 42;

        Thread th4 = new Thread(()=>msg.MyParametrizedMethod(param1, param2));
        


        Int32[] numeros = { 1, 2 };

        th1.Priority = ThreadPriority.Highest;

        th1.Start();
        th2.Start();
        th3.Start(numeros);
        th4.Start();

        th1.Join();
        th2.Join();
        th3.Join();
        th4.Join();
        Console.WriteLine("Juntar los Arrays");
        */


        Ejemplo pv = new Ejemplo();
        pv.vector = new int[1000];
        Random rnd = new Random(10);
        for (int i = 0; i < 1000; i++)
        {
            pv.vector[i] = rnd.Next(1, 1000);
        }


        DateTime start = DateTime.Now;
        pv.MetodoBurbuja();
        DateTime end = DateTime.Now;

        TimeSpan ts = (end - start);
        Console.WriteLine("Elapsed Time is {0} ms", ts.TotalMilliseconds);

        pv.Imprimir();


    }

    public void MetodoBurbuja()
    {
        int t;
        for (int a = 1; a < vector.Length; a++)
            for (int b = vector.Length - 1; b >= a; b--)
            {
                if (vector[b - 1] > vector[b])
                {
                    t = vector[b - 1];
                    vector[b - 1] = vector[b];
                    vector[b] = t;
                }
            }
    }
    public void Imprimir()
    {
        Console.WriteLine("Vector ordenados en forma ascendente");
        for (int f = 0; f < vector.Length; f++)
        {
            Console.Write(vector[f] + "  ");
        }
        Console.ReadKey();
    }

}