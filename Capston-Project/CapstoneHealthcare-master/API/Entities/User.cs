using Microsoft.AspNetCore.Identity;

namespace APPUI.Entities
{
    public class User : IdentityUser<int>
    {
        public UserAddress Address { get; set; }
    }
}
