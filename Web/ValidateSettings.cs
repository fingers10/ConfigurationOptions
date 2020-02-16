using System.ComponentModel.DataAnnotations;

namespace Web
{
    public class ValidateSettings
    {
        [Required]
        public string Test { get; set; }
    }
}
