﻿using Engine.Events;
using Engine.UI;

using Utils;
using Serilog;
using Engine;

namespace GirlLife {
    internal static class Program {
        public static void Main(string[] args) {
            Log.Logger = Logger.GetLogger(args.Contains("--debug"), args.Contains("--consoleLog"));

            Location.Current.Value = Location.GetInstance("myRoom");

            Game game = new();
            game.Run();
        }
    }
}