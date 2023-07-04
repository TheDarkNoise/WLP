using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.IO;
namespace WLP.Models
{
    public class IntrusionMonitor
    {
        private EventLog log;
        private static string LogFile = null;
        public IntrusionMonitor(string logFile)
        {
            
            LogFile = logFile;



           

            Thread t = new Thread(Looper);
            t.IsBackground = true;
            t.Start();
        }





        static object lockLogObj = new object();
        public static void WriteLog(string text)
        {
            if(LogFile == null) { return; }
            lock (lockLogObj)
            {
                File.AppendAllLines(LogFile, new string[] { text });
            }
        }

        private void Looper()
        {
            
            DateTime lastTime = DateTime.Now;
            while (true)
            {
                var logs = wevtutil.GetEvents();
                
                // get the entries from when the last loop ran
                logs = logs.Where(e => DateTime.Parse(e.System.TimeCreated.SystemTime) > lastTime).ToList();
                Console.WriteLine(logs.Count + " logs found since "+lastTime.ToString());
                logs = logs.OrderBy(e => DateTime.Parse(e.System.TimeCreated.SystemTime)).ToList();
                List<PossibleIntruder> intruders = new List<PossibleIntruder>();
                foreach (var log in logs)
                {
                    var ip = log.EventData.Data.Text;
                    IPAddress theIp = null;
                    if(ip != null && !ip.StartsWith("127") && IPAddress.TryParse(ip, out theIp))
                    {
                        //Valid Ip
                        PossibleIntruder pi = GetIntruder(ip);
                        if (pi != null)
                        {
                            intruders.Add(pi);
                        }
                    }
                    lastTime = DateTime.Parse(log.System.TimeCreated.SystemTime);
                }
                
                foreach(var pi in intruders)
                {
                    if(pi.Attempts >= 5)
                    {
                        pi.Block();
                    }
                    else
                    {
                        Console.WriteLine("Attempt from:"+pi.IpAddress+" detected...");
                    }
                }


                PruneBanned();
                Thread.Sleep(10000);
            }
        }



        private void ProcessIntrusionAttempt()
        {

        }



        public List<PossibleIntruder> Intruders = new List<PossibleIntruder>();


        private object lockObj = new object();
        private void Log_EntryWritten(object sender, EntryWrittenEventArgs e)
        {
            if (e.Entry.EntryType == EventLogEntryType.FailureAudit)
            {
                TimeSpan ts = DateTime.Now - e.Entry.TimeGenerated;
                if(ts.TotalMinutes > 5)
                {
                    return;
                }
                lock (lockObj)
                {
                    
                    var str = e.Entry.ReplacementStrings;
                    if (str == null || str.Length < 20)
                    {
                        return;
                    }
                    string ip = str[19];
                    if (ip.Length < 4)
                    {
                        return;
                    }
                    if (ip.StartsWith("127"))
                    {
                        return;
                    }

                    PossibleIntruder pi = GetIntruder(ip);

                    if (pi != null && pi.Attempts > 5)
                    {
                        pi.Block();
                        WriteLog(DateTime.Now.ToString()+":"+pi.IpAddress);
                    }
                    PruneBanned();
                }
            }
        }

        private void PruneBanned()
        {

            Intruders = Intruders.Where(e => !e.Banned || (e.Banned && e.ShouldNotPrune())).ToList();
        }

        private PossibleIntruder GetIntruder(string ip)
        {
            var existing = Intruders.FirstOrDefault(e => e.IpAddress == ip);
            if(existing == null)
            {
                existing = new PossibleIntruder { IpAddress = ip, Attempts = 0 };
                Intruders.Add(existing);
            }
            existing.Attempts++;
            return existing;
        }


        

    }
}
