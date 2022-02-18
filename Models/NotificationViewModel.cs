using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    [DataType(DataType.EmailAddress)]
    public String? Email { get; set; }

    [Display(Name = "Phone Number")]
    [DataType(DataType.PhoneNumber)]
    public String? PhoneNumber { get; set; }

    [DataType(DataType.Text)]
    public String? Supervisor { get; set; }

    public Dictionary<int, String> Supervisors { get; set; } = new Dictionary<int, String>() { { 0, "Select..." } };
}