using System.Diagnostics;
using Backend.API.Services.Contracts;

namespace Backend.API.Services
{
    public class PythonExecuteService : IPythonExecuteService
    {
        private string myScriptsFolder = Path.Combine(AppContext.BaseDirectory, "analysis");

        public async Task<string> Execute(string script="main.py", string arguments="")
        {
            string executablePath = OperatingSystem.IsWindows() ? "python.exe" : "python3";
            string path = Path.Combine(myScriptsFolder, script);

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(executablePath, path)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.StartInfo.EnvironmentVariables["OPENAI_API_KEY"] = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

            if (arguments != string.Empty)
            {
                process.StartInfo.Arguments = arguments;
            }

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            await process.WaitForExitAsync();

            return output;
        }
    }
}
