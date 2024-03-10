    using NUnit.Framework;
    using Microsoft.AspNetCore.Mvc.Testing;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Signee.ManagerWeb.Controllers; // Replace with your actual API namespace
    using System.Text.Json;
    using System.Text;
    using Microsoft.AspNetCore.Hosting;
    using NUnit.Framework.Legacy;

    
    public class DisplayControllerAPITest
    {
        private HttpClient _client;

        [OneTimeSetUp]
        public void Setup()
        {
            var factory = new WebApplicationFactory<Signee.ManagerWeb.Program>();
            _client = factory.CreateClient();
        }

        private async Task<string> AuthenticateAsync()
        {
            var loginData = new { email = "admin@signee.com", password = "administrator" };
            var loginContent = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
            var loginResponse = await _client.PostAsync("/api/auth/login", loginContent);
            ClassicAssert.IsTrue(loginResponse.IsSuccessStatusCode);
            var loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<dynamic>(loginResponseBody);
            string token = loginResult.userData.token;
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
            var groupResult = JsonSerializer.Deserialize<dynamic>(groupResponseBody);
            string groupId = groupResult.id;

            // Add display with group ID
            var displayDataWithId = new { name = "displej-with-id-001", groupId = groupId };
            var displayContentWithId = new StringContent(JsonSerializer.Serialize(displayDataWithId), Encoding.UTF8, "application/json");
            var displayResponseWithId = await _client.PostAsync("/api/display", displayContentWithId);
            ClassicAssert.IsTrue(displayResponseWithId.IsSuccessStatusCode);

            // Add display without group ID
            var displayDataWithoutId = new { name = "displej-without-id-001", groupId = "" };
            var displayContentWithoutId = new StringContent(JsonSerializer.Serialize(displayDataWithoutId), Encoding.UTF8, "application/json");
            var displayResponseWithoutId = await _client.PostAsync("/api/display", displayContentWithoutId);
            ClassicAssert.IsTrue(displayResponseWithoutId.IsSuccessStatusCode);

            // Further assertions to verify the response could be added here
        }
    }