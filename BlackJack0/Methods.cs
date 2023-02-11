using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack0
{
    static class Methods
    {
        public static void DibujarTablero(List<int> tablero)
        {
            List<string> tableroAuxiliar = new();
            int index = 0;
            String impresion = "\n";
            foreach (int casilla in tablero)
            {
                if (casilla == 4)
                    tableroAuxiliar.Add(Convert.ToString(index + 1));
                else if (casilla == 1)
                    tableroAuxiliar.Add("X");
                else
                    tableroAuxiliar.Add("O");
                index++;
            }
            index = 0;
            foreach (string casilla in tableroAuxiliar)
            {
                if (index == 2 || index == 5 || index == 8)
                    impresion = impresion + casilla + " \n";
                else
                    impresion = impresion + casilla + " | ";
                index++;
            }
            Console.WriteLine(impresion);
        }
        public static int SelecionarCasillaCasa(List<int> tablero, int marcaCasa, int marcaJugador, int turnoCasa)
        {
            Random random = new Random();
            int casilla = 4;
            if (marcaCasa == 0 && tablero[4] == 4)
                casilla = 4;
            else if (marcaCasa == 0 && SumaCasillas(tablero, 0, 2, 6) + tablero[8] == 10 && turnoCasa < 3)
            {
                casilla = 2 * random.Next(0, 3) + 1;
                casilla = VerificarCasillaCasaCentral(casilla, tablero);
            }
            else if (GanarTriqui(tablero, marcaCasa) != -1)
                casilla = GanarTriqui(tablero, marcaCasa);
            else if (GanarTriqui(tablero, marcaJugador) != -1)
                casilla = GanarTriqui(tablero, marcaJugador);
            else if (CompletarTriqui(tablero, marcaCasa) != -1)
                casilla = CompletarTriqui(tablero, marcaCasa);
            else
            {
                if (marcaCasa == 0 && tablero[4] == 1 && SumaCasillas(tablero, 0, 2, 6) + tablero[8] == 16)
                {
                    Console.WriteLine("Decidí");
                    casilla = 2 * random.Next(0, 4);
                    casilla = VerificarCasillaCasaEsquina(casilla, tablero);
                }
                else if (marcaCasa == 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (BuscarCasilla(tablero, i, i + 3, i + 6) == 4)
                            casilla = BuscarCasilla(tablero, i, i + 3, i + 6);
                    }
                }


            }

            return casilla;
        }
        public static int VerificarCasilla(int casilla, List<int> tablero)
        {
            if (casilla > 0 && casilla < 10)
            {
                while (tablero[casilla - 1] == 0 || tablero[casilla - 1] == 1)
                {
                    Console.WriteLine("Está casilla no es valida. Digita el número de una casilla libre...\n");
                    casilla = int.Parse(Console.ReadLine());
                }
            }
            else
            {
                while (casilla <= 0 || casilla > 9)
                {
                    Console.WriteLine("Este número no corresponde a ninguna casilla. Los números válidos son los que aparecen en la cuadrícula...\n");
                    casilla = int.Parse(Console.ReadLine());
                }
            }

            return casilla;
        }
        public static int VerificarCasillaCasaCentral(int casilla, List<int> tablero)
        {
            Random random = new();
            {
                casilla = 2 * random.Next(0, 3) + 1;
            }
            return casilla;
        }
        public static int VerificarCasillaCasaEsquina(int casilla, List<int> tablero)
        {
            Random random = new();
            {
                casilla = 2 * random.Next(0, 4);
            }
            return casilla;
        }
        public static int SumaCasillas(List<int> tablero, int a, int b, int c)
        {
            return tablero[a] + tablero[b] + tablero[c];
        }
        public static int BuscarCasilla(List<int> tablero, int a, int b, int c)
        {
            for (int i = a; i <= c; i += b - a)
            {
                if (tablero[i] == 4)
                    return i;
            }
            return -1;
        }
        public static int BuscarCasillaEstrategica(List<int> tablero, int a, int b, int c, int marcaCasa)
        {
            List<int> posiblesCasillas = new List<int>();
            List<int> tableroPrueba = tablero.ToList();
            Random random = new();
            for (int i = a; i <= c; i += b - a)
            {
                if (tablero[i] == 4)
                    posiblesCasillas.Add(i);
            }
            foreach (int casilla in posiblesCasillas)
            {
                tableroPrueba[casilla] = marcaCasa;
                if (EsEstrategicaLaCasilla(tableroPrueba, marcaCasa))
                    return casilla;
                tableroPrueba = tablero.ToList();
            }
            return posiblesCasillas[0];

        }
        public static int CompletarTriqui(List<int> tablero, int marcaCasa)
        {
            int a = 0;

            if (SumaCasillas(tablero, 0, 4, 8) == (8 + marcaCasa))
                return BuscarCasillaEstrategica(tablero, 0, 4, 8, marcaCasa);
            else if (SumaCasillas(tablero, 2, 4, 6) == (8 + marcaCasa))
                return BuscarCasillaEstrategica(tablero, 2, 4, 6, marcaCasa);
            for (int i = 0; i < 3; i++)
            {
                if (SumaCasillas(tablero, a, a + 1, a + 2) == (8 + marcaCasa))
                    return BuscarCasillaEstrategica(tablero, a, a + 1, a + 2, marcaCasa);
                else if (SumaCasillas(tablero, i, i + 3, i + 6) == (8 + marcaCasa))
                    return BuscarCasillaEstrategica(tablero, i, i + 3, i + 6, marcaCasa);

                a += 3;
            }
            return -1;
        }
        public static int GanarTriqui(List<int> tablero, int marcaCasa)
        {
            int a = 0;
            if (SumaCasillas(tablero, 0, 4, 8) == (4 + 2 * marcaCasa))
                return BuscarCasilla(tablero, 0, 4, 8);
            else if (SumaCasillas(tablero, 2, 4, 6) == (4 + 2 * marcaCasa))
                return BuscarCasilla(tablero, 2, 4, 6);

            for (int i = 0; i < 3; i++)
            {
                if (SumaCasillas(tablero, a, a + 1, a + 2) == (4 + 2 * marcaCasa))
                    return BuscarCasilla(tablero, a, a + 1, a + 2);
                else if (SumaCasillas(tablero, i, i + 3, i + 6) == (4 + 2 * marcaCasa))
                    return BuscarCasilla(tablero, i, i + 3, i + 6);

                a += 3;
            }
            return -1;
        }
        public static bool EsEstrategicaLaCasilla(List<int> tablero, int marcaCasa)
        {
            int a = 0;
            if (SumaCasillas(tablero, 0, 4, 8) == (4 + 2 * marcaCasa))
                return true;
            else if (SumaCasillas(tablero, 2, 4, 6) == (4 + 2 * marcaCasa))
                return true;

            for (int i = 0; i < 3; i++)
            {
                if (SumaCasillas(tablero, a, a + 1, a + 2) == (4 + 2 * marcaCasa))
                    return true;
                else if (SumaCasillas(tablero, i, i + 3, i + 6) == (4 + 2 * marcaCasa))
                    return true;

                a += 3;
            }
            return false;
        }
        public static int VerificarTriqui(List<int> tablero)
        {
            int a = 0;
            bool empate;
            if (SumaCasillas(tablero, 0, 4, 8) == 3 || SumaCasillas(tablero, 2, 4, 6) == 3)
                return 1;
            else if (SumaCasillas(tablero, 0, 4, 8) == 0 || SumaCasillas(tablero, 2, 4, 6) == 0)
                return 0;

            for (int i = 0; i < 3; i++)
            {
                if (SumaCasillas(tablero, a, a + 1, a + 2) == 3 || SumaCasillas(tablero, i, i + 3, i + 6) == 3)
                    return 1;
                else if (SumaCasillas(tablero, a, a + 1, a + 2) == 0 || SumaCasillas(tablero, i, i + 3, i + 6) == 0)
                    return 0;

                a += 3;
            }
            empate = true;
            foreach (int casilla in tablero)
            {
                if (casilla == 0 || casilla == 1)
                    empate = empate && true;
                else
                    empate = empate && false;
            }
            if (empate)
                return 2;

            return -1;
        }
        public static bool QuienJuegaPrimero(int monedaAlAire)
        {
            int moneda;
            Console.WriteLine("Vamos a lanzar una moneda para ver quien va primero...\n" +
                "Elige 0 para cara o 1 para sello");
            moneda = int.Parse(Console.ReadLine());
            while (moneda != 0 && moneda != 1)
            {
                Console.WriteLine("Esta opción no es valida..." +
                    " \n por favor elige 0 para Cara o 1 para Sello");
                moneda = int.Parse(Console.ReadLine());
            }
            if (moneda == monedaAlAire)
                return true;
            else
                return false;
        }
        public static int ResultadoCoins(int apuesta, bool victoria)
        {
            if (victoria)
                return apuesta;
            else
                return -apuesta;
        }
        public static int VerificarCoins(int platziCoins, int opcion)
        {
            int volverAJugar;
            if (platziCoins > 1)
            {
                Console.WriteLine($"\nAhora tienes {platziCoins - 1} platziCoins \n" +
                    "Si deseas volver a jugar presiona 1, si deseas ir al menú presiona 0");
                volverAJugar = int.Parse(Console.ReadLine());
                if (volverAJugar == 1)
                    return opcion;
                else
                    return -1;
            }
            else
                return -2;
        }
        public static int VerificarSolicitud(int platziCoins)
        {

            while (platziCoins > 500 || platziCoins < 1)
            {
                Console.WriteLine($"\n {platziCoins} platziCoins, no es una cantidad válida \n" +
                    "Por favor, selecciona una cantidad entre 1 y 500 platzicoins");
                platziCoins = int.Parse(Console.ReadLine());
            }

            return platziCoins;
        }
        public static int VerificarApuesta(int apuesta, int platziCoins, int valorJuego)
        {
            bool control;
            do
            {
                if (apuesta > platziCoins || apuesta < valorJuego)
                {
                    control = true;
                    Console.WriteLine("Apuesta no valida, ingresa un valor valido para la apuesta\n" +
                        $"Recuerda que debe ser mayor que el valor del juego :{valorJuego} y menor o igual a {platziCoins - 1}, que son los platziCoins que posees");
                    apuesta = int.Parse(Console.ReadLine());
                }
                else
                    control = false;

            } while (control);
            return apuesta;

        }
        public static bool VerificarJugabilidad(int platziCoins, int valorJuego)
        {
            if (platziCoins < valorJuego)
            {
                Console.WriteLine("No tienes suficientes platziCoins para este juego, presiona enter para ir al menú\n");
                Console.ReadLine();
                return true;
            }
            else
                return false;
        }
    }
}
