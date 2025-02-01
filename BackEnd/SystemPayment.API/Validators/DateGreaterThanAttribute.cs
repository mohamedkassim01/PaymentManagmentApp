using System.ComponentModel.DataAnnotations;

namespace SystemPayment.API.Validators
{
	public class DateGreaterThanAttribute : ValidationAttribute
	{
		private readonly string _comparisonProperty;

		public DateGreaterThanAttribute(string comparisonProperty)
		{
			_comparisonProperty = comparisonProperty;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var currentValue = (DateTime?)value;

			var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
			if (property == null)
				return new ValidationResult($"Property {_comparisonProperty} not found.");

			var comparisonValue = (DateTime?)property.GetValue(validationContext.ObjectInstance);

			if (currentValue.HasValue && comparisonValue.HasValue && currentValue <= comparisonValue)
			{
				return new ValidationResult(ErrorMessage ?? $"The field {validationContext.MemberName} must be later than {_comparisonProperty}.");
			}

			return ValidationResult.Success;
		}
	}
}
