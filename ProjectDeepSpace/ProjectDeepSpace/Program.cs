using System;

namespace ProjectDeepSpace
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (DeepSpace game = new DeepSpace())
            {
                game.Run();
            }
        }
    }
#endif
}

