using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace OpenAiApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class HttpController : Controller
    {
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Request(string content)
        {
            using HttpClient client = new HttpClient();
            var payload = JsonConvert.SerializeObject(new
            {
                model = "gpt-3.5-turbo",
                messages = new List<object>()
                {
                    new { role= "user", content= content}
                }
            });
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions"))
            {

                request.Content = new StringContent(payload, Encoding.UTF8);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "");
                using var response = await client.SendAsync(request);

                string result = await response.Content.ReadAsStringAsync();
                return Content(result);
            }

        }
    }
}
