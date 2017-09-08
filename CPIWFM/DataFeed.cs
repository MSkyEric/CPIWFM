using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace CPIWFM
{
    public class DataFeed
    {
        public DataFeed()
        {
        }

        public static string[] sDescFilter;

        private static SchedulesResultList ConvertJsonToObject()
        {
            string sJsonStr = "";
            using (StreamReader sr = new StreamReader(File.Open(@"schedule.json", FileMode.Open)))
            {
                sJsonStr = sr.ReadToEnd();
            }
            SchedulesResultList oSchedulesResultList = JsonConvert.DeserializeObject<SchedulesResultList>(sJsonStr);

            return oSchedulesResultList;
        }

        private static List<Schedules> ParseObjectToList(SchedulesResultList oSchedulesResultList)
        {
            List<Schedules> oSchedulesList = new List<Schedules>();

            foreach (Schedules oSchedules in oSchedulesResultList.ScheduleResult.Schedules)
            {
                // Execute and set the work time list for the person   
                List<DateTime> lstWorkTime = new List<DateTime>();
                foreach (Projection oProjectionTmp in oSchedules.Projection)
                {
                    string sToday = oSchedules.Date.ToString("yyyy-MM-dd");
                    string sProjectionDate = oProjectionTmp.Start.ToString("yyyy-MM-dd");
                    if (sToday == sProjectionDate && !sDescFilter.Contains(oProjectionTmp.Description))
                    {
                        int iCount = oProjectionTmp.minutes / 15;
                        for (int i = 0; i < iCount; i++)
                        {
                            DateTime dtCanMeetingTime = oProjectionTmp.Start.AddMinutes(i * 15);

                            // only add the date in the same day.
                            bool bAdd = dtCanMeetingTime.Date >= oSchedules.Date.Date.AddDays(1) ? false : true;
                            if (bAdd)
                            {
                                lstWorkTime.Add(dtCanMeetingTime);
                            }
                        }
                    }
                }
                oSchedules.lstWorkTime = lstWorkTime;

                oSchedulesList.Add(oSchedules);
            }

            return oSchedulesList;
        }

        public static List<Schedules> InitData()
        {
            SchedulesResultList oSchedulesResultList = ConvertJsonToObject();
            List<Schedules> oSchedulesList = ParseObjectToList(oSchedulesResultList);

            return oSchedulesList;
        }

        private const int iMinutesToSplit = 15;
        public static List<DateTime> GetWholeDayTimeRangeByDate(DateTime dtToday)
        {
            int iRangeCount = 60 * 24 / iMinutesToSplit;

            List<DateTime> lstDateTimeRange = new List<DateTime>();
            for (int i = 0; i < iRangeCount; i++)
            {
                lstDateTimeRange.Add(dtToday.AddMinutes(iMinutesToSplit * i));
            }
            return lstDateTimeRange;
        }
    }
    

}
