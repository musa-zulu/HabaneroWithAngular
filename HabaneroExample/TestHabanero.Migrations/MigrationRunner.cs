using System;
using System.Reflection;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using Habanero.Base;
using Habanero.DB;

namespace TestHabanero.Migrations
{
    public class MigrationsRunner<TMigrations>
    {
        public IDatabaseConfig DatabaseConfig { get; private set; }
        public MissingDBFallBackOptions MissingDbFallBackOption { get; set; }

        public MigrationsRunner(IDatabaseConfig databaseConfig, MissingDBFallBackOptions missingDbFallBackOption)
        {
            if (databaseConfig == null) throw new ArgumentNullException("databaseConfig");
            DatabaseConfig = databaseConfig;
            MissingDbFallBackOption = missingDbFallBackOption;
        }

        public void MigrateToLatest()
        {
            MigrateTo(-1);
        }

        public void MigrateTo(long targetVersion)
        {
            var runner = CreateMigrationRunner();
            if (targetVersion == -1)
                runner.MigrateUp(true);
            else
                runner.MigrateUp(targetVersion, true);
        }

        private MigrationRunner CreateMigrationRunner()
        {
            var announcer = new TextWriterAnnouncer(s => System.Diagnostics.Debug.WriteLine(s));
            var type = typeof(TMigrations);
            var assembly = Assembly.GetAssembly(type);

            var migrationContext = new RunnerContext(announcer)
            {
                Namespace = type.Namespace
            };

            if (!DatabaseExists())
            {
                HandleMissingDatabase();
            }
            else
            {
                //CheckForAndRemoveOldVersionInfoKey();
            }

            var options = new MigrationOptions { PreviewOnly = false, Timeout = 10000 };
            var factory = new FluentMigrator.Runner.Processors.SqlServer.SqlServer2008ProcessorFactory();
            var connectionString = DatabaseConfig.GetConnectionString();
            var processor = factory.Create(connectionString, announcer, options);
            var runner = new MigrationRunner(assembly, migrationContext, processor);
            return runner;
        }

        public void MigrateDownTo(long targetVersion)
        {
            var runner = CreateMigrationRunner();
            runner.MigrateDown(targetVersion, true);
        }

        private void HandleMissingDatabase()
        {
            switch (MissingDbFallBackOption)
            {
                case MissingDBFallBackOptions.CreateDatabase:
                    CreateDatabase();
                    break;
                case MissingDBFallBackOptions.ThrowMissingDatabaseError:
                    throw new Exception(
                        string.Format("The database '{0}' is missing. Please create the database before continuing.",
                                      DatabaseName));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string DatabaseName
        {
            get { return DatabaseConfig.Database; }
        }

        private bool DatabaseExists()
        {
            var sql = string.Format("SELECT name FROM master.dbo.sysdatabases WHERE name = '{0}'", DatabaseName);
            var masterDatabaseConnection = GetMasterDatabaseConnection();
            var existingDatabaseName = Convert.ToString(masterDatabaseConnection.ExecuteRawSqlScalar(sql));
            return existingDatabaseName.ToLower() == DatabaseName.ToLower();
        }

        private void CheckForAndRemoveOldVersionInfoKey()
        {
            var sql = string.Format("IF EXISTS (SELECT name FROM [{0}].dbo.sysindexes WHERE name = 'PK_VersionInfo') ALTER TABLE  [{0}].[dbo].[VersionInfo] DROP CONSTRAINT [PK_VersionInfo]", DatabaseName);
            var masterDatabaseConnection = GetMasterDatabaseConnection();
            var result = masterDatabaseConnection.ExecuteRawSql(sql);
        }

        private IDatabaseConnection GetMasterDatabaseConnection()
        {
            var databaseConfig = GetMasterDatabaseConfig();
            var masterDatabaseConnection = databaseConfig.GetDatabaseConnection();
            return masterDatabaseConnection;
        }

        private DatabaseConfig GetMasterDatabaseConfig()
        {
            return new DatabaseConfig(DatabaseConfig.Vendor, DatabaseConfig.Server, "master", DatabaseConfig.UserName, DatabaseConfig.Password, DatabaseConfig.Port);
        }

        private void CreateDatabase()
        {
            var masterDatabaseConnection = GetMasterDatabaseConnection();
            masterDatabaseConnection.ExecuteRawSql(string.Format("CREATE DATABASE {0}", DatabaseName));
            if (!DatabaseExists())
            {
                throw new Exception(string.Format("The database '{0}' could not be created successfully.", DatabaseName));
            }
        }

    }

    internal class MigrationOptions : IMigrationProcessorOptions
    {
        public bool PreviewOnly { get; set; }
        public int Timeout { get; set; }
        public string ProviderSwitches { get; }
    }

    public enum MissingDBFallBackOptions
    {
        CreateDatabase,
        ThrowMissingDatabaseError
    }
}
