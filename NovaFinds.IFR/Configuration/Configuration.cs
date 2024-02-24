namespace NovaFinds.IFR.Configuration
{
    using Microsoft.Extensions.Configuration;

    public class Configuration
    {
        public IConfiguration Config { get; }

        public Configuration()
        {
            var path = Directory.GetCurrentDirectory() + "/bin/Debug/net8.0/Configuration";
            var filename = "appsettings.json";
            var runningContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
            if (runningContainer != null && bool.Parse(runningContainer)){
                path = "/app/Configuration";
                filename = "appsettings.docker.json";
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile(filename, optional: true, reloadOnChange: true);
            this.Config = builder.Build();
        }
    }
}