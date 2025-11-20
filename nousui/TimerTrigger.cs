using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

public class TimerFunction
{
    [FunctionName("TimerTriggerFunction")]
    public async Task Run(
        [TimerTrigger("0 */5 * * * *")] TimerInfo myTimer,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log)
    {
        var now = DateTime.Now;
        log.LogInformation($"Timer triggered at: {now}");

        // Queue に登録メッセージを送る例（任意）
        // await queue.AddAsync("start");

        string instanceId = "SingleInstance-Orchestrator";

        // 既に動いているかチェック
        var status = await starter.GetStatusAsync(instanceId);

        if (status != null &&
            status.RuntimeStatus != OrchestrationRuntimeStatus.Completed &&
            status.RuntimeStatus != OrchestrationRuntimeStatus.Failed &&
            status.RuntimeStatus != OrchestrationRuntimeStatus.Terminated)
        {
            log.LogWarning($"Orchestrator already running. InstanceId={instanceId}, Status={status.RuntimeStatus}");
            return;
        }

        // 新規開始
        log.LogInformation("Starting new orchestrator instance.");

        await starter.StartNewAsync("MainOrchestrator", instanceId, null);

        log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
    }
}