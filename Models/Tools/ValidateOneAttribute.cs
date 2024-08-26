namespace ELib_IDSFintech_Internship.Models.Tools
{
    using System.ComponentModel.DataAnnotations;

    public class ValidateOneAttribute : ValidationAttribute
    {
        private readonly string _property1;
        private readonly string _property2;
        private readonly string _property3;

        public ValidateOneAttribute(string property1, string property2, string property3)
        {
            _property1 = property1;
            _property2 = property2;
            _property3 = property3;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property1Value = validationContext.ObjectType.GetProperty(_property1)?.GetValue(validationContext.ObjectInstance);
            var property2Value = validationContext.ObjectType.GetProperty(_property2)?.GetValue(validationContext.ObjectInstance);
            var property3Value = validationContext.ObjectType.GetProperty(_property3)?.GetValue(validationContext.ObjectInstance);

            if (property3Value == null)
            {
                return new ValidationResult($"{_property3} must be provided");
            }
            //if both arent present
            if (property1Value == null && property2Value == null)
            {
                return new ValidationResult($"Either {_property1} or {_property2} must be provided.");
            }
            //if both are present, we dont want both also
            else if( property1Value != null && property2Value != null)
            {
                return new ValidationResult($"Either {_property1} or {_property2} must be removed.");
            }

            return ValidationResult.Success;
        }
    }
}
