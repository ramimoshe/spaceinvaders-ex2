using System;

namespace A09_Ex02_Koby_021766944_Inbar_015267479
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

