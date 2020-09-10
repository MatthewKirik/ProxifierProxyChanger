using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace AutoProxyChanger
{
    public class ChangeProxyJob : IJob
    {
        public static int profileIndex = 1;
        public async Task Execute(IJobExecutionContext context)
        {
            string currentProfile = $"{ConfigurationManager.AppSettings["profilesPath"]}profile{profileIndex}.ppx";
            string strCmdText;
            //strCmdText = $"cmd; ping 127.0.0.1 -n 6";
            strCmdText = $"\"\"{ConfigurationManager.AppSettings["proxyfierPath"]}\" \"{currentProfile}\" silent-load\"";
            Console.WriteLine(strCmdText);
            //Console.WriteLine(ConfigurationManager.AppSettings["proxyfierPath"]);
            //System.Diagnostics.Process.Start("CMD.exe", strCmdText);
            ExecuteCommandSync(strCmdText);
            profileIndex++;
            if (profileIndex > int.Parse(ConfigurationManager.AppSettings["proxyAmount"]))
                profileIndex = 1;
        }
        public void ExecuteCommandSync(string command)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                //procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Display the command output.
                Console.WriteLine(result);
            }
            catch (Exception objException)
            {
                // Log the exception
            }
        }
    }
}
