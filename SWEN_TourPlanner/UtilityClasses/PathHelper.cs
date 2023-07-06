
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWEN_TourPlanner.Utility
{
    public class PathHelper
    {
        public PathHelper() { }

        public string GetBasePath()
        {
            string workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string basePath = Directory.GetParent(workingDirectory).Parent.Parent.Parent.Parent.Parent.FullName;
            return basePath;
        }
    }
}
