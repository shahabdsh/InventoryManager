namespace InventoryManager.Api.Dtos
{
    public class FieldValidationErrorDto
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
        public string AttemptedValue { get; set; }
    }
}
