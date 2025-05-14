namespace ServiceGraph.Web.Services
{
    public class IconsUtility
    {
        private readonly HttpClient httpClient;
        public IconsUtility(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<SvgFileInfo>> GetIcons()
        {
            return await httpClient.GetFromJsonAsync<List<SvgFileInfo>>("/Icons?query=*") ?? new List<SvgFileInfo>();
        }
    }
}
