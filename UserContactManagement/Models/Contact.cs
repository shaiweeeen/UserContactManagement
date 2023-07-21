using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using UserContactManagement.Areas.Identity.Data;

namespace UserContactManagement.Models
{
    public class Contact
    {
       
        public int Id { get; set; }
        [Required]
        [Display(Name ="First Name")]
        public  string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    

        [Required]
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; }
        [Required]
        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }

       
        public string UserId { get; set; }//foreign key
     
        public User User { get; set; } //navigation property to User entity
    }

   
    
}
