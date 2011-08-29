﻿using System;

namespace Airplane
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Airplane game = new Airplane())
            {
                game.Run();
            }
        }
    }
#endif
}

