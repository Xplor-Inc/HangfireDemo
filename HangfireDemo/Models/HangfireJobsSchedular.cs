using Hangfire;
using System.Text;
using System;
using Newtonsoft.Json.Serialization;

namespace HangfireDemo.Models;

public class HangfireJobsSchedular
{
    public void AddHangfireJobs()
    {
        for (int i = 1; i <= 100; i++)
        {
            var postbody = new PostBody { Title = $"{i}. Title body of post method", UserId = 5 };
            BackgroundJob.Enqueue<HangfireJobsSchedular>(e => e.DummyApiCall(postbody));
        }
    }

    public async Task<string> DummyApiCall(PostBody postBody)
    {
        HttpClient httpClient = new() { BaseAddress = new Uri("https://dummyjson.com/") };
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(postBody, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        var respo = await httpClient.PostAsync("posts/add", data);
        if (respo != null && respo.IsSuccessStatusCode)
        {
            return await respo.Content.ReadAsStringAsync();
        }
        else
        {
            return "Error while calling API with values : " + data;
        }
    }

}

public class PostBody
{
    public string   Title   { get; set; } = default!;
    public int      UserId  { get; set; }
}