using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreadAStar.Threading;
using System.Management;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace ThreadAStar.Model
{
    public class ThreadManagerDotNet4
    {
        /*public List<ThreadingBaseMethod> ListThread { get; set; }
        public Int32 NombreThread { get; set; }
        public TypeThreading TypeThreading { get; set; }
        public List<Map> ListMap { get; set; }
        public Int32 NombreMapCalculee { get; set; }

        public ThreadManager(int nombreThread, TypeThreading typeThreading, List<Map> listMap)
        {
            this.NombreThread = nombreThread;
            this.TypeThreading = typeThreading;
            this.ListMap = listMap;

            ListThread = new List<ThreadingBaseMethod>();
        }

        public ThreadingBaseMethod CreateThreads(Map map)
        {
            ThreadingBaseMethod threadingMethod = null;

            switch (this.TypeThreading)
            {
                case TypeThreading.Natif:
                    threadingMethod = new ThreadingNativeMethod(this, map);
                    break;
                case TypeThreading.BackgroundWorker:
                    threadingMethod = new BackGroundWorkerMethod(this, map);
                    break;
                case TypeThreading.ParallelFX:
                    break;
                case TypeThreading.DotNet4:
                    break;
                default:
                    break;
            }

            this.ListThread.Add(threadingMethod);

            return threadingMethod;
        }

        public void Start()
        {
            for (int i = 0; i < NombreThread; i++)
            {
                if (NombreMapCalculee < this.ListMap.Count)
                {
                    ThreadingBaseMethod newThreadingMethod = CreateThreads(this.ListMap[NombreMapCalculee]);
                    newThreadingMethod.Start();
                    NombreMapCalculee++;
                }
            }


            while (!allCalculCompleted)
            {
                t = 0;

                //foreach (ThreadingBaseMethod threadingMethod in ListThread)
                foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
                {

                    //---
                    ConnectionOptions oConn;
                    ManagementScope oMs;

                    oConn = new ConnectionOptions();
                    oConn.Impersonation = ImpersonationLevel.Impersonate;
                    oConn.Authentication = AuthenticationLevel.PacketPrivacy;

                    oMs = new ManagementScope();//"\\\\" + Environment. + "\\root\\cimv2", oConn);

                    string sQuery = String.Format("SELECT * FROM win32_PerfFormattedData_PerfProc_Thread WHERE IdProcess={0} and IdThread={1}",
                        Process.GetCurrentProcess().Id, thread.Id);

                    ObjectQuery oQuery = new ObjectQuery(sQuery);
                    ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
                    ManagementObjectCollection oReturnCollection = oSearcher.Get();


                    foreach (ManagementObject oReturn in oReturnCollection)
                    {
                        PropertyDataCollection retProperties = oReturn.Properties;

                        ulong a = (ulong)retProperties["PercentProcessorTime"].Value;
                        t += a;

                    }
                    //---
                }

                Form1.frm.Text = t.ToString();
                Application.DoEvents();
                Thread.Sleep(10);
            }
        }

        public ulong t = 0;

        public void CalculCompleted(ThreadingBaseMethod threadingMethod)
        {
            this.ListThread.Remove(threadingMethod);

            if (NombreMapCalculee < this.ListMap.Count)
            {
                ThreadingBaseMethod newThreadingMethod = CreateThreads(this.ListMap[NombreMapCalculee]);
                newThreadingMethod.Start();
                NombreMapCalculee++;
            }
            else
            {
                AllCalculCompleted();
            }
        }

        public bool allCalculCompleted = false;

        public void AllCalculCompleted()
        {
            int a = 0;
            allCalculCompleted = true;
        }

        public void Stop()
        {
        }*/
    }
}
