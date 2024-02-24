namespace NovaFinds.IFR.Configuration
{
    using Docker;
    using Microsoft.Extensions.Configuration;

    public class Configuration
    {
        public IConfiguration Config { get; }

        public Configuration()
        {
            var path = Directory.GetCurrentDirectory() + "/bin/Debug/net8.0/Configuration";
            var filename = "appsettings.json";
            var isRunningInDocker = DockerCheck.IsRunningInDocker();
            if (isRunningInDocker){
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