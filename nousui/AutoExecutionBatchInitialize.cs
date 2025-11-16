using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

public static class Activity_Init
{
    [FunctionName("Activity_Init")]
    public static async Task Run([ActivityTrigger] object input, ILogger log)
    {
        log.LogInformation("初期処理を実行中...");
        await Task.Delay(500);  // ダミー処理（初期化などを想定）
        log.LogInformation("初期処理完了。");
    }
}
