using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Utils.pkg.Validation.Request;

public class ValidationWrapper<T>
{
    public bool IsValid { get; set; }
    public T Value { get; set; }

    public IEnumerable<ValidationResult> ValidationResults { get; set; }
}