namespace SendReportService.Models
{
    public class Ratings
    {
        public long id { get; set; }
        public long organization_id { get; set; }
        public string adress { get; set; }
        public string name { get; set; }
        public int rate { get; set; }
    }
}
