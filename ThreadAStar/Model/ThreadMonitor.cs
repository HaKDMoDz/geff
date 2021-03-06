﻿using System;
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
        private List<int> _listThreadToIgnore;
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
            _ucMonitoring = ucMonitoring;

            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
        }

        public void StartMonitoring(short refreshRate)
        {
            _cancelMonitoring = false;
            _refreshRate = refreshRate;

            perfCounterCPU = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);

            ListTimeLineData = new List<TimelineData>();
            _listPreviousThread = new List<int>();
            _firstRefresh = DateTime.Now.TimeOfDay;
            _lastRefresh = DateTime.Now.TimeOfDay;

            //--- Liste des threads en cours.
            //    ces threads sont à exclure
            _listThreadToIgnore = new List<int>();
            foreach (ProcessThread processThread in Process.GetCurrentProcess().Threads)
            {
                _listThreadToIgnore.Add(processThread.Id);
            }
            //---

            if(!_backgroundWorker.IsBusy)
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
                if(!_listThreadToIgnore.Contains(processThread.Id))
                    listCurrentThread.Add(processThread.Id);
            }

            timelineData.CountThreads = (byte)listCurrentThread.Count;
            timelineData.CountNewThreads = (byte)listCurrentThread.Count(t => !_listPreviousThread.Contains(t));
            timelineData.CountDeadThreads = (byte)_listPreviousThread.Count(t => !listCurrentThread.Contains(t));

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
