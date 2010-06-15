using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;
using System.Management;
using ThreadAStar.UC;
using System.Reflection;
using System.IO;

namespace ThreadAStar.Model
{
    public class ThreadMonitor
    {
        private UCMonitoring _ucMonitoring;
        private BackgroundWorker _backgroundWorker;
        private Boolean _cancelMonitoring = false;
        private TimeSpan _lastRefresh;
        private Int16 _refreshRate;
        private TimeSpan _firstRefresh;
        private List<int> _listPreviousThread;
        private PerformanceCounter perfCounterCPU;

        public List<TimelineData> ListTimeLineData { get; set; }

        public Int32 ElapsedTimePart
        {
            get
            {
                return (Int32)_lastRefresh.Subtract(_firstRefresh).TotalMilliseconds / _refreshRate;
            }
        }

        public ThreadMonitor(UCMonitoring ucMonitoring)
        {
            this.ListTimeLineData = new List<TimelineData>();
            _refreshRate = 50;
            _ucMonitoring = ucMonitoring;

            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
        }

        public void StartMonitoring()
        {
            perfCounterCPU = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);

            ListTimeLineData = new List<TimelineData>();
            _listPreviousThread = new List<int>();
            _firstRefresh = DateTime.Now.TimeOfDay;
            _lastRefresh = DateTime.Now.TimeOfDay;
            _backgroundWorker.RunWorkerAsync();
        }

        public void StopMonitoring()
        {
            _cancelMonitoring = true;
        }

        private void LoopMonitoring()
        {
            while (!_cancelMonitoring)
            {
                _lastRefresh = DateTime.Now.TimeOfDay;

                //---
                TimelineData timelineData = new TimelineData();

                timelineData.Time = DateTime.Now.TimeOfDay.Subtract(_firstRefresh).TotalMilliseconds / _refreshRate;
                timelineData.CPU = SurveyCPU();
                timelineData.RAM = SurveyRAM();
                SurveyThreads(ref timelineData);

                ListTimeLineData.Add(timelineData);
                //---

                _ucMonitoring.RefreshGraph();

                Thread.Sleep(_refreshRate);
            }
        }

        private byte SurveyCPU()
        {
            byte cpu = 0;

            cpu = (byte)(perfCounterCPU.NextValue() / (float)Environment.ProcessorCount);
            return cpu;
        }

        private long SurveyRAM()
        {
            long ram = 0;

            ram = Process.GetCurrentProcess().PagedMemorySize64;
            return ram;
        }

        private void SurveyThreads(ref TimelineData timelineData)
        {
            List<int> listCurrentThread = new List<int>();

            foreach (ProcessThread processThread in Process.GetCurrentProcess().Threads)
            {
                listCurrentThread.Add(processThread.Id);
            }

            timelineData.CountThreads = (byte)listCurrentThread.Count;
            timelineData.CountNewThreads = (byte)listCurrentThread.Count(t => _listPreviousThread.Contains(t));
            
            if(_listPreviousThread.Count>0)
                timelineData.CountDeadThreads = (byte)(timelineData.CountThreads - timelineData.CountNewThreads);

            _listPreviousThread = listCurrentThread;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            LoopMonitoring();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
    }
}
