using DBInstaller.Interface;
using Sparc.DatabaseInstaller.Sources.DatabaseInstaller;
using Sparc.DatabaseInstaller.Sources.DatabaseInstaller.Enum;
using Sparc.DatabaseInstaller.Sources.ScriptLoader;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;

namespace DBConfiguration
{
    [Export(typeof(IDBInstaller))]
    public class ConfigurationScript : IDBInstaller
    {
        private DatabaseInstaller _dbInst;

        public DbInfo DbInfo;

        private ScriptName _scriptN;

        public ConfigurationScript()
        {
            _scriptN = new ScriptName("DBConfiguration.Scripts.For_Update.", "DBConfiguration.Scripts.For_Create.", EnumVersionPlace.VersionDB);

            _dbInst = new DatabaseInstaller();

            ScriptLoader();
        }

        /// <summary>
        /// Загрузить скрипты на создание и обновление базы данных.
        /// </summary>
        private void ScriptLoader()
        {
            _dbInst.CreateScriptLoader = new ResourceScriptLoader(Assembly.GetCallingAssembly(),
                _scriptN.Preffix_Create,
                _scriptN.Suffix);
            _dbInst.UpdateScriptLoader = new ResourceScriptLoader(Assembly.GetCallingAssembly(),
                _scriptN.Preffix_Update,
                _scriptN.Suffix);
        }

        public void InstallDB(InstallOptions options, ILogger logger)
        {
            CheckDB(logger);
            _dbInst.Install(options, _scriptN);
            CheckDB(logger);
        }

        public void CheckDB(ILogger logger)
        {
            _dbInst.Logger = logger;
            DbInfo = _dbInst.Check("KPK2", "Version", _scriptN);
        }

        public void BackUp(ILogger logger)
        {
            _dbInst.Logger = logger;
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "Sparc\\KPK2\\Database");
            _dbInst.BackUp(folderPath, _scriptN);
        }

        public void Restore(string path, ILogger logger)
        {
            _dbInst.Logger = logger;
            _dbInst.Restore(path);
        }
    }
}