using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Linq;
namespace WLP.Models
{
    public class PossibleIntruder
    {
        public string IpAddress { get; set; }
        public int Attempts { get; set; }

        public bool Banned { get; set; }

        public DateTime BannedTime { get; set; }

        public void Block()
        {
            if (Banned)
            {
                return;
            }
            var existingFirewallRules = AdvFirewall.GetRules();
            var existingForThisIp = existingFirewallRules.FirstOrDefault(e => e.RemoteIp.Contains(this.IpAddress));
            if(existingForThisIp != null)
            {
                //Console.WriteLine("Existing firewall rule already exists for this ip address:"+this.IpAddress);
                return;
            }

            IntrusionMonitor.WriteLog(DateTime.Now.ToString() + ":" + this.IpAddress);
            Console.WriteLine("Adding firewall rule for:"+IpAddress);
            IPAddress ipAddy;
            if (!IPAddress.TryParse(IpAddress, out ipAddy))
            {
                return;
            }
            string commandLine = "advfirewall firewall add rule name=block" + ipAddy.ToString() + " dir=in interface=any action=block remoteip=" + ipAddy.ToString() + "/32";
            ProcessStartInfo psi = new ProcessStartInfo("netsh") { Arguments = commandLine };
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            Process proc = new Process { StartInfo = psi };
            proc.Start();
            proc.WaitForExit(10000);
            Banned = true;
            BannedTime = DateTime.Now;
        }


        public bool ShouldNotPrune()
        {
            TimeSpan ts = DateTime.Now - BannedTime;
            return ts.TotalMinutes < 2;
        }
    }
}
