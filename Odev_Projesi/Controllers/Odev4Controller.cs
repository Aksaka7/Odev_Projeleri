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

                // JSON'ı response body olarak döndürme
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
        [Route("Dosyaupload")]
        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files, [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            long size = files.Sum(f => f.Length);
            string path = @"C:\Users\Mehmet_Asker\source\repos\Odevteslimapp18\Odev_Projesi\Properties\www";


            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();


                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

          

            return Ok(new { count = files.Count, size });
        }

    }
}
