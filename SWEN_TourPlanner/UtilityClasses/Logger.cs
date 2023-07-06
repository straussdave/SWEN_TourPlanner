using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SWEN_TourPlanner.Utility
{
    public class Logger
    {
        public ILog log;

        public Logger()
        {
            var a = Assembly.GetExecutingAssembly();

            var patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date %level %logger - %message%newline";
            patternLayout.ActivateOptions();

            var rollingFileAppender = new RollingFileAppender()
            {
                Name = "FileAppender",
                Layout = patternLayout,
                Threshold = Level.All,
                AppendToFile = true,
                File = "./TourPlanner.log",
                MaximumFileSize = "1MB",
                MaxSizeRollBackups = 15
            };
            rollingFileAppender.ActivateOptions();
            BasicConfigurator.Configure(rollingFileAppender);
            log = LogManager.GetLogger(typeof(Logger));

            log.Debug(a.ToString());
        }
    }
}