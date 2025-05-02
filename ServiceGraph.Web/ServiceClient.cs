using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using ServiceGraph.Common;
public class ServiceClient(HttpClient httpClient)
{
    public async Task<List<Project>> GetProjectsAsync(string User)
    {
        try
        {
            var response = await httpClient.GetAsync($"/Projects/{User}");

            // Read JSON string explicitly
            var jsonString = await response.Content.ReadAsStringAsync();

            // Debug: Print or log the JSON result to inspect
            Console.WriteLine("JSON Response:\n" + jsonString);

            // Attempt deserialization safely
            var results = JsonSerializer.Deserialize<List<Project>>(jsonString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return results ?? new List<Project>();
        }
        catch (System.Text.Json.JsonException jsonEx)
        {
            // Log detailed JSON deserialization errors
            Console.WriteLine($"Deserialization failed: {jsonEx.Message}");
            throw; // or handle accordingly
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            throw;
        }
    }

    public async Task<ModelPostResponse> UpsertProject(Project project)
    {
        try
        {
            var httpResponse = await httpClient.PutAsJsonAsync("/Project", project);

            // Initialize with a success state
            var mpr = new ModelPostResponse
            {
                HasError = false,
                Errors = new List<Exception>()
            };

            // Check if the HTTP response indicates a failure
            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorContent = await httpResponse.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<ModelPostResponse>(errorContent);

                if (errorResponse != null)
                {
                    return errorResponse;
                }
                else
                {
                    // Return a generic error if the response cannot be deserialized
                    mpr.HasError = true;
                    mpr.Errors.Add(new Exception("Unknown error occurred while processing the response."));
                }
            }

            return mpr;
        }
        catch (Exception err)
        {
            // Handle any exceptions during the process
            return new ModelPostResponse { HasError = true, Errors = new List<Exception> { err } };
        }
    }
    public async Task<ModelPostResponse> ImportProjectAsync(Project project)
    {
        try
        {
            var httpResponse = await httpClient.PostAsJsonAsync("/Import", project);

            var mpr = new ModelPostResponse
            {
                HasError = !httpResponse.IsSuccessStatusCode,
                Errors = new List<Exception>()
            };

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorContent = await httpResponse.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<ModelPostResponse>(errorContent);
                mpr = errorResponse ?? mpr;
            }
            else
            {
                // If the operation is successful, you can update the ModelPostResponse accordingly
                mpr.HasError = false;
            }

           

            return mpr;
        }
        catch (Exception err)
        {
            return new ModelPostResponse { HasError = true, Errors = new List<Exception> { err } };
        }
    }
    public async Task<HttpResponseMessage> ExportProjectAsync(string projectId)
    {
        try
        {
            var httpResponse = await httpClient.GetAsync($"/export?projectId={projectId}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                // Handle the case where the server response is not successful
                throw new HttpRequestException($"Request failed with status code {httpResponse.StatusCode}");
            }

            return httpResponse;
        }
        catch (Exception ex)
        {
            // Handle other exceptions that may occur
            throw new Exception("An error occurred while exporting data collection: " + ex.Message, ex);
        }
    }
    public async Task<List<SvgFileInfo>> GetIcons()
    {
       return await httpClient.GetFromJsonAsync<List<SvgFileInfo>>("/Icons?query=*") ?? new List<SvgFileInfo>();
    }
}

 
    //public async Task<DataCollection> GetServicesAsync()
    //{
    //     return await httpClient.GetFromJsonAsync<DataCollection>("/Services") ?? null;
    //}
 //public async Task<ModelPostResponse> InsertServiceNode(ServiceNode node)
    //{
    //    try
    //    {
    //        var httpResponse = await httpClient.PostAsJsonAsync("/Services", node);

    //        var mpr = new ModelPostResponse
    //        {
    //            HasError = true,
    //            Errors = new List<Exception> { new Exception("Unknown error occurred.") }
    //        };

    //        if (!httpResponse.IsSuccessStatusCode)
    //        {
    //            var errorContent = await httpResponse.Content.ReadAsStringAsync();
    //            var errorResponse = JsonConvert.DeserializeObject<ModelPostResponse>(errorContent);
    //            mpr = errorResponse ?? mpr;

    //        }

    //        return mpr;

    //    }
    //    catch (Exception err)
    //    {
    //        return new ModelPostResponse { HasError = true, Errors = new List<Exception> { err } };
    //    }
    //}
    //public async Task<ModelPostResponse> UpsertServiceNode(ServiceNode node)
    //{
    //    try
    //    {
    //        var httpResponse = await httpClient.PutAsJsonAsync("/UpdateNode", node);

    //        // Initialize with a success state
    //        var mpr = new ModelPostResponse
    //        {
    //            HasError = false,
    //            Errors = new List<Exception>()
    //        };

    //        // Check if the HTTP response indicates a failure
    //        if (!httpResponse.IsSuccessStatusCode)
    //        {
    //            var errorContent = await httpResponse.Content.ReadAsStringAsync();
    //            var errorResponse = JsonConvert.DeserializeObject<ModelPostResponse>(errorContent);

    //            if (errorResponse != null)
    //            {
    //                return errorResponse;
    //            }
    //            else
    //            {
    //                // Return a generic error if the response cannot be deserialized
    //                mpr.HasError = true;
    //                mpr.Errors.Add(new Exception("Unknown error occurred while processing the response."));
    //            }
    //        }

    //        return mpr;
    //    }
    //    catch (Exception err)
    //    {
    //        // Handle any exceptions during the process
    //        return new ModelPostResponse { HasError = true, Errors = new List<Exception> { err } };
    //    }
    //}
    //public async Task<bool> AddEdge(Edge edge)
    //{
    //    var results = await httpClient.PostAsJsonAsync<Edge>($"/Edge",edge);
    //    if (results.IsSuccessStatusCode)
    //        return true;
    //    else
    //        return false;
    //}
    //public async Task<bool> RemoveEdge(string id)
    //{
    //    var results = await httpClient.DeleteAsync($"/Edge/{id}");
    //    if (results.IsSuccessStatusCode)
    //        return true;
    //    else
    //        return false;
    //}
    //public async Task<bool> RemoveServiceNode(string id)
    //{
    //    var results = await httpClient.DeleteAsync($"/Services/{id}");
    //    if(results.IsSuccessStatusCode)
    //        return true;
    //    else
    //        return false;   
    //}
    //public async Task<ServiceNode> GetServiceNodeAsync(string id)
    //{
    //    return await httpClient.GetFromJsonAsync<ServiceNode>($"/Services/{id}") ?? null;
    //}   