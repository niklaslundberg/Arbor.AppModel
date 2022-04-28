using System.ComponentModel.DataAnnotations;
using Arbor.AppModel.Validation;

namespace Arbor.AppModel.Tests
{
    [NoValidation]
    public class NoValidationAttributeModel
    {
        [Required]
        public string? Name { get; set; }
    }
}