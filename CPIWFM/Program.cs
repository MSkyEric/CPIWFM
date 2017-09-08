using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Collections;

namespace CPIWFM
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                #region // Init data
                DataFeed.sDescFilter = new string[2] { "Short break", "Lunch" };
                List<Schedules> oSchedules = DataFeed.InitData();
                #endregion

                #region // Do work
                while (true)
                {
                    int iNumberOfPerson = 0;
                    
                    // Get number from front
                    while (iNumberOfPerson == 0)
                    {
                        Console.WriteLine("Please input the number of person:");
                        int.TryParse(Console.ReadLine(), out iNumberOfPerson);
                    }

                    // Use the context to invoke the strategy
                    Context oContext;
                    Strategy oStrategy = new CPIWFMStrategy(oSchedules, iNumberOfPerson);
                    oContext = new Context(oStrategy);
                    oContext.ContextInterface();

                    Console.WriteLine(oStrategy.sMessage.TrimStart(','));
                    Console.WriteLine();
                    
                }
                #endregion
            }
            catch (Exception ex)
            {
                #region // log the exception
                using (StreamWriter oStreamWriter = new StreamWriter("Error.txt", true))
                {
                    oStreamWriter.WriteLine("Time：" + DateTime.Now.ToString());
                    oStreamWriter.WriteLine("Message：" + ex.Message);
                    oStreamWriter.WriteLine("Source：" + ex.Source);
                    oStreamWriter.WriteLine("StackTrace：\n" + ex.StackTrace.Trim());
                    oStreamWriter.WriteLine("TargetSite：" + ex.TargetSite);
                    oStreamWriter.WriteLine();
                }
                #endregion

                Console.WriteLine("Error happened, please check the error file!");

                Console.ReadLine();
            }
        }
    }

}

