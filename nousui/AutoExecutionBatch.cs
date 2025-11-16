using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

public static class OrchestratorAndTimer
{
    // タイマートリガー（定期的にオーケストレーションを開始）
    [FunctionName("TimerStarter")]
    public static async Task RunTimer(
        [TimerTrigger("0 */1 * * * *")] TimerInfo timer,   // 毎分実行（例）
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log)
    {
        log.LogInformation($"Timer triggered at: {DateTime.Now}");

        // オーケストレーターを開始
        string instanceId = await starter.StartNewAsync("MainOrchestrator", null);
        log.LogInformation($"Started orchestration with ID = {instanceId}");
    }

    // オーケストレーター（処理の流れを定義）
    [FunctionName("MainOrchestrator")]
    public static async Task RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        // 初期処理
        await context.CallActivityAsync("AutoExecutionBatchInitialize", null);

        // メイン処理（POST送信）
        await context.CallActivityAsync("AutoDictionaryCreation", null);
    }
}
