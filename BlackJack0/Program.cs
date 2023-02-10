/* BlackJack desde cero y con las nuevas características a incorporar
 *O sea las siguientes:
 1. Que el jugador pueda apostar cierta cantidad de platzicoins
 2. Que dad esa cantidad los pierda o los gane contra la maquina
 3. Que la pantalla se limpie cuando gane

               PLATZINO (Le agregué el tres en raya)
 */

using System.Data.SqlTypes;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

class Methods
{
    public void DibujarTablero(List<int> tablero)
    {
        List<string> tableroAuxiliar = new();
        int index = 0;
        String impresion = "\n";
        foreach(int casilla in tablero)
        {
            if (casilla == 4)
                tableroAuxiliar.Add(Convert.ToString(index+1));
            else if (casilla == 1)
                tableroAuxiliar.Add("X");
            else
                tableroAuxiliar.Add("O");
            index++;
        }
        index = 0;
        foreach(string casilla in tableroAuxiliar)
        {
            if (index == 2 || index == 5 || index == 8)
                impresion = impresion  + casilla  + " \n";
            else
                impresion = impresion + casilla + " | ";
            index++;
        }
        Console.WriteLine(impresion);
    }
    public int SelecionarCasillaCasa(List<int> tablero, int marcaCasa, int marcaJugador,int turnoCasa)
    {
        Random random = new Random();
        int casilla = 4;
        if (marcaCasa == 0 && tablero[4] == 4)
            casilla = 4;
        else if (marcaCasa == 0  && SumaCasillas(tablero, 0, 2, 6) + tablero[8] == 10 && turnoCasa < 3)
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
    public int VerificarCasilla(int casilla, List<int> tablero)
    {
        if(casilla>0 && casilla < 10)
        {
            while (tablero[casilla - 1] == 0 || tablero[casilla - 1] == 1)
            {
                Console.WriteLine("Está casilla no es valida. Digita el número de una casilla libre...\n");
                casilla = int.Parse(Console.ReadLine());
            }
        }
        else
        {
            while (casilla<=0 || casilla > 9)
            {
                Console.WriteLine("Este número no corresponde a ninguna casilla. Los números válidos son los que aparecen en la cuadrícula...\n");
                casilla = int.Parse(Console.ReadLine());
            }
        }
        
        return casilla;
    }
    public int VerificarCasillaCasaCentral(int casilla, List<int> tablero)
    {
        Random random = new();
        {
            casilla = 2 * random.Next(0, 3)+1;
        }
        return casilla;
    }
    public int VerificarCasillaCasaEsquina(int casilla, List<int> tablero)
    {
        Random random = new();
        {
            casilla = 2* random.Next(0,4);
        }
        return casilla;
    }
    public int SumaCasillas(List<int> tablero, int a, int b, int c)
    {
        return tablero[a]+ tablero[b]+ tablero[c];
    }
    public int BuscarCasilla(List<int> tablero, int a, int b, int c)
    {
        for (int i = a; i <= c; i += b-a)
        {
            if (tablero[i] == 4)
                return i;
        }
        return -1;
    }
    public int BuscarCasillaEstrategica(List<int> tablero, int a, int b, int c, int marcaCasa)
    {
        List<int> posiblesCasillas = new List<int>();
        List<int> tableroPrueba = tablero.ToList();
        Random random = new();
        for(int i = a; i <= c; i += b - a)
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
    public int CompletarTriqui(List<int> tablero, int marcaCasa)
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
    public int GanarTriqui(List<int> tablero, int marcaCasa)
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
    public bool EsEstrategicaLaCasilla(List<int> tablero, int marcaCasa)
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
    public int VerificarTriqui(List<int> tablero)
    {
        int a = 0;
        bool empate;
        if (SumaCasillas(tablero, 0, 4, 8) == 3 || SumaCasillas(tablero, 2, 4, 6) == 3)
            return 1;
        else if (SumaCasillas(tablero, 0, 4, 8) == 0 || SumaCasillas(tablero, 2, 4, 6) == 0)
            return 0;

        for (int i =0; i < 3; i++)
        {
            if (SumaCasillas(tablero, a, a + 1, a + 2) == 3 || SumaCasillas(tablero, i, i + 3, i + 6) == 3)
                return 1;
            else if (SumaCasillas(tablero, a, a + 1, a + 2) == 0 || SumaCasillas(tablero, i, i + 3, i + 6) == 0)
                return 0;

            a += 3;
        }
        empate = true;
        foreach(int casilla in tablero)
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
    public bool QuienJuegaPrimero(int monedaAlAire)
    {
        int moneda;
        Console.WriteLine("Vamos a lanzar una moneda para ver quien va primero...\n" +
            "Elige 0 para cara o 1 para sello");
        moneda = int.Parse(Console.ReadLine());
        while(moneda!=0 && moneda != 1)
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
	public int ResultadoCoins(int apuesta, bool victoria)
	{
		if (victoria)
			return apuesta;
		else
			return -apuesta;
	}
    public int VerificarCoins(int platziCoins, int opcion)
    {
        int volverAJugar;
        if (platziCoins > 1)
        {
            Console.WriteLine($"\nAhora tienes {platziCoins-1} platziCoins \n"+
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
    public int VerificarSolicitud(int platziCoins)
    {
        
        while (platziCoins > 500 || platziCoins <1)
        {
            Console.WriteLine($"\n {platziCoins} platziCoins, no es una cantidad válida \n" +
                "Por favor, selecciona una cantidad entre 1 y 500 platzicoins");
            platziCoins = int.Parse(Console.ReadLine());
        }
        
            return platziCoins;
    }
    public int VerificarApuesta(int apuesta, int platziCoins, int valorJuego)
    {
        bool control;
        do
        {
            if (apuesta > platziCoins || apuesta < valorJuego )
            {
                control = true;
                Console.WriteLine("Apuesta no valida, ingresa un valor valido para la apuesta\n" +
                    $"Recuerda que debe ser mayor que el valor del juego :{valorJuego} y menor o igual a {platziCoins-1}, que son los platziCoins que posees");
                apuesta = int.Parse(Console.ReadLine());
            }
            else
                control = false;
            
        } while (control);
        return apuesta;
        
    }
    public bool VerificarJugabilidad(int platziCoins, int valorJuego)
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
class Program
{
	static void Main(string[] args)
	{
        int puntajeJugador = 0;
        int puntajeCasa = 0;
        int platziCoins = 1;
        int solicitudPlatziCoins = 0;
        int apuesta = 0;
        Methods metodos = new Methods();
        Random rnd = new();
        int opcion = 0;
        List<string> juegos = new()
            {
                "Pedir más Platzicoins",
                "21",
                "Tres en raya"
            };

        while (platziCoins != 0)
        {
            switch (opcion)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("Bienvenido a P L A T Z I N O \n ");
                    switch (solicitudPlatziCoins)
                    {
                        case 0:
                            Console.WriteLine("¿Cuántos platzicoins deseas? " +
                                "Recuerda que debes apostar cierta cantidad en cada juego.\n " +
                                "La apuesta mínima es de 50 y puedes solicitar máximo 500 cada vez.\n" +
                                "Podrás solicitar platziCoins 3 veces más");
                            platziCoins += int.Parse(Console.ReadLine());
                            platziCoins = metodos.VerificarSolicitud(platziCoins-1)+1;
                            break;
                        case 1:
                        case 2:
                        case 3:
                            Console.WriteLine($"Tienes {platziCoins - 1} platzicoins \n" +
                           $"Solo puedes solicitar platzicoins {4 - solicitudPlatziCoins} veces más\n" +
                           "Recuerda que la apuesta mínima es de 50 y puedes solicitar máximo 500 cada vez.\n" +
                           $" ¿Cuántos deseas?");
                            
                            platziCoins += metodos.VerificarSolicitud(int.Parse(Console.ReadLine()));
                            break;
                        default:
                            Console.WriteLine("¿Más Platzicoins? " +
                               "Lo siento, has superado el límite de solicitudes \n");
                            if (platziCoins == 1)
                            {
                                Console.WriteLine("Ya no puedes jugar más\n");
                                platziCoins--;
                            }
                            else
                            {
                                Console.WriteLine("Presiona Enter para volver al menu...");
                                Console.ReadLine();
                                opcion = -1;
                            }
                                 
                            break;
                        
                    }
                    solicitudPlatziCoins++;
                    
                    opcion = -1;
                    break;

                case -1:
                    Console.Clear();
                    if (platziCoins == 1)
                    {
                        Console.WriteLine("No tienes platziCoins, recuerda que debes tenerlos para ingresar a un juego," +
                            "\n presiona enter para ver si te dan más...");
                        Console.ReadLine();
                        opcion = 0; break;
                    }
                    else
                    {
                        Console.WriteLine($"\n\t------MENÚ--------\n" +
                            $"Tienes {platziCoins - 1} platziCoins\n");
                        Console.WriteLine("¿Qué juego deseas probar? \n");
                        foreach (string juego in juegos)
                        {
                            Console.WriteLine($"\t{juegos.IndexOf(juego)}. {juego}");
                        }
                        Console.WriteLine("\n Digita el numero correspondiente al juego que deseas");
                        opcion = int.Parse(Console.ReadLine());
                        
                    }
                        break;
                    

                case -2:
                    int continuar;
                    Console.WriteLine("\n Haz perdido todos tus platziCoins, si quieres intentar obetener más presiona 1, si no presiona 0");
                    continuar = int.Parse(Console.ReadLine());
                    if (continuar == 1)
                        opcion = 0;
                    else
                        platziCoins = 0;
                    Console.Clear();
                    break;
                case 1:
                    if (metodos.VerificarJugabilidad(platziCoins - 1, 50))
                    {
                        opcion = -1;
                        break;
                    }
                    Console.Clear();
                    puntajeJugador = 0;
                    puntajeCasa = 0;
                    Console.WriteLine("BIENVENIDO AL JUEGO DE 21\n " +
                        $"Tienes {platziCoins-1} platziCoins para jugar. " +
                        "En este juego debes apostar mínimo 50 platziCoins\n" +
                        "Comencemos...\n" +
                        "¿Cuánto deseas apostar?");
                    apuesta = int.Parse(Console.ReadLine());
                    apuesta = metodos.VerificarApuesta(apuesta, platziCoins, 50);
                    int carta;
                    string otraCarta;
                    do
                    {
                        carta = rnd.Next(1, 12);
                        puntajeJugador += carta;
                        Console.WriteLine($"Toma una carta: {carta}" +
                            $"\n ¿Deseas otra carta?");
                        otraCarta = Console.ReadLine();
                    } while ((otraCarta == "si" || otraCarta == "Si"
                             || otraCarta == "yes") && puntajeJugador < 21);
                    puntajeCasa = rnd.Next(14, 22);

                    if (puntajeJugador > 21)
                    {
                        Console.WriteLine($"Perdiste, te pasaste de 21 \n " +
                            $"| Tu puntaje: {puntajeJugador} | Puntaje Casa: {puntajeCasa} ");
                        platziCoins += metodos.ResultadoCoins(apuesta, false);
                    }
                    else if (puntajeCasa > 21 || puntajeJugador > puntajeCasa)
                    {
                        Console.WriteLine("Ganaste  "+
                            $"| Tu puntaje: {puntajeJugador} | Puntaje Casa: {puntajeCasa} ");
                        platziCoins += metodos.ResultadoCoins(apuesta, true);
                    }
                    else
                    {
                        Console.WriteLine("Perdiste  "+
                            $"| Tu puntaje: {puntajeJugador} | Puntaje Casa: {puntajeCasa} ");
                        platziCoins += metodos.ResultadoCoins(apuesta, false);

                    }
                    opcion = metodos.VerificarCoins(platziCoins, opcion);
                    
                    break;

                case 2:
                    
                    if (metodos.VerificarJugabilidad(platziCoins - 1, 200))
                    {
                        opcion = -1;
                        break;
                    }
                    Console.Clear();
                    Console.WriteLine("BIENVENIDO AL TRES EN RAYA\n " +
                       $"Tienes {platziCoins - 1} platziCoins para jugar. " +
                       "En este juego debes apostar mínimo 200 platziCoins\n" +
                       "Comencemos...\n" +
                       "¿Cuánto deseas apostar?");
                    apuesta = int.Parse(Console.ReadLine());
                    apuesta = metodos.VerificarApuesta(apuesta, platziCoins, 200);
                    int seleccionJugador;
                    int seleccionCasa;
                    int marcaJugador = 0;
                    int marcaCasa = 0;
                    int juegoActivo = -1;
                    bool jugadorPrimero;
                    int turnoCasa = 0;
                    List<int> tablero = new() { 4, 4, 4, 4, 4, 4, 4, 4, 4 };

                    jugadorPrimero = metodos.QuienJuegaPrimero(rnd.Next(0, 1));
                    if (jugadorPrimero)
                    {
                        marcaJugador = 1;
                        Console.WriteLine("¡VAS PRIMERO!\n TÚ marcaras la X \n");
                    }
                    else
                    {
                        marcaCasa = 1;
                        turnoCasa = 1;
                        Console.WriteLine("¡VA PRIMERO LA CASA! \nTÚ marcaras la O \n");
                        tablero[metodos.SelecionarCasillaCasa(tablero, marcaCasa, marcaJugador,turnoCasa)] = marcaCasa;
                    }
                    while (juegoActivo == -1)
                    {
                        
                        metodos.DibujarTablero(tablero);
                        Console.WriteLine("Elige una casilla por su número, " +
                            "cuando se ocupe aparecera una X o una O, y ya no será seleccionable \n");
                        seleccionJugador = int.Parse(Console.ReadLine());
                        seleccionJugador = metodos.VerificarCasilla(seleccionJugador, tablero);
                        tablero[seleccionJugador-1] = marcaJugador;
                        juegoActivo = metodos.VerificarTriqui(tablero);
                        if (juegoActivo != -1)
                            break;
                        Console.WriteLine("La casa elige... ");
                        turnoCasa += 1;
                        seleccionCasa = metodos.SelecionarCasillaCasa(tablero, marcaCasa, marcaJugador, turnoCasa);
                        tablero[seleccionCasa] = marcaCasa;
                        juegoActivo = metodos.VerificarTriqui(tablero);
                    }

                    metodos.DibujarTablero(tablero);

                    if (juegoActivo == marcaJugador)
                    {
                        Console.WriteLine("Ganaste ");
                        platziCoins += metodos.ResultadoCoins(apuesta, true);
                    }
                    else if(juegoActivo == marcaCasa)
                    {
                        Console.WriteLine("Perdiste ");
                        platziCoins += metodos.ResultadoCoins(apuesta, false);
                    }
                    else
                    {
                        Console.WriteLine($"Empataste, ");
                    }
                        
                    opcion = metodos.VerificarCoins(platziCoins, opcion);
                    break;

                default:
                    Console.WriteLine("Opción no valida, oprime enter para volver a elegir");
                    Console.ReadLine();
                    opcion = -1;
                    Console.Clear();
                    Console.WriteLine("Bienvenido a P L A T Z I N O \n ");
                    break;
            }

        }
        Console.WriteLine($"Adios");
    }
}
