using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Text;

namespace RemoteMonitor
{
    public class MachineMonitor
    {
        private ManagementScope ms = null;
        ConnectionOptions co = null;
        private static MachineMonitor instance;
        private string machine = null;
        ManagementObjectSearcher searcherMemRam = null;
        ManagementObjectSearcher searcherCpu = null;


        public MachineMonitor(string machine, string username = null, string userPassword = null)
        {
            this.machine = machine;

            if (co != null)
                co = null;

            if (ms != null)
                ms = null;

            if (searcherCpu != null)
                searcherCpu = null;

            if (searcherMemRam != null)
                searcherMemRam = null;

            co = new ConnectionOptions();
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(userPassword))
            {
                co.Username = @"UNIVAG\michael.lopes";
                co.Password = "@michel2015";
            }

            ms = new ManagementScope("\\\\" + machine + "\\root\\cimv2", co);
            ms.Options.EnablePrivileges = true;

            ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            searcherMemRam = new ManagementObjectSearcher(ms, wql);

            ObjectQuery wql2 = new ObjectQuery("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor");
            searcherCpu = new ManagementObjectSearcher(ms, wql2);
        }



        public long GetUsageCPUPercentage()
        {
            VerifyMachine();
            long result = 0;
            ManagementObject cpuTimes = searcherCpu.Get().Cast<ManagementObject>().FirstOrDefault();
            result = Convert.ToInt64(cpuTimes["PercentProcessorTime"]);
            return result;
        }

        public long GetUsageMemoryPercentage()
        {
            VerifyMachine();
            ManagementObjectCollection results = searcherMemRam.Get();
            long result = 0;
            foreach (ManagementObject res in results)
            {
                var totalVisibleMemorySize = Convert.ToInt64(res["TotalVisibleMemorySize"]);
                var freePhysicalMemory = Convert.ToInt64(res["FreePhysicalMemory"]);
                result = (100 - ((freePhysicalMemory * 100) / totalVisibleMemorySize));
            }

            return result;
        }



        private void VerifyMachine()
        {
            if (machine == null)
            {
                throw new Exception("Machine Name is not seting.");
            }
        }

    }
}
