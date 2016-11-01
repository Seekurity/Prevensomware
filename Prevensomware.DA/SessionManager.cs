using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using Prevensomware.Dto;
using Environment = NHibernate.Cfg.Environment;

namespace Prevensomware.DA
{
    public static class SessionManager
    {
        private static ISession _session;
        private static readonly object SyncRoot = new object();
        public static ISession Session
        {
            get
            {
                lock (SyncRoot)
                {
                    return _session ?? GenerateSession();
                }
            }
        }

        private static void ExtractDatabaseFromResources(string extractionPath)
        {
            var databaseStream = Resource.Prevensomeware_db;
            if(databaseStream == null) throw new Exception("Can't create the database.");
            File.WriteAllBytes(extractionPath, databaseStream);
        }
        private static string GetConnectionString()
        {
            var appDatafolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
            var prevensomwareFolder = Path.Combine(appDatafolder, "Prevensomware");
            var fullDatabasePath = Path.Combine(prevensomwareFolder, "Prevensomeware_db.xrtyzwm");
            if (!Directory.Exists(prevensomwareFolder))
            {
                Directory.CreateDirectory(prevensomwareFolder);
            }
            if (!File.Exists(fullDatabasePath))
            {
                ExtractDatabaseFromResources(fullDatabasePath);
            }
            return $"Data Source={fullDatabasePath};Version=3;BinaryGuid=False";
        }
        private static ISession GenerateSession()
        {
            var cfg = new Configuration()
                .AddProperties(new Dictionary<string, string>
                {
                    {Environment.ConnectionDriver, typeof (SQLite20Driver).FullName},
                    {Environment.Dialect, typeof (SQLiteDialect).FullName},
                    {Environment.ConnectionProvider, typeof (DriverConnectionProvider).FullName},
                    {Environment.ConnectionString, GetConnectionString()},
                    {Environment.QuerySubstitutions, "true=1;false=0"},
                    {Environment.ShowSql, "true"}
                });
            cfg.AddClass(typeof(DtoFileInfo));
            cfg.AddClass(typeof(DtoLog));
            cfg.AddClass(typeof(DtoRegistryKey));
            cfg.AddClass(typeof(DtoRegistryValue));

            var sessions = cfg.BuildSessionFactory();
            _session = sessions.OpenSession();
            return _session;
        }
    }
}
