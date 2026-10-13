using Sparc.DatabaseInstaller.Sources.DatabaseInstaller.Enum;

namespace Sparc.DatabaseInstaller.Sources.ScriptLoader
{
    public class ScriptName
    {
        /// <summary>
        /// Preffix for update script
        /// </summary>
        public string Preffix_Update { get; set; }

        /// <summary>
        /// Preffix for create script
        /// </summary>
        public string Preffix_Create { get; set; }

        /// <summary>
        /// suffix
        /// </summary>
        public string Suffix = ".sql";
        
        /// <summary>
        /// Версия хранится в Базе данных
        /// </summary>
        public EnumVersionPlace Version_InDB { get; set; }

        public ScriptName(string preffix_Update, string preffix_Create, EnumVersionPlace version_InDB)
        {
            Preffix_Create = preffix_Create;
            Preffix_Update = preffix_Update;
            Version_InDB = version_InDB;
        }
    }
}
