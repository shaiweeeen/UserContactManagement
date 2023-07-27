using UserContactManagement.Areas.Identity.Data;

namespace UserContactManagement.Models
{
    public class Contact
    {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string DeliveryAddress { get; set; }

        public string BillingAddress { get; set; }
        public string ContactNumber { get; set; }
        public string AvatarUrl { get; set; }

        public string UserId { get; set; }//foreign key

        public User User { get; set; } //navigation property to User entity
    }



}
