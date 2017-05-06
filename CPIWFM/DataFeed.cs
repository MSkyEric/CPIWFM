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
    class DataFeed
    {
        public DataFeed()
        {
        }

        public static string[] sDescFilter;
        public static List<Schedules> InitData()
        {
            string sJsonStr = "";
            using (StreamReader sr = new StreamReader(File.Open(@"schedule.json", FileMode.Open)))
            {
                sJsonStr = sr.ReadToEnd();
            }
            sJsonStr = Regex.Replace(sJsonStr, @"\\/Date\((\d+)[+](\d{4})\)\\/", match =>
            {
                DateTime datetime = new DateTime(1970, 1, 1);
                datetime = datetime.AddMilliseconds(long.Parse(match.Groups[1].Value));
                datetime = datetime.ToLocalTime();
                return datetime.ToString("yyyy-MM-dd HH:mm");
            });

            // use XML to construct the data
            XmlDocument xmlDoc = JsonConvert.DeserializeXmlNode(sJsonStr);

            List<Schedules> oSchedulesList = new List<Schedules>();
            Schedules oSchedules;
            XmlNodeList SchedulesList = xmlDoc.SelectNodes("//ScheduleResult//Schedules");
            foreach (XmlNode SchedulesNode in SchedulesList)
            {
                oSchedules = new Schedules();
                int iContractTimeMinutes;
                int.TryParse(SchedulesNode.SelectSingleNode("ContractTimeMinutes").InnerText.Trim(), out iContractTimeMinutes);
                oSchedules.ContractTimeMinutes = iContractTimeMinutes;

                DateTime dtDate;
                DateTime.TryParse(SchedulesNode.SelectSingleNode("Date").InnerText.Trim(), out dtDate);
                oSchedules.Date = dtDate;

                oSchedules.IsFullDayAbsence = bool.Parse(SchedulesNode.SelectSingleNode("IsFullDayAbsence").InnerText.Trim());

                oSchedules.Name = SchedulesNode.SelectSingleNode("Name").InnerText.Trim();
                oSchedules.PersonId = SchedulesNode.SelectSingleNode("PersonId").InnerText.Trim();

                XmlNodeList ProjectionNodeList = SchedulesNode.SelectNodes("Projection");

                List<Projection> oProjectionList = new List<Projection>();
                Projection oProjection;

                List<DateTime> lstWorkTime = new List<DateTime>();
                foreach (XmlNode ProjectionNode in ProjectionNodeList)
                {
                    oProjection = new Projection();

                    oProjection.Color = ProjectionNode.SelectSingleNode("Color").InnerText.Trim();
                    oProjection.Description = ProjectionNode.SelectSingleNode("Description").InnerText.Trim();

                    DateTime dtStart;
                    DateTime.TryParse(ProjectionNode.SelectSingleNode("Start").InnerText.Trim(), out dtStart);
                    oProjection.Start = dtStart;

                    int iMinutes;
                    int.TryParse(ProjectionNode.SelectSingleNode("minutes").InnerText.Trim(), out iMinutes);
                    oProjection.minutes = iMinutes;

                    oProjectionList.Add(oProjection);
                }
                oSchedules.PersonPerjection = oProjectionList;

                // Execute and set the work time list for the person                
                foreach (Projection oProjectionTmp in oProjectionList)
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

            #region Debug Info
            ///*
            using (StreamWriter oStreamWriter = new StreamWriter("WorkTimeList.txt", true))
            {
                foreach (Schedules oSchedule in oSchedulesList)
                {
                    oStreamWriter.WriteLine(oSchedule.Name);

                    var lstWorkTime = oSchedule.lstWorkTime;
                    foreach (DateTime dt in lstWorkTime)
                    {
                        oStreamWriter.Write(dt.ToString("HH:mm")+", ");

                    }

                    oStreamWriter.WriteLine();
                }

            }
            //*/
            #endregion

            return oSchedulesList;
        }

        const int iMinutesToSplit = 15;
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
