using EnsoulSharp.SDK;

namespace EnsoulAbarca
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GameEvent.OnGameLoad += MainCheat.OnGameLoad;
        }
    }
}
