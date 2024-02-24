namespace NovaFinds.IFR.Configuration.Docker
{
    public static class DockerCheck
    {
        public static bool IsRunningInDocker()
        {
            try{
                if (File.ReadAllText("/proc/1/cgroup").Contains("docker") || File.ReadAllText("/proc/1/cgroup").Contains("kubepods")){ return true; }
            }
            catch{}

            return false;
        }
    }
}