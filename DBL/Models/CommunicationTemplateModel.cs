namespace DBL.Models
{
    public class CommunicationTemplateModel
    {

        public long Templateid { get; set; }
        public string? Templatename { get; set; }
        public string? Templatesubject { get; set; }
        public string? Templatebody { get; set; }
        public string? Module { get; set; }
        public string? Moduleemail { get; set; }
        public string? Modulephone { get; set; }
        public string? Modulelogo { get; set; }
        public bool Isactive { get; set; }
        public bool Isdeleted { get; set; }
    }
}
