using System.ComponentModel.DataAnnotations;

namespace Arbor.AppModel.Tests
{
    public class NoValidationModel
    {
        [Required]
        public string? Name { get; set; }
    }
}