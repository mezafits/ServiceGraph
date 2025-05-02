 
    public static class IconServices
    {
        public static void ConstructMappings(WebApplication app)
        {
            app.MapGet("/Icons", (string query, SvgFileCache svgFileCache) =>
            {
                
                return svgFileCache.Search(query);
            });
        }
    }
 
