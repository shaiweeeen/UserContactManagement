using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using UserContactManagement.Models;

namespace UserContactManagement.Areas.Identity.Data;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
  
    public string FirstName {get; set; }
    public string LastName { get; set; }
    public virtual IEnumerable<Contact> Contacts { get; set; } = new List<Contact>();

}

