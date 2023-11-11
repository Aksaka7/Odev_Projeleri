namespace Odev_Projesi.Model.OyuncuDetaylari
{
    public class UserModel
    {
        public bool Authenticate { get; set; }

        public string Token { get; set; }

        public DateTime TokenExpireDate { get; set; }

        public string Message { get; set; }
    }
}
