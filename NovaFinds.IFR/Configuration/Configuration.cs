namespace NovaFinds.IFR.Configuration
{
    using Microsoft.Extensions.Configuration;

    public class Configuration
    {

        public IConfiguration Config { get; }

        public Configuration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()+"/bin/Debug/net8.0/Configuration")
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            this.Config = builder.Build();
        }
    }
}