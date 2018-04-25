using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.IO;

namespace TestPS
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            string argumentToFeed = "Mark";
            string ps1AsText = GetTextFromPs1File(@"M:\Projects\TestScaffold\TestPS\PSScripts\Get-Test.ps1");
            //string ps1AsText = GetDetailsFromPs1File(@"M:\Projects\TestScaffold\TestPS\PSScripts\Get-Test.ps1");

            var functionName = GetFunctionName(ps1AsText);
            var parameterName = GetParameterList(ps1AsText);
            

            List<string> psArgs = new List<string> { "Mark", "34" } ;

            PsAnalyzer psAnalyzer = new PsAnalyzer(@"M:\Projects\TestScaffold\TestPS\PSScripts\Get-Test.ps1", psArgs);
            */
            List<string> psArgs2 = new List<string> { "Chrome" };

            PsAnalyzer psAnalyzer2 = new PsAnalyzer(@"M:\Projects\TestScaffold\TestPS\PSScripts\Get-Procs.ps1", psArgs2);


            //PowerShell ps = PowerShell.Create();
            /*
            ps.AddScript(ps1AsText)
                .Invoke();

            var results = ps
                .AddCommand(functionName)
                .AddParameter(parameterName,argumentToFeed)
                .Invoke();
            */

            foreach (var item in psAnalyzer2.PSOutput)
            {
                Console.WriteLine(item);
            }

            /*
            foreach (var item in psAnalyzer.PSOutput)
            {
                Console.WriteLine(item);
            }            
            */

            Console.ReadLine();
        }

        static private string GetTextFromPs1File(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                var ps1AsText = sr.ReadToEnd();
                return ps1AsText;
            }
        }


        static private string GetDetailsFromPs1File(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                var ps1AsText = sr.ReadToEnd();

                var functionName = GetFunctionName(ps1AsText);
                var parameterName = GetParameterList(ps1AsText);

                return ps1AsText;
            }
        }

        private static string GetParameterList(string ps1AsText)
        {
            string PARAM = "Param";
            char LEFTPARANTH = '(';
            char RIGHTPARANTH = ')';
            char DOLLAR = '$';

            var paramIndex = ps1AsText.IndexOf(PARAM) + PARAM.Length;
            var leftParanthIndex = ps1AsText.IndexOf(LEFTPARANTH, paramIndex);
            var rightParanthIndex = ps1AsText.IndexOf(RIGHTPARANTH, paramIndex);
            var parameterStartIndex = ps1AsText.IndexOf(DOLLAR, paramIndex);

            var parameterName = ps1AsText.Substring(parameterStartIndex, rightParanthIndex - parameterStartIndex).TrimStart('$').Trim();

            return parameterName;

        }

        static private string GetFunctionName(string ps1AsText)
        {
            string FUNCTION = "Function ";
            string CURLY = "{";

            var startIndex = ps1AsText.IndexOf(FUNCTION) + FUNCTION.Length;
            var endIndex = ps1AsText.IndexOf(CURLY);

            var functionName = ps1AsText.Substring(startIndex, endIndex - startIndex);

            return functionName;
        }


    }
}



/*
TODO
    PsCollectionToList ?
    Create Model?
    Create Controller?
    Create View?
    Create Post?
    Create ViewResult?

Other
    Ps1 to Exe ?

*/
