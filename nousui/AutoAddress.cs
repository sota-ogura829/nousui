using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

public static class Activity_Main
{
    [FunctionName("Activity_Main")]
    public static async Task Run([ActivityTrigger] object input, ILogger log)
    {
        log.LogInformation("メイン処理を実行中...");

        string baseUrl = "https://example.com/api/";
        string endpoint = "postdata";
        string jsonBody = "{\"device\":\"A001\",\"status\":\"active\"}";

        string response = await HttpHelper.SendPostRequest(baseUrl, endpoint, jsonBody);

        log.LogInformation($"POST Response: {response}");
    }
}
