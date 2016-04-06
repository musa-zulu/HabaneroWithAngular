using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Habanero.Base;
using Habanero.Base.Logging;
using Habanero.DB;

namespace TestHabanero.Migrations
{
    public class DatabaseMigrator
    {
        public const string DATABASE_VERSION_SETTING_NAME = "DATABASE_VERSION";

        protected static readonly IHabaneroLogger _logger = GlobalRegistry.LoggerFactory.GetLogger("DatabaseMigrator");

        public static void ProcessMigrateCommand(string scriptPath)
        {
            var dbVersion = GetDbVersion();
            if (dbVersion == -1) return;

            (from fileName in Directory.EnumerateFiles(scriptPath, "*.sql")
             let name = Path.GetFileName(fileName)
             where name != null
             let versionNumber = Convert.ToInt32(name.Substring(0, 3))
             where versionNumber > dbVersion
             select new { Name = fileName, Script = File.ReadAllText(fileName), VersionNumber = versionNumber })
                .ForEach(script =>
                {
                    _logger.Log(string.Format("Processing script {0}...", script.Name), LogCategory.Info);
                    try
                    {
                        DatabaseConnection.CurrentConnection.ExecuteRawSql(script.Script);
                        GlobalRegistry.Settings.SetString(DATABASE_VERSION_SETTING_NAME, script.VersionNumber.ToString());
                        _logger.Log("Done", LogCategory.Info);
                    }
                    catch (Exception ex)
                    {
                        _logger.Log("DatabaseMigrator Error:", ex, LogCategory.Error);
                        throw new Exception(ex.Message, ex.InnerException);
                    }
                });
            _logger.Log("Migration complete, db is now at version " + GetDbVersion(), LogCategory.Debug);
        }

        public static int GetDbVersion()
        {
            try
            {
                return Convert.ToInt32(GlobalRegistry.Settings.GetString(DATABASE_VERSION_SETTING_NAME));
            }
            catch (DatabaseReadException ex)
            {
                if (ex.InnerException is SqlException && ex.InnerException.Message.Contains("Invalid object name 'settings'"))
                {
                    CreateSettingsTable();
                    GlobalRegistry.Settings.SetString(DATABASE_VERSION_SETTING_NAME, "0");
                }
                else
                {
                    Console.Out.WriteLine("Couldn't retrieve setting {0}", DATABASE_VERSION_SETTING_NAME);
                    Console.Out.WriteLine(ex.Message);
                    if (ex.InnerException != null) Console.Out.WriteLine(ex.InnerException.Message);
                    return -1;
                }
                return 0;
            }
        }

        public static void CreateSettingsTable()
        {
            DatabaseConnection.CurrentConnection.ExecuteRawSql(
                @"CREATE TABLE dbo.[settings](
					[SettingName] [varchar](255) NOT NULL,
					[SettingValue] [varchar](255) NOT NULL,
					[StartDate] [datetime] NOT NULL,
					[EndDate] [datetime] NULL
				) ON [PRIMARY];");
        }
    }
}