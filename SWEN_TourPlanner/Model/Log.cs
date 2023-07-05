
namespace SWEN_TourPlanner.Model
{
    public partial class Log
    {
        public int? Id { get; set; }

        public int? TourId { get; set; }

        public DateTime? TourDate { get; set; }

        public string? Comment { get; set; } 

        public int? Difficulty { get; set; }

        public int? TotalTime { get; set; }

        public int? Rating { get; set; }

        public virtual Tour? Tour { get; set; }
    }
}
