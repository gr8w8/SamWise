using Serilog;
using System;
using System.Linq;

namespace Pierian_Converter
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
            Log.Information("Application Started");

            ConvertFile.ConvertPierianFile(args.First(), args[1], args.Last());

        }
    }
}
