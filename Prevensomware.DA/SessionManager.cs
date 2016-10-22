using NHibernate;
using NHibernate.Cfg;
using Prevensomware.Dto;

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

        private static ISession GenerateSession()
        {
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddClass(typeof (DtoFileInfo));
            cfg.AddClass(typeof(DtoLog));
            cfg.AddClass(typeof(DtoRegistryKey));
            cfg.AddClass(typeof(DtoRegistryValue));

            var sessions = cfg.BuildSessionFactory();
            _session = sessions.OpenSession();
            return _session;
        }
    }
}
