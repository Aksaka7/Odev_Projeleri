using System.ComponentModel.DataAnnotations;

namespace Odev_Projesi.Context.Domain
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
