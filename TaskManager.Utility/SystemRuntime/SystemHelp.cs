using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualBasic.Devices;

namespace TaskManager.Utility
{
    public  class SystemHelp
    {
        readonly PerformanceCounter cpu;

        readonly ComputerInfo cinf;

        public SystemHelp()
        {
            this.cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            this.cinf = new ComputerInfo();
        }

        public  double GetCpuPercent()
        {
            var percentage = this.cpu.NextValue();
            return Math.Round(percentage, 2, MidpointRounding.AwayFromZero);
        }

        public double GetMemoryPercent()
        {
            var usedMem = this.cinf.TotalPhysicalMemory - this.cinf.AvailablePhysicalMemory;//总内存减去可用内存
            return Math.Round((double)(usedMem / Convert.ToDecimal(this.cinf.TotalPhysicalMemory) * 100),2,MidpointRounding.AwayFromZero);
        }
    }
}
