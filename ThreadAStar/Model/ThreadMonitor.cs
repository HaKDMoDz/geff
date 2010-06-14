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
        volatile public List<TimelineData> ListTimeLineData;// { get; set; }
        volatile public Dictionary<String, ThreadData> ListThreadData;// { get; set; }

        private UCMonitoring _ucMonitoring;
        private BackgroundWorker _backgroundWorker;
        private Boolean _cancelMonitoring = false;
        private TimeSpan _lastRefresh;
        private Int16 _refreshRate;
        private List<String> _listInstanceThread;

        public ThreadMonitor(UCMonitoring ucMonitoring)
        {
            this.ListTimeLineData = new List<TimelineData>();
            this.ListThreadData = new Dictionary<String, ThreadData>();
            _refreshRate = 500;
            _ucMonitoring = ucMonitoring;

            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
        }

        public void StartMonitoring()
        {
            RefreshThreadInstanceNames();

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
                RefreshThreadInstanceNames();

                SurveyThreads();

                _ucMonitoring.RefreshGraph();
                
                Thread.Sleep(_refreshRate);
            }
        }

        private void RefreshThreadInstanceNames()
        {
            //--- Détermine les instances de thread de l'application
            PerformanceCounterCategory perfCategorie = new PerformanceCounterCategory("Thread");
            string[] instanceNames = perfCategorie.GetInstanceNames();

            _listInstanceThread = new List<string>();
            _listInstanceThread.AddRange(instanceNames.ToList().FindAll(s => s.StartsWith(Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName))));
            //---
        }

        private void SurveyThreads()
        {
            Random rnd = new Random();

            foreach (string threadInstanceName in _listInstanceThread)
            {
                ThreadData threadData = new ThreadData();

                if (ListThreadData.ContainsKey(threadInstanceName))
                {
                    threadData = ListThreadData[threadInstanceName];
                }
                else
                {
                    threadData.CPUMin = 255;
                    threadData.CPUMax = 0;

                    ListThreadData.Add(threadInstanceName, threadData);

                    threadData.PerformanceCounter = new PerformanceCounter("Thread", "% Processor time", threadInstanceName);
                    threadData.PerformanceCounter.BeginInit();
                }

                threadData.CountRefresh++;

                byte percentProcessorTime = (byte)threadData.PerformanceCounter.NextValue();

                if (percentProcessorTime < threadData.CPUMin)
                    threadData.CPUMin = percentProcessorTime;

                if (percentProcessorTime > threadData.CPUMax)
                    threadData.CPUMax = percentProcessorTime;

                //TODo : calcul de la moyenne sur un flottant
                //threadData.CPUAverage = (byte)(((short)threadData.CPUAverage * (threadData.CountRefresh - 1) + (short)percentProcessorTime) / threadData.CountRefresh);
                threadData.CPUAverage = percentProcessorTime;

                ListThreadData[threadInstanceName] = threadData;
            }
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
