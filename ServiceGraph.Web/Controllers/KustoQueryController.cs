using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Client;
using Kusto.Data.Net.Client;
using Kusto.Data.Common;
using System.Threading.Tasks;
using System;
using Kusto.Data;

namespace ServiceGraph.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class KustoQueryController : Controller
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly string _kustoCluster = "https://icmcluster.kusto.windows.net";
        private readonly string _kustoDatabase = "";
        private readonly string[] _kustoScopes = new[] { "https://icmcluster.kusto.windows.net/.default" };

        public KustoQueryController(ITokenAcquisition tokenAcquisition)
        {
            _tokenAcquisition = tokenAcquisition;
        }

        [HttpGet]
        public async Task<IActionResult> Query(string query)
        {
            try
            {
                // Acquire token on behalf of the signed-in user
                var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(_kustoScopes);

                // Build the connection string with user token
                var kcsb = new KustoConnectionStringBuilder(_kustoCluster)
                    .WithAadUserTokenAuthentication(accessToken);

                using var queryProvider = KustoClientFactory.CreateCslQueryProvider(kcsb);

                var requestProps = new ClientRequestProperties();
                requestProps.ClientRequestId = $"ServiceGraph;{Guid.NewGuid()}";
                requestProps.SetOption("servertimeout", TimeSpan.FromSeconds(30));

                using var reader = queryProvider.ExecuteQuery(_kustoDatabase, query, requestProps);
                

                var results = new System.Text.StringBuilder();
                while (reader.Read())
                {
                    results.AppendLine(reader[0].ToString());
                }

                return Ok(results.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest($"Error querying Kusto: {ex.Message}");
            }
        }
    }
}