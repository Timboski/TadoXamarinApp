using Polly;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace TadoXamarinApp
{
    public class TadoController
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _username;
        private readonly string _password;

        public TadoController(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public async Task SetOverlayTemperature(Info info, int tempInCelcius, int numberOfSeconds, int zone)
        {
            var requestUri = $"https://my.tado.com/api/v2/homes/{info.homeId}/zones/{zone}/overlay";
            using (HttpContent content = GenerateZoneOverlayContent(tempInCelcius, numberOfSeconds))
                await Policy
                    .Handle<HttpRequestException>()
                    .RetryAsync(async (ex, count) => await GetToken())
                    .ExecuteAsync(async () =>
                    {
                        var response = await _httpClient.PutAsync(requestUri, content);
                        var body = await response.Content.ReadAsStringAsync();
                    });
        }

        public async Task CancelOverlayTemperature(Info info, int Zone)
        {
            await Policy
                .Handle<HttpRequestException>()
                .RetryAsync(async (ex, count) => await GetToken())
                .ExecuteAsync(async () =>
                    {
                        var response = await _httpClient.DeleteAsync($"https://my.tado.com/api/v2/homes/{info.homeId}/zones/{Zone}/overlay");
                        var body = await response.Content.ReadAsStringAsync();
                    });
        }

        public async Task<Info> ReadInfo()
        {
            return await Policy
               .Handle<HttpRequestException>()
               .RetryAsync(async (ex, count) => await GetToken())
               .ExecuteAsync(async () =>
               {
                   var response = await _httpClient.GetAsync("https://my.tado.com/api/v1/me");
                   response.EnsureSuccessStatusCode();
                   var body = await response.Content.ReadAsStringAsync();
                   return JsonSerializer.Deserialize<Info>(body);
               });
        }

        private static StringContent GenerateZoneOverlayContent(int tempInCelcius, int numberOfSeconds)
        {
            var overlay = new ZoneOverlay(tempInCelcius, numberOfSeconds);
            var overlayString = JsonSerializer.Serialize(overlay);
            var contenttemp = new StringContent(overlayString);
            contenttemp.Headers.ContentType.MediaType = "application/json";
            var content = contenttemp;
            return content;
        }

        private async Task GetToken()
        {
            var requestUri = $"https://auth.tado.com/oauth/token?client_id=tado-web-app&grant_type=password&scope=home.user&username={_username}&password={_password}&client_secret=wZaRN7rpjn3FoNyF5IFuxg9uMzYJcvOoQ8QWiIqS3hfk6gLhVlG57j5YNoZL2Rtc";
            var response = await _httpClient.PostAsync(requestUri, null);
            string token = await ReadAccessToken(response);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private static async Task<string> ReadAccessToken(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            var header = JsonSerializer.Deserialize<Header>(body);
            return header.access_token;
        }

        class Header
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string refresh_token { get; set; }
            public int expires_in { get; set; }
            public string scope { get; set; }
            public string jti { get; set; }
        }

        public class Info
        {
            public string name { get; set; }
            public string email { get; set; }
            public string username { get; set; }
            public bool enabled { get; set; }
            public string id { get; set; }
            public int homeId { get; set; }
            public string locale { get; set; }
            public string type { get; set; }
        }


        public class ZoneOverlay
        {
            public ZoneOverlay(int tempInCelcius, int numberOfSeconds)
            {
                setting = new Setting(tempInCelcius);
                termination = new Termination(numberOfSeconds);
            }

            public Setting setting { get; set; }
            public Termination termination { get; set; }
        }

        public class Setting
        {
            public Setting(int temperatureInCelcius)
            {
                temperature = new Temperature(temperatureInCelcius);
            }

            public string type => "HEATING";
            public string power => "ON";
            public Temperature temperature { get; }
        }

        public class Temperature
        {
            public Temperature(int temperatureInCelcius)
            {
                celsius = temperatureInCelcius;
            }

            public int celsius { get; }
        }

        public class Termination
        {
            public Termination(int seconds)
            {
                durationInSeconds = seconds;
            }

            public string type => "TIMER";
            public int durationInSeconds { get; }
        }

    }
}
