
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Threading.Tasks;

namespace ApiTestsGitHub.GitHubRepository
{
    public class GitHubRepositoryTests
    {
        private RestClient client;
        private RestRequest request;

        [SetUp]
        public void Setup()
        {
            client = new RestClient("https://api.github.com");
            client.Authenticator = new HttpBasicAuthenticator("{{user}}", "{{token}}");
        }

        [Test]
        public async Task Test_01GitHub_CreateRepository()
        {
            this.request = new RestRequest("/user/repos");
            this.request.AddJsonBody(new { name = "RepositoryCreatedWithRestSharpForTest" });
            this.request.AddHeader("Accept", "application/vnd.github.v3+json");

            var response = await this.client.ExecuteAsync(this.request, Method.Post);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public async Task Test_02GitHub_RenameRepo()
        {
            this.request = new RestRequest("/repos/{{user}}/RepositoryCreatedWithRestSharpForTest");
            this.request.AddJsonBody(new { name = "RepositoryCreatedWithRestSharpForTest2" });

            var response = await this.client.ExecuteAsync(this.request, Method.Patch);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
           
        }

        [Test]
        public async Task Test_03GitHub_DeleteRepo()
        {
            this.request = new RestRequest("/repos/{{user}}/RepositoryCreatedWithRestSharpForTest2");
            this.request.AddHeader("Accept", "application/vnd.github.v3+json");

            var response = await this.client.ExecuteAsync(this.request, Method.Delete);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
}
