using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWEN_TourPlanner.Model
{
    internal class Log
    {
        public int id {  get; set; }
        public int tourId { get; set; }
        public string tourDate { get; set; }
        public string comment { get; set; }
        public int difficulty { get; set; }
        public int totalTime { get; set; }
        public int rating { get; set; }
    }
}
