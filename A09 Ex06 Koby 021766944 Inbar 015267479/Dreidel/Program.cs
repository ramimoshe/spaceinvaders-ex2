using System;

namespace DreidelGame
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (DreidelGame game = new DreidelGame())
            {
                game.Run();
            }
        }
    }
}

