using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace WLP
{
    /// <summary>
    /// Had to create this class to determine if we have already banned the person - since the EventLog can occasional freeze and then we get 
    /// 2-3 attempts in a single go - don't want duplication in the firewall
    /// </summary>
    public class AdvFirewall
    {
        /*Rule Name:                            DCOM - Block
        ----------------------------------------------------------------------
        Enabled:                              Yes
        Direction:                            In
        Profiles:                             Domain,Private,Public
        Grouping:                             
        LocalIP:                              Any
        RemoteIP:                             Any
        Protocol:                             TCP
        LocalPort:                            135
        RemotePort:                           Any
        Edge traversal:                       No
        Action:                               Block
*/

        private static string GetValueFromLine(string line)
        {
            string[] lineSplit = line.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            if (lineSplit.Length == 1)
            {
                return "";
            }
            return lineSplit[1].Trim();
        }
        public static List<FirewallRule> GetRules()
        {
            string commandLine = "advfirewall firewall show rule name=all";
            ProcessStartInfo psi = new ProcessStartInfo("netsh") { Arguments = commandLine };
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            Process proc = new Process { StartInfo = psi };
            proc.Start();
            proc.WaitForExit(10000);
            string content = proc.StandardOutput.ReadToEnd();
            string[] lines = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            List<FirewallRule> firewalls = new List<FirewallRule>();
            int index = 0;
            FirewallRule rule = null;
            foreach (string line in lines)
            {
                if (line.Contains("Rule Name:"))
                {
                    string rulename = GetValueFromLine(line);
                    rule = new FirewallRule();
                    rule.RuleName = rulename;
                    firewalls.Add(rule);
                    //Start of a rule
                }
                else if (line.Contains("Enabled:"))
                {
                    rule.Enabled = GetValueFromLine(line);
                }
                else if (line.Contains("Direction:"))
                {
                    rule.Direction = GetValueFromLine(line);
                }
                else if (line.Contains("Profiles:"))
                {
                    rule.Profiles = GetValueFromLine(line);
                }
                else if (line.Contains("Grouping:"))
                {
                    rule.Grouping = GetValueFromLine(line);
                }
                else if (line.Contains("LocalIP:"))
                {
                    rule.LocalIp = GetValueFromLine(line);
                }
                else if (line.Contains("RemoteIP:"))
                {
                    rule.RemoteIp = GetValueFromLine(line);
                }
                else if (line.Contains("Protocol:"))
                {
                    rule.Protocol = GetValueFromLine(line);
                }
                else if (line.Contains("LocalPort:"))
                {
                    rule.LocalPort = GetValueFromLine(line);
                }
                else if (line.Contains("RemotePort:"))
                {
                    rule.RemotePort = GetValueFromLine(line);
                }
                else if (line.Contains("Edge traversal:"))
                {
                    rule.EdgeTraversal = GetValueFromLine(line);
                }
                else if (line.Contains("Action:"))
                {
                    rule.Action = GetValueFromLine(line);
                }

            }
            return firewalls;
        }
            
        


        public class FirewallRule
        {
            public string RuleName { get; set; }
            public string Enabled { get; set; }
            public string Direction { get; set; }
            public string Profiles { get; set; }
            public string Grouping { get; set; }
            public string LocalIp { get; set; }
            public string RemoteIp { get; set; }
            public string Protocol { get; set; }
            public string LocalPort { get; set; }
            public string RemotePort { get; set; }
            public string EdgeTraversal { get; set; }
            public string Action { get; set; }
        }
    }
}
