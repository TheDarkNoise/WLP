using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace WLP
{
    public class wevtutil
    {

        public static List<Models.Event> GetEvents()
        {



            string commandLine = "qe Microsoft-Windows-RemoteDesktopServices-RdpCoreTS/Operational /q:\"Event / System / EventID = 140\"";
            ProcessStartInfo psi = new ProcessStartInfo("wevtutil") { Arguments = commandLine };
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            Process proc = new Process { StartInfo = psi };
            proc.Start();
            proc.WaitForExit(10000);
            string output = proc.StandardOutput.ReadToEnd();

            // string output = File.ReadAllText(@"C:\Users\baale\Desktop\test.xml");

            string[] lines = output.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            List<Models.Event> foundEvents = new List<Models.Event>();

            XmlSerializer serializer = new XmlSerializer(typeof(Models.Event));
            MemoryStream ms = null;
            foreach (string s in lines)
            {
                
                byte[] data = Encoding.UTF8.GetBytes(s);
                ms = new MemoryStream(data);
                var evnt = serializer.Deserialize(ms) as Models.Event;
                foundEvents.Add(evnt);
                ms.Close();
                ms = null;
            }

            return foundEvents;

        }


    }
}
