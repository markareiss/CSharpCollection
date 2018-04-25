using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace TestPS
{
    public class PsAnalyzer
    {
        public string Path { get; set; }
        public string FuctionName { get; set; }

        public List<string> ParameterNames { get; set; }
        public List<string> Arguments { get; set; }

        public Collection<PSObject> PSOutput { get; set; }

        private PowerShell ps;
        private string textFromPsFile;

        public PsAnalyzer(string path, List<string> psArguments)
        {
            Path = path;
            Arguments = psArguments;

            textFromPsFile = GetTextFromPsFile(Path);

            startScript();
            loadFunction();
            loadParamsAndArgs();
            PSOutput = runScript();

            convertPSObjectToList();   
        }

        private void convertPSObjectToList()
        {
            foreach (var psOut in PSOutput)
            {
                foreach (var item in psOut.Properties)
                {
                    var tempType = item.TypeNameOfValue;
                    var tempName = item.Name;
                    var tempValue = item.Value;
                }                
            }
        }

        private Collection<PSObject> runScript()
        {
            var output = ps.Invoke();
            return output;
        }

        private void loadParamsAndArgs()
        {
            GetParameterList(textFromPsFile);

            for (int i = 0; i < ParameterNames.Count; i++)
            {
                ps.AddParameter(ParameterNames[i], Arguments[i]);
            }
        }

        private void loadFunction()
        {
            FuctionName = GetFunctionName(textFromPsFile);
            ps.AddCommand(FuctionName);
        }

        private void startScript()
        {
            ps = PowerShell.Create();
            ps.AddScript(textFromPsFile).Invoke();
        }

        private string GetTextFromPsFile(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                var ps1AsText = sr.ReadToEnd();
                return ps1AsText;
            }
        }

        private string GetFunctionName(string psAsText)
        {
            string FUNCTION = "Function ";
            string CURLY = "{";

            var startIndex = psAsText.IndexOf(FUNCTION) + FUNCTION.Length;
            var endIndex = psAsText.IndexOf(CURLY);

            var functionName = psAsText.Substring(startIndex, endIndex - startIndex);

            return functionName;
        }

        private void GetParameterList(string psAsText)
        {
            string PARAM = "Param";
            char LEFTPARANTH = '(';
            char RIGHTPARANTH = ')';
            char DOLLAR = '$';

            var paramIndex = psAsText.IndexOf(PARAM) + PARAM.Length;

            var leftParanthIndex = psAsText.IndexOf(LEFTPARANTH, paramIndex);
            var rightParanthIndex = psAsText.IndexOf(RIGHTPARANTH, paramIndex);

            bool containsParameter = psAsText.Substring(leftParanthIndex, rightParanthIndex - leftParanthIndex).Contains(DOLLAR);

            if (containsParameter) {
                ParameterNames = new List<string>();
            }

            var tempIndex = paramIndex;

            while (containsParameter)
            {
                var parameterStartIndex = psAsText.IndexOf(DOLLAR, tempIndex);

                containsParameter = psAsText.Substring(parameterStartIndex + 1 , rightParanthIndex - tempIndex).Contains(DOLLAR);

                string parameterName = "";

                if (containsParameter)
                {
                    var parameterEndIndex = psAsText.IndexOf(',', parameterStartIndex);
                    parameterName = psAsText.Substring(parameterStartIndex, parameterEndIndex - parameterStartIndex).TrimStart('$').Trim();

                    tempIndex = psAsText.IndexOf(parameterName, tempIndex) + parameterName.Length;
                }
                else
                {
                    parameterName = psAsText.Substring(parameterStartIndex, rightParanthIndex - parameterStartIndex).TrimStart('$').Trim();
                }

                ParameterNames.Add(parameterName);
            }
        }
    }
}
