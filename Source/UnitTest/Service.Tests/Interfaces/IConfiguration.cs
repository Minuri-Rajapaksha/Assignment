
namespace Service.Tests.Interfaces
{
    public interface IConfiguration : Microsoft.Extensions.Configuration.IConfiguration
    {
        string GetConnectionString(string name);

        string GetValue(string name);
    }
}
