using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiTestsGitHub.GitHubIssue
{
    public class GitHubIssuesTests
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
        public async Task Test_GitHub_APIRequest()
        {
            var url = "repos/{{user}}/{{repo}}/issues";
            request = new RestRequest(url);
            var response = await client.ExecuteAsync(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Test_Git_CheckIssuesNotEmpty()
        {
            var url = "repos/{{user}}/{{repo}}/issues";
            request = new RestRequest(url);
            var response = await client.ExecuteAsync(request);
            var issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);
            Assert.That(issues.Count > 1);

            foreach (var issue in issues)
            {
                Assert.Greater(issue.id, 0);
                Assert.Greater(issue.number, 0);
                Assert.IsNotEmpty(issue.title);
            }
        }

        [Test]
        public async Task Test_GitHub_CreateIssue()
        {
            var url = "repos/{{user}}/{{repo}}/issues";
            request = new RestRequest(url);
            request.AddBody(new { title = "RestSharpIssueTest" });

            var response = await client.ExecuteAsync(request, Method.Post);


            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }


        [Test]
        public async Task Test_GitHub_RenameIssue()
        {
            var url = "repos/{{user}}/{{repo}}/issues/6";
            request = new RestRequest(url);
            request.AddBody(new { title = "Тhird issue from Postman" });

            var response = await client.ExecuteAsync(request, Method.Patch);


            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Test_GitHub_GetIssueWithNumber()
        {
            var url = "repos/{{user}}/{{repo}}/issues/6";
            request = new RestRequest(url);
            request.AddBody(new { title = "Тhird issue from Postman" });

            var response = await client.ExecuteAsync(request, Method.Get);


            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Test_GitHub_GetIssues()
        {
            var url = "repos/{{user}}/{{repo}}/issues";
            request = new RestRequest(url);

            var response = await client.ExecuteAsync(request, Method.Get);
            var issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);

            foreach (var issue in issues)
            {
                Console.WriteLine(issue.id);
                Assert.IsNotEmpty(issue.htmlUrl);
                Assert.IsNotNull(issue.id);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }
    }
}