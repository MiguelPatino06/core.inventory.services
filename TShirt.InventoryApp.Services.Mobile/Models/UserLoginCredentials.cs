using System.Collections.Generic;
using System.Threading.Tasks;
using TShirt.InventoryApp.Services.Mobile.Services;
using System.ComponentModel.DataAnnotations;
using TShirt.InventoryApp.Services.Mobile.Properties;

namespace TShirt.InventoryApp.Services.Mobile.Models
{
  public class UserLoginCredentials : IValidatableObject
  {
    [Display(Name = "Nombre Usuario")]
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldRequiredValidationMessage")]
    [DataType(DataType.Text)]
    [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "TextFieldMaxLenghtValidationMessage")]
    public string Code { get; set; }


    [Display(Name = "Password")]
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldRequiredValidationMessage")]
    [DataType(DataType.Password)]
    [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "TextFieldMaxLenghtValidationMessage")]
    public string Password { get; set; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {

      UserServices _services = new UserServices();
      var result = _services.GetProviderName(Code, Password).Result;
      var task = Task.Run(async () => await _services.GetProviderName(Code, Password));

      if (task.Result.IsActive == 1)
      {
        yield return new ValidationResult(string.Format("El usuario se encuentra en estado Inactivo", Password), new string[] { "Password" });

      }
    }
  }
}
