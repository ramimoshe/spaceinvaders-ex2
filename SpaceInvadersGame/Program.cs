using System;

namespace SpaceInvadersGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SpaceInvadersGame game = new SpaceInvadersGame())
            {
                game.Run();
            }
        }
    }
}

