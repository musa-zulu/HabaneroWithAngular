using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.Console;
using Habanero.DB;
using TestHabanero.Bootstrap;
using TestHabanero.BO;
using TestHabanero.Migrations;
using BORegistry = Habanero.BO.BORegistry;

namespace TestHabanero
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            IocContainer.Setup();


            GlobalRegistry.ApplicationName = "TestHabanero";
            GlobalRegistry.ApplicationVersion = "1.32.05 - 2016.03.08";
            var testHabaneroApp = new HabaneroAppConsole(GlobalRegistry.ApplicationName,
                                                     GlobalRegistry.ApplicationVersion);
            testHabaneroApp.LoadClassDefs = false;
            var databaseConfig = DatabaseConfig.ReadFromConfigFile();

//            if (GetCurrentVerison() < 102)
//            {
//                GlobalRegistry.Settings = new DatabaseSettings("settings", DatabaseConnection.CurrentConnection);
//                var scriptPath = Server.MapPath("~") + "\\Content\\Scripts";
//                DatabaseMigrator.ProcessMigrateCommand(scriptPath);
//            }
            DatabaseConnection.CurrentConnection = databaseConfig.GetDatabaseConnection();
            BORegistry.DataAccessor = new DataAccessorDB();
            BOBroker.LoadClassDefs();

            var connectionString = GetConnectionString();
            MigrateDatabaseWith(connectionString);
        
        }
        private int GetCurrentVerison()
        {
            var sql = "Select SettingValue from settings where SettingName='DATABASE_VERSION'";
            var databaseConnection = DatabaseConnection.CurrentConnection;
            var dataReader = databaseConnection.LoadDataReader(sql);
            dataReader.Read();
            var version = Convert.ToInt32(dataReader["SettingValue"].ToString());
            return version;
        }

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        }

        private void MigrateDatabaseWith(string connectionString)
        {
            var runner = new Migrator(connectionString);
            runner.MigrateToLatest();
        }

    }
}
