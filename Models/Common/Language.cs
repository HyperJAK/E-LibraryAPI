using System.ComponentModel.DataAnnotations;

namespace ELib_IDSFintech_Internship.Models.Common
{
    public class Language
    {
        [Key]
        public int Id { get; set; }

        public required string Type { get; set; }
    }
}
