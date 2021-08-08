using EnsoulSharp.SDK;

namespace EnsoulAbarca
{
    public class Program
    {
        public static void Main()
        {
            GameEvent.OnGameLoad += MainCheat.OnGameLoad;
        }
    }
}
