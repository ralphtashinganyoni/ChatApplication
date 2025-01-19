using Microsoft.AspNetCore.Identity;

namespace ChatApplication.Server.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }
    }
}
