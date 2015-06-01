using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteMonitor
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            Thread tMonitor = new Thread(startMonitor);
            tMonitor.Start();  
        }

        private void startMonitor()
        {
            MachineMonitor machineMonitor = new MachineMonitor("B-NTIDDT001");
            while (true)
            {
                
                var ram = "Mem RAM: " + machineMonitor.GetUsageMemoryPercentage() + " %";
                var cpu = "CPU: " + machineMonitor.GetUsageCPUPercentage() + " %";

                if (this.lblRam.InvokeRequired)
                {
                    this.lblRam.Invoke((MethodInvoker)delegate() { this.lblRam.Text = ram; });
                }
                else
                {
                    this.lblRam.Text = ram;
                }
                //
                if (this.lblCpu.InvokeRequired)
                {
                    this.lblCpu.Invoke((MethodInvoker)delegate() { this.lblCpu.Text = cpu; });
                }
                else
                {
                    this.lblCpu.Text = cpu;
                }
                Thread.Sleep(1000);
            }
        }
    }
}
