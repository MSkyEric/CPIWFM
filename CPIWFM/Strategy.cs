using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CPIWFM
{
    abstract class Strategy
    {
        public string sMessage;
        public abstract void AlgorithmForTheStrategy();
    }
    class CPIWFMStrategy : Strategy
    {
        public int iNumberOfPerson { get; set; }
        
        public List<Schedules> oSchedules { get; set; }
        
        public List<DateTime> PerDayTimeRange { get; set; }

        public CPIWFMStrategy(List<Schedules> oSchedules, int iNumberOfPerson)
        {
            this.iNumberOfPerson = iNumberOfPerson;
            this.oSchedules = oSchedules;
            DateTime dtToday = DateTime.Parse("2015-12-14");
            this.PerDayTimeRange = DataFeed.GetWholeDayTimeRangeByDate(dtToday);
        }
        
        public override void AlgorithmForTheStrategy()
        {
            this.sMessage = "";
            // Iterate every time range, and create task for every schedule of one person.
            foreach (DateTime dtTmp in PerDayTimeRange)
            {
                int iCount = 0;
                int iSchedulesCount = this.oSchedules.Count;
                Parallel.For(0, iSchedulesCount, (i) =>
                {
                    if (this.oSchedules[i].lstWorkTime.Contains(dtTmp))
                    {
                        //Console.WriteLine("{0}, Thread ID {1} ,dtTmp = {2}, Person={3} ", i, Thread.CurrentThread.ManagedThreadId, dtTmp.ToString(), this.oSchedules[i].Name);
                        iCount++;
                    }
                });

                // Record the time string
                if (iCount >= this.iNumberOfPerson)
                {
                    this.sMessage += ", " + dtTmp.ToString("HH:mm");
                }
            }

            this.sMessage = this.sMessage.Length == 0 ? "No Result!" : this.sMessage;

        }       

    }
}
