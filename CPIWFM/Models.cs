using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPIWFM
{

    public class SchedulesResultList
    {
        public ScheduleResult ScheduleResult;
    }

    public class ScheduleResult
    {
        public List<Schedules> Schedules { get; set; }
    }

    public class Schedules
    {
        public int ContractTimeMinutes { get; private set; }

        public DateTime Date { get; set; }

        public bool IsFullDayAbsence { get; set; }

        public string Name { get; set; }

        public string PersonId { get; set; }

        public List<Projection> Projection { get; set; }

        public List<DateTime> lstWorkTime
        {
            get; set;
        }

    }

    public class Projection
    {
        string _Color;
        string _Description;
        DateTime _Start;
        int _minutes;

        public int minutes
        {
            get
            {
                return _minutes;
            }

            set
            {
                _minutes = value;
            }
        }

        public DateTime Start
        {
            get
            {
                return _Start;
            }

            set
            {
                _Start = value;
            }
        }

        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        public string Color
        {
            get
            {
                return _Color;
            }

            set
            {
                _Color = value;
            }
        }
    }
}
