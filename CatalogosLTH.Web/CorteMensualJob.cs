using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CatalogosLTH.Web
{
    public class CorteMensualJob:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            using (StreamWriter streamWriter = new StreamWriter(@"C:\sitios\LTH\IDGLog.txt", true))
            {
                streamWriter.WriteLine(DateTime.Now.ToString());
            }
        }
    }
}