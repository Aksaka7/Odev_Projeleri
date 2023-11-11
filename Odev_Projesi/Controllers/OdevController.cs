using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Odev_Projesi.Context;
using Odev_Projesi.Model.OyuncuDetaylari;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Odev_Projesi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OdevController : ControllerBase
    {
        private readonly OdevKurulumDbContext dbContext;

        public OdevController(IConfiguration configuration, OdevKurulumDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("UserOlustur")]
        public IActionResult LoginIslemiIcinCreate([FromBody] OyuncuKayitModel model ) 
        {
            if (model == null)
            {
                return BadRequest("Form Alanlari Boş bırakılamaz.");
            }

            if(string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(model.Password)) 
            {
                return BadRequest("Email&Password cannot not be Empty");
            }

            // Email için Regex kontrolü
            string emailRedex = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            if (!Regex.IsMatch(model.Email, emailRedex))
            {
                return BadRequest("Geçerli bir Email adresi giriniz.");
            }

            // Password için sayı kontrolü
            string passwordRedex = "^[0-9]+$";
            if (!Regex.IsMatch(model.Password, passwordRedex))
            {
                return BadRequest("Password sadece sayılardan oluşmalıdır.");
            }

            dbContext.UserOdevs.Add(new Context.Domain.UserInfo { Name = model.Name, Surname = model.Surname, Email = model.Email, Password = model.Password, Id= Guid.NewGuid()});
            var result = dbContext.SaveChanges();
            
            if(result <=0)
            {
                return BadRequest("Uyelik oluşturulamadi");
            }
            else
            {
                return Ok("User Created.");
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult LoginPage([FromBody] OyuncuLoginModel model)
        {
            if(string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Email and Password cannot be empty.");
            }

            var response = new UserModel();

            var findUser = dbContext.UserOdevs.FirstOrDefault(p => p.Email == model.Email && p.Password == model.Password);

            if(findUser == null)
            {
                return NotFound();
            }

            response.TokenExpireDate = DateTime.Now;
            response.Message = "Oyuncu Giriş yaptı";
            response.Authenticate = true;
            response.Token = string.Empty;

            return Ok(JsonConvert.SerializeObject(response));

        }

        [HttpPost]
        [Route("FormatDecimal")]
        public IActionResult FormatDecimal([FromBody] decimal value)
        {
            // Decimal türündeki veriyi CurrentCulture ile formatla ve göster
            string formattedValue = value.ToString("N", CultureInfo.CurrentCulture);
            return Ok(formattedValue);
        }
    }
}
