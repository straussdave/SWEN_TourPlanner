
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWEN_TourPlanner.Model
{
    public partial class Tour
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? FromLocation { get; set; }

        public string? ToLocation { get; set; }

        public string? TransportType { get; set; }

        public int? TourDistance { get; set; }

        public int? EstimatedTime { get; set; }

        public string? RouteImage { get; set; }

        public virtual ICollection<Log>? Logs { get; set; } = new List<Log>();
    }
}