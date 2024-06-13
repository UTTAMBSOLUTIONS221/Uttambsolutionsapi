namespace DBL.Entities
{
    public class Newcustomersubscription
    {
        public string? Fullname { get; set; }
        public string? Emailaddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Emailto { get; set; }
        public string? subject { get; set; }
        public string? body { get; set; }
        public bool IsBodyHtml { get; set; }
        public string? EmailServer { get; set; }
        public string? EmailServerEmail { get; set; }
        public string? EmailServerPassword { get; set; }
    }
}
