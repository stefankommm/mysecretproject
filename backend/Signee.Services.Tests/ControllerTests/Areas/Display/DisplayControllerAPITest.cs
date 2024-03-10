    using NUnit.Framework;
    using System.Text.Json;
    using System.Text;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework.Legacy;
    using Signee.ManagerWeb.Models.Group;
    using Signee.Services.Areas.Auth.Models;

    [TestFixture]
    public class DisplayControllerAPITest
    {
        private HttpClient _client;
        private const string BASE_URL = "http://localhost:5140";
        
        [OneTimeSetUp]
        public void Setup()
        {
            _client = new HttpClient
            {
                BaseAddress = new System.Uri(BASE_URL)
            };
        }

        private async Task<string> AuthenticateAsync()
        {
            var loginData = new { email = "admin@signee.com", password = "administrator" };
            var loginContent = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
            var loginResponse = await _client.PostAsync("/api/auth/login", loginContent);
            ClassicAssert.IsTrue(loginResponse.IsSuccessStatusCode);
            var loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
            var loginResponseJsonObject = JObject.Parse(loginResponseBody);
            var userData = loginResponseJsonObject["userData"].ToString();
            var loginResult = JsonSerializer.Deserialize<AuthResponseApi>(userData);
            string token = loginResult.Token;
            return token;
        }

        [Test]
        public async Task TestGroupAndDisplayFlow()
        {
            // Authenticate and get token
            string token = await AuthenticateAsync();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Create group
            var groupData = new { name = "my-test-group" };
            var groupContent = new StringContent(JsonSerializer.Serialize(groupData), Encoding.UTF8, "application/json");
            var groupResponse = await _client.PostAsync("/api/group/", groupContent);
            ClassicAssert.IsTrue(groupResponse.IsSuccessStatusCode);
            var groupResponseBody = await groupResponse.Content.ReadAsStringAsync();
            var groupResult = JsonSerializer.Deserialize<GroupResponseApi>(groupResponseBody);
            string groupId = groupResult.Id;

            // Add display with group ID
            var displayDataWithId = new { name = "displej-with-id-001", groupId = groupId };
            var displayContentWithId = new StringContent(JsonSerializer.Serialize(displayDataWithId), Encoding.UTF8, "application/json");
            var displayResponseWithId = await _client.PostAsync("/api/display", displayContentWithId);
            ClassicAssert.IsTrue(displayResponseWithId.IsSuccessStatusCode);

            // Add display without group ID
            var displayDataWithoutId = new { name = "without-id-001" };
            var displayContentWithoutId = new StringContent(JsonSerializer.Serialize(displayDataWithoutId), Encoding.UTF8, "application/json");
            var displayResponseWithoutId = await _client.PostAsync("/api/display", displayContentWithoutId);
            ClassicAssert.IsTrue(displayResponseWithoutId.IsSuccessStatusCode);

            // Further assertions to verify the response could be added here
        }
    }