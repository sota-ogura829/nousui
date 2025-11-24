using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.DurableTask;

namespace DurableSingleInstanceApp
{
    public class TimerStartFunction
    {
        private readonly ILogger _logger;

        public TimerStartFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TimerStartFunction>();
        }

        [Function("TimerTriggerStarter")]
        public async Task RunAsync(
            [TimerTrigger("0 */1 * * * *")] TimerInfo timer,
            [DurableClient] DurableTaskClient client)
        {
            string instanceId = "MySingleInstance";   // 固定インスタンスID

            // ① 既存のオーケストレーション状態を取得
            OrchestrationMetadata? existing = await client.GetInstanceAsync(instanceId);

            // ② 実行中かチェック
            if (existing != null &&
                (existing.RuntimeStatus == OrchestrationRuntimeStatus.Running ||
                 existing.RuntimeStatus == OrchestrationRuntimeStatus.Pending))
            {
                _logger.LogInformation($"すでに実行中のため新規起動しません。InstanceId = {instanceId}");
                return;
            }

            // ③ 未実行 or 完了しているので新規起動
            _logger.LogInformation($"オーケストレーションを開始します。InstanceId = {instanceId}");

            await client.ScheduleNewOrchestrationInstanceAsync(
                orchestratorName: "MainOrchestrator",
                instanceId: instanceId);
        }
    }
}