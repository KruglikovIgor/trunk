namespace DBInstaller.Interface
{
    public interface IDBInstaller
    {
        void InstallDB(InstallOptions options, ILogger logger);

        void CheckDB(ILogger logger);

        void BackUp(ILogger logger);

        void Restore(string path, ILogger logger);
    }
}
