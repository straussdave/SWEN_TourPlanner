using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using SWEN_TourPlanner.Model;

namespace SWEN_TourPlanner.Utility
{
    internal class ReportGenerator
    {
        static PathHelper ph = new PathHelper();
        DatabaseHandler db = new DatabaseHandler();
        string reportsPath = ph.GetBasePath() + "\\Reports";
        string imagesPath = ph.GetBasePath() + "\\Images";


        public void GenerateReport(Tour tour)
        {
            string pdfName = tour.Name + ".pdf";
            string path = Path.Combine(reportsPath, pdfName);
            var writer = new PdfWriter(path);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var header = new Paragraph(tour.Name)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                .SetFontSize(25)
                .SetBold();
            document.Add(header);

            var description = new Paragraph(tour.Description);
            var transportType = new Paragraph("Transport Type: " + CapitalizeFirstLetter(tour.TransportType));
            var fromLocation = new Paragraph("From: " + tour.FromLocation);
            var toLocation = new Paragraph("To: " + tour.ToLocation);
            var tourDistance = new Paragraph("Distance: " + tour.TourDistance + "km");
            var time = new Paragraph("Estimated Time: " + ConvertToHoursAndMinutes(tour.EstimatedTime));
            var imageFileName = tour.RouteImage;
            var imagePath = Path.Combine(imagesPath, imageFileName);
            var image = ImageDataFactory.Create(imagePath);
            var tableHeader = new Paragraph("Tour Logs")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetBold()
                .SetFontSize(20);
            var table = new Table(UnitValue.CreatePercentArray(new float[] { 10, 50, 1, 20, 1 })).UseAllAvailableWidth();
            table.AddHeaderCell("Date");
            table.AddHeaderCell("Comment");
            table.AddHeaderCell("Difficulty");
            table.AddHeaderCell("Real Time");
            table.AddHeaderCell("Rating");
            table.SetFontSize(14).SetBackgroundColor(ColorConstants.WHITE);
            List<Log> logList = db.ReadLogs();
            foreach (var log in logList)
            {
                if (log.TourId == tour.Id)
                {
                    table.AddCell(log.TourDate.Value.ToString("dd/MM/yyyy"));
                    table.AddCell(log.Comment);
                    table.AddCell(log.Difficulty.ToString());
                    table.AddCell(ConvertToHoursAndMinutes(log.TotalTime));
                    table.AddCell(log.Rating.ToString());
                }

            }


            document.Add(description);
            document.Add(transportType);
            document.Add(fromLocation);
            document.Add(toLocation);
            document.Add(tourDistance);
            document.Add(time);
            document.Add(new iText.Layout.Element.Image(image));
            document.Add(tableHeader);
            document.Add(table);
            document.Close();
        }

        private string ConvertToHoursAndMinutes(int? seconds)
        {
            string timeString = "";
            if (seconds.HasValue)
            {
                int hours = (int)Math.Floor((decimal)(seconds / 3600));
                int minutes = (int)(seconds % 3600 / 60);
                if (hours <= 0)
                {
                    timeString = minutes.ToString() + " Minute";
                }
                else if (hours == 1)
                {
                    timeString = hours.ToString() + " Hour and " + minutes.ToString() + " Minute";
                }
                else
                {
                    timeString = hours.ToString() + " Hours and " + minutes.ToString() + " Minute";
                }
                if (minutes != 1)
                {
                    timeString += "s";
                }

            }
            return timeString;
        }

        private string CapitalizeFirstLetter(string text)
        {
            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}
