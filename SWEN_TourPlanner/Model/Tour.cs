using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SWEN_TourPlanner.Model
{
    public class Tour
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string transportType { get; set; }
        public string tourDistance { get; set; }
        public string estimatedTime { get; set; }
        public string routeImagePath { get; set; }
    }
}