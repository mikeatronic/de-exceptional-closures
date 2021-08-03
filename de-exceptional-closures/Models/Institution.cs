using System;

namespace de_exceptional_closures.Models
{
    public class Institution
    {
        public string ReferenceNumber { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Management { get; set; }
        public string InstitutionType { get; set; }
        public string Status { get; set; }
        public DateTime? DateClosed { get; set; }
        public string Email { get; set; }
    }
}