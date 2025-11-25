using Azure;
using Azure.Data.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class TimerStarter
{
    private readonly ILogger _logger;
    private readonly TableClient _tableClient;

    public TimerStarter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<TimerStarter>();

        // Durable Functions の Instances テーブルに接続
        string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        _tableClient = new TableClient(connectionString, "Instances");
    }

    [Function("TimerStarter")]
    public async Task Run([TimerTrigger("*/30 * * * * *")] TimerInfo timer)
    {
        string instanceId = "single-orchestrator";

        // Instances テーブルから 1行読み取る
        try
        {
            var entity = await _tableClient.GetEntityAsync<TableEntity>(
                partitionKey: instanceId,
                rowKey: string.Empty // Instances の RowKey は空文字固定
            );

            string runtimeStatus = entity.Value.GetString("RuntimeStatus");

            // Running / Pending / ContinuedAsNew などは再起動しない
            if (runtimeStatus != "Completed" &&
                runtimeStatus != "Failed" &&
                runtimeStatus != "Terminated")
            {
                _logger.LogInformation($"既存インスタンスが動作中のため起動しません: {runtimeStatus}");
                return;
            }
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            // 404 = 存在しない（初回起動OK）
            _logger.LogInformation("既存インスタンスなし → 起動可能");
        }

        // ここでオーケストレーションを起動する
        _logger.LogInformation("オーケストレーションを起動します");
        // DurableClient を使う部分は省略（あなたの環境に合わせる）
    }
}