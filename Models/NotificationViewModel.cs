using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CodingChallenge.Models;

public class NotificationViewModel
{
    [Display(Name = "First Name")]
    [DataType(DataType.Text)]
    [Required]
    public String? FirstName { get; set; }

    [Display(Name = "Last Name")]
    [DataType(DataType.Text)]
    [Required]
    public String? LastName { get; set; }

    public Boolean IsEmailSelected { get; set; }

    public Boolean IsPhoneSelected { get; set; }

    [EmailAddress]
    [RequiredIfEmailChecked]
    public String? Email { get; set; }

    [Display(Name = "Phone Number")]
    [Phone, MinLength(7), MaxLength(14)]
    [RequiredIfPhoneNumberChecked]
    public String? PhoneNumber { get; set; }

    [Display(Name = "Supervisor")]
    [Required]
    public int SelectedSupervisorId { get; set; }

    public static List<SelectListItem>? Supervisors { get; set; }
}

public class RequiredIfEmailChecked : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
      var instance = validationContext.ObjectInstance;  
      var type = instance.GetType();  
      Boolean propertyValue = (Boolean)type.GetProperty(nameof(NotificationViewModel.IsEmailSelected)).GetValue(instance, null);  
      if (propertyValue && (value == null || String.IsNullOrWhiteSpace(value.ToString()))) 
      {  
         return new ValidationResult("Email is selected. Please add an Email address.");  
      }  
      return ValidationResult.Success; 
    }
}

public class RequiredIfPhoneNumberChecked : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
      var instance = validationContext.ObjectInstance;  
      var type = instance.GetType();  
      Boolean propertyValue = (Boolean)type.GetProperty(nameof(NotificationViewModel.IsPhoneSelected)).GetValue(instance, null);  
      if (propertyValue && (value == null || String.IsNullOrWhiteSpace(value.ToString()))) 
      {  
         return new ValidationResult("Phone is selected. Please add a phone number.");  
      }  
      return ValidationResult.Success; 
    }
}