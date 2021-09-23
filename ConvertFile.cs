using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NGS.ReportingDTOs;
using Newtonsoft.Json;
using NGS.ReportingDtoTests.CGW;
using NGS.ReportingDTOs.AutoRep.TSO500;
using NGS.ReportingDTOs.CGW.CleanV3DtoModels;

namespace Pierian_Converter
{
    public static class ConvertFile
    {
        private static string directory = string.Empty;
        private static string fileName = string.Empty;
        private static string writeDirectory = "C:\\Temp";
        public static void ConvertPierianFile(string directory, string fileName, string writeDirectory)
        {
            Log.Information("Hello World");
            //directory = "C:\\Users\\25416\\Source\\Repos\\Project Notes\\Perian Project\\2021-08-31Examples\\";
            //fileName = "Example 15 fullrun_1_14.json";
            //writeDirectory = "C:\\Temp";
            ReadPierianFile($"{directory}{fileName}");
        }


        private static bool ReadPierianFile(string filePath)
        {
            var jsonText = File.ReadAllText(filePath);
            var cgwData = new CgwV3Data();
            var cgwDto = cgwData.V3StringToCgwDTO(jsonText);
            var cleanV3DTO = new CleanV3DTO(cgwDto);
            // EELRS Conversion
            EelrsDto eelrsDto = new EelrsDto();
            eelrsDto.LoadFromCleanV3Dto(cleanV3DTO);
            var eelrsText = JsonConvert.SerializeObject(eelrsDto);
            // Millennium Conversion.
            MillenniumDto millenniumDto = new MillenniumDto(eelrsDto);
            var text = millenniumDto.ProduceBenchscaleFile("oooo", "aaa", DateTime.Now.ToString());
            var millenniumText = JsonConvert.SerializeObject(millenniumDto);
            try
            {
                File.WriteAllText($"{writeDirectory}\\EELRS.json", eelrsText);
                File.WriteAllText($"{writeDirectory}\\Millenniym.json", millenniumText);
                File.WriteAllText($"{writeDirectory}\\BenchScaleFile.txt", text);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return false;
        }
    }
}
