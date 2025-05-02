
public class SvgFileCache
{
    private Dictionary<string, SvgFileInfo> cache = new Dictionary<string, SvgFileInfo>();

    public SvgFileCache(string path = "Icons")
    {    
        LoadSvgFiles(path);
    }

    private void LoadSvgFiles(string directoryPath)
    {
        foreach (var file in Directory.GetFiles(directoryPath, "*.svg", SearchOption.AllDirectories))
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            var parts = fileName.Split('-');

            if (parts.Length >= 4) // Ensure there are at least 4 parts
            {
                var fileInfo = new SvgFileInfo
                {
                    Id = parts[0],
                    Type = parts[1],
                    Subtype = parts[2],
                    Name = string.Join("-", parts.Skip(3)).Replace("-"," "), // Joining the remaining parts as Name
                    Content = File.ReadAllText(file)
                };
                cache[fileName] = fileInfo;
            }
        }
    }

    public List<SvgFileInfo> Search(string query)
    {
        if(query == "*")
            return cache.Values.ToList();  

        return cache.Values.Where(info => info.Name.Contains(query,StringComparison.InvariantCultureIgnoreCase))
                           .Take(20)
                           .ToList();
    }
}
