using Kusto.Data.Net.Client;
using Kusto.Data;
using Microsoft.Identity.Web;
using Kusto.Data.Common;
using System.Linq;
public interface IKustoQueryService
{
    Task<List<string>> RunSampleQueryAsync();
}
public class KustoQueryService : IKustoQueryService
{
    private readonly ITokenAcquisition _tokenAcquisition;

    public KustoQueryService(ITokenAcquisition tokenAcquisition)
    {
        _tokenAcquisition = tokenAcquisition;
    }

    public async Task<List<string>> RunSampleQueryAsync()
    {
        var token = await _tokenAcquisition.GetAccessTokenForUserAsync(
            new[] { "https://icmcluster.kusto.windows.net/.default" });

        var kcsb = new KustoConnectionStringBuilder("https://icmcluster.kusto.windows.net")
            .WithAadUserTokenAuthentication(token);

        using var client = KustoClientFactory.CreateCslQueryProvider(kcsb);
        var query = @"IncidentsSnapshotV2() | where OwningTenantName == 'Azure Monitor Essentials'| where SourceOrigin  == 'Monitor'| where isnotempty(Mitigation)| summarize  count() by MonitorId";
        var requestProps = new ClientRequestProperties();
        requestProps.ClientRequestId = $"ServiceGraph;{Guid.NewGuid()}";
        requestProps.SetOption("servertimeout", TimeSpan.FromSeconds(30));
            //icmcluster/IcmDataWarehouse
        using var reader = client.ExecuteQuery("IcmDataWarehouse", query, requestProps);

        var results = new List<string>();
        while (reader.Read())
            results.Add(reader[0].ToString());

        return results;
    }
}
