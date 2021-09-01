using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Job.PricesWF
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Inicia proceso de Cambio de Precios");
            Sincronizacion.Ejecuta_CambiosPrecio();
            Console.WriteLine("Termina proceso de Cambio de Precios");

            Console.WriteLine("Inicia proceso de Nuevas Partidas");
            Sincronizacion.Ejecuta_NuevasPartidas();
            Console.WriteLine("Termina proceso de Nuevas Partidas");

        }
    }
}
