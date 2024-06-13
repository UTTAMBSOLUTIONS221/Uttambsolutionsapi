namespace Uttambsolutionsapi
{
    public class Util
    {
        public static string ShareConnectionString(IConfiguration config)
        {
            return config["ConnectionStrings:DatabaseConnection"];
        }
    }
}
