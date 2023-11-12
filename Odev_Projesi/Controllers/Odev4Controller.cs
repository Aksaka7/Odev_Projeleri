using Microsoft.AspNetCore.Mvc;
using Odev_Projesi.Model.Serialization;
using Newtonsoft.Json;
using System.Xml.Serialization;
using Odev_Projesi.Context.Domain;
using Microsoft.AspNetCore.Hosting;

namespace Odev_Projesi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Odev4Controller : ControllerBase 
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        //IWebHostEnvironment özeligi dosya yükleme gibi işlemlerde sunucunun fiziksel
        //dosya yollarına erişmenizi ve bu dosyalara yazma veya okuma işlemleri yapmanızı sağlar.
        public Odev4Controller(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Route("JSON_Odev4_Serialize")]
        public async Task<IActionResult> JsonIslemleri()
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://jsonplaceholder.typicode.com")
            };

            int userId = 1; // Örnek olarak bir kullanıcıyı JSON'a çeviriyoruz
            SerializationUser? serializationUser = await client.GetFromJsonAsync<SerializationUser>($"users/{userId}");
            SerializationUser user = serializationUser;


            if (user != null)
            {
                // Nesneyi JSON formatına çevirme
                string jsonResult = JsonConvert.SerializeObject(user);

                return Ok(jsonResult);
            }
            else
            {
                return NotFound(); // veya başka bir durum kodu
            }
        }

        [HttpGet]
        [Route("XML_Odev4_Serialize")]
        public async Task<IActionResult> XMLIslemleri()
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://jsonplaceholder.typicode.com")
            };

            XmlSerializer seriali = new XmlSerializer(typeof(List<SerializationUser>));

            var userId = 1;
            SerializationUser? serializationUser = await client.GetFromJsonAsync<SerializationUser>($"users/{userId}");
            SerializationUser user = serializationUser;

            var o = new List<SerializationUser>
            {
                new SerializationUser {Id = user.Id, Name = user.Name, Username = user.Username ,Email = user.Email, Phone = user.Phone }
            };

            TextWriter writer = new StreamWriter(Guid.NewGuid().ToString() + "xml");
            seriali.Serialize(writer, o);
            writer.Close();

            return Ok();
        }


        [HttpPost]
        [Route("Dosyauploads")]
        public IActionResult DosyaYukles(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "documents");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Ok("Dosya yükleme başarılı!");
            }

            return BadRequest("Dosya yüklenemedi.");
        }


    }
}
