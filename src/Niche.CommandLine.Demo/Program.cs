﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Niche.ConsoleLogging;

namespace Niche.CommandLine.Demo
{
    public static class Program
    {
        private static ConsoleLogger _logger;

        public static int Main(string[] args)
        {
            _logger = new ConsoleLogger(
                ConsoleLoggerOptions.DisplayBanner
                | ConsoleLoggerOptions.UseLabels
                | ConsoleLoggerOptions.ShowTime);

            var processor = new CommandLineProcessor(args)
                .WithErrorAction(ShowErrors)
                .WithHelpAction(ShowHelp);

            return processor.Parse<ProgramOptions>()
                .Execute(MainCore);
        }

        private static int MainCore(ProgramOptions options)
        {
            _logger.Information("Configured foreground: {0}", options.ForegroundColor);

            _logger.Heading("Heading");
            _logger.Action("Action");
            _logger.Information("Information");
            _logger.Detail("Detail");
            _logger.Debug("Debug");
            _logger.Warning("Warning");
            _logger.Success("Success");
            _logger.Failure("Failure");

            if (Debugger.IsAttached)
            {
                Console.ReadLine();
            }

            return 0;
        }

        private static void ShowHelp(IEnumerable<string> help)
        {
            _logger.Information("Available commandline options:");
            _logger.Detail(help);
        }

        private static void ShowErrors(IEnumerable<string> errors)
        {
            _logger.Failure(errors);
        }
    }
}
