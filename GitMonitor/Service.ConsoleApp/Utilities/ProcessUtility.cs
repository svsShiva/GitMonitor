using System.Diagnostics;

namespace Service.ConsoleApp.Utilities
{
    internal class ProcessUtility
    {
        internal static string ExecuteCommand(string workingDir, string command)
        {
            try
            {
                Process process = new Process();

                process.StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = workingDir,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    Arguments = $"/c" + command,

#if DEBUG
                    CreateNoWindow = false
#else
                    CreateNoWindow = true
#endif

                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                return output;
            }
            catch
            {
                throw;
            }
        }
    }
}