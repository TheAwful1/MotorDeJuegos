namespace MotorDeJuegos;

class Program
{
    static void Main(string[] args) {

        using (Juego juego = new(500, 500))
        {
            juego.Run();

        }
    }
}