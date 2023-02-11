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
using BlackJack0;
class Program
{
	static void Main(string[] args)
	{
        int puntajeJugador = 0;
        int puntajeCasa = 0;
        int platziCoins = 1;
        int solicitudPlatziCoins = 0;
        int apuesta = 0;
        Random rnd = new();
        int opcion = 0;
        List<string> juegos = new()
            {
                "Pedir más Platzicoins",
                "21",
                "Tres en raya",
                "Salir"
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
                            platziCoins = Methods.VerificarSolicitud(platziCoins-1)+1;
                            break;
                        case 1:
                        case 2:
                        case 3:
                            Console.WriteLine($"Tienes {platziCoins - 1} platzicoins \n" +
                           $"Solo puedes solicitar platzicoins {4 - solicitudPlatziCoins} veces más\n" +
                           "Recuerda que la apuesta mínima es de 50 y puedes solicitar máximo 500 cada vez.\n" +
                           $" ¿Cuántos deseas?");
                            
                            platziCoins += Methods.VerificarSolicitud(int.Parse(Console.ReadLine()));
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
                    if (Methods.VerificarJugabilidad(platziCoins - 1, 50))
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
                    apuesta = Methods.VerificarApuesta(apuesta, platziCoins, 50);
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
                        platziCoins += Methods.ResultadoCoins(apuesta, false);
                    }
                    else if (puntajeCasa > 21 || puntajeJugador > puntajeCasa)
                    {
                        Console.WriteLine("Ganaste  "+
                            $"| Tu puntaje: {puntajeJugador} | Puntaje Casa: {puntajeCasa} ");
                        platziCoins += Methods.ResultadoCoins(apuesta, true);
                    }
                    else
                    {
                        Console.WriteLine("Perdiste  "+
                            $"| Tu puntaje: {puntajeJugador} | Puntaje Casa: {puntajeCasa} ");
                        platziCoins += Methods.ResultadoCoins(apuesta, false);

                    }
                    opcion = Methods.VerificarCoins(platziCoins, opcion);
                    
                    break;

                case 2:
                    
                    if (Methods.VerificarJugabilidad(platziCoins - 1, 200))
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
                    apuesta = Methods.VerificarApuesta(apuesta, platziCoins, 200);
                    int seleccionJugador;
                    int seleccionCasa;
                    int marcaJugador = 0;
                    int marcaCasa = 0;
                    int juegoActivo = -1;
                    bool jugadorPrimero;
                    int turnoCasa = 0;
                    List<int> tablero = new() { 4, 4, 4, 4, 4, 4, 4, 4, 4 };

                    jugadorPrimero = Methods.QuienJuegaPrimero(rnd.Next(0, 1));
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
                        tablero[Methods.SelecionarCasillaCasa(tablero, marcaCasa, marcaJugador,turnoCasa)] = marcaCasa;
                    }
                    while (juegoActivo == -1)
                    {
                        
                        Methods.DibujarTablero(tablero);
                        Console.WriteLine("Elige una casilla por su número, " +
                            "cuando se ocupe aparecera una X o una O, y ya no será seleccionable \n");
                        seleccionJugador = int.Parse(Console.ReadLine());
                        seleccionJugador = Methods.VerificarCasilla(seleccionJugador, tablero);
                        tablero[seleccionJugador-1] = marcaJugador;
                        juegoActivo = Methods.VerificarTriqui(tablero);
                        if (juegoActivo != -1)
                            break;
                        Console.WriteLine("La casa elige... ");
                        turnoCasa += 1;
                        seleccionCasa = Methods.SelecionarCasillaCasa(tablero, marcaCasa, marcaJugador, turnoCasa);
                        tablero[seleccionCasa] = marcaCasa;
                        juegoActivo = Methods.VerificarTriqui(tablero);
                    }

                    Methods.DibujarTablero(tablero);

                    if (juegoActivo == marcaJugador)
                    {
                        Console.WriteLine("Ganaste ");
                        platziCoins += Methods.ResultadoCoins(apuesta, true);
                    }
                    else if(juegoActivo == marcaCasa)
                    {
                        Console.WriteLine("Perdiste ");
                        platziCoins += Methods.ResultadoCoins(apuesta, false);
                    }
                    else
                    {
                        Console.WriteLine($"Empataste, ");
                    }
                        
                    opcion = Methods.VerificarCoins(platziCoins, opcion);
                    break;
                case 3:
                    platziCoins = 0;
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
