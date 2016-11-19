using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using Prevensomware.DA;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public class AppStartupConfigurator
    {
        public Action<string,LogType> LogDelegate { get; set; }
        private static readonly Random Random = new Random();

        public DtoUserSettings GenerateInitialUserSettings()
        {
            var dtoUserSettings = new DtoUserSettings
            {
                ServiceInfo = GenerateInitialServiceInfo(),
                CreateDateTime = DateTime.Now
            };
            dtoUserSettings.SelectedFileExtensionList = GenerateInitialExtensionFileList(dtoUserSettings);
            new BoUserSettings().Save(dtoUserSettings);

            return dtoUserSettings;
        }

        private DtoServiceInfo GenerateInitialServiceInfo()
        {
            var serviceInfo = new DtoServiceInfo
            {
                CreateDateTime = DateTime.Now,
                Interval = 5,
                Name = "Prevensomware",
                NextServiceRunDateTime = DateTime.Now.AddHours(5)
            };
            new BoServiceInfo().Save(serviceInfo);
            return serviceInfo;
        }

        private IList<DtoFileInfo> GenerateInitialExtensionFileList(DtoUserSettings dtoUserSettings)
        {
            var initialExtensionArray = new[] { "yuv", "ycbcra", "xis", "wpd", "tex", "sxg", "stx", "srw", "srf", "sqlitedb", "sqlite3",
                "sqlite", "sdf", "sda", "s3db", "rwz", "rwl", "rdb", "rat", "raf", "qby", "qbx", "qbw", "qbr", "qba", "psafe3", "plc", "plus_muhd",
                "pdd", "oth", "orf", "odm", "odf", "nyf", "nxl", "nwb", "nrw", "nop", "nef", "ndd", "myd", "mrw", "moneywell", "mny", "mmw",
                "mfw", "mef", "mdc", "lua", "kpdx", "kdc", "kdbx", "jpe", "incpas", "iiq", "ibz", "ibank", "hbk", "gry", "grey", "gray", "fhd",
                "ffd", "exf", "erf", "erbsql", "eml", "dxg", "drf", "dng", "dgc", "des", "der", "ddrw", "ddoc", "dcs", "db_journal", "csl", "csh",
                "crw", "craw", "cib", "cdrw", "cdr6", "cdr5", "cdr4", "cdr3", "bpw", "bgt", "bdb", "bay", "bank", "backupdb", "backup", "back", "awg",
                "apj", "ait", "agdl", "ads", "adb", "acr", "ach", "accdt", "accdr", "accde", "vmxf", "vmsd", "vhdx", "vhd", "vbox", "stm", "rvt", "qcow",
                "qed", "pif", "pdb", "pab", "ost", "ogg", "nvram", "ndf", "m2ts", "log", "hpp", "hdd", "groups", "flvv", "edb", "dit", "dat", "cmt", "bin", 
                "aiff", "xlk", "wad", "tlg", "say", "sas7bdat", "qbm", "qbb", "ptx", "pfx", "pef", "pat", "oil", "odc", "nsh", "nsg", "nsf", "nsd", "mos", "indd",
                "iif", "fpx", "fff", "fdb", "dtd", "design", "ddd", "dcr", "dac", "cdx", "cdf", "blend", "bkp", "adp", "act", "xlr", "xlam", "xla", "wps",
                "tga", "pspimage", "pct", "pcd", "fxg", "flac", "eps", "dxb", "drw", "dot", "cpi", "cls", "cdr", "arw", "aac", "thm", "srt", "save", "safe",
                "pwm", "pages", "obj", "mlb", "mbx", "lit", "laccdb", "kwm", "idx", "html", "flf", "dxf", "dwg", "dds", "csv", "css", "config", "cfg", "cer", "asx",
                "aspx", "aoi", "accdb", "7zip", "xls", "wab", "rtf", "prf", "ppt", "oab", "msg", "mapimail", "jnt", "doc", "dbx", "contact", "mid", "wma", "flv", "mkv",
                "mov", "avi", "asf", "mpeg", "vob", "mpg", "wmv", "fla", "swf", "wav", "qcow2", "vdi", "vmdk", "vmx", "wallet", "upk", "sav", "ltx", "litesql", "litemod",
                "lbf", "iwi", "forge", "das", "d3dbsp", "bsa", "bik", "asset", "apk", "gpg", "aes", "ARC", "PAQ", "tar",
                "bz2", "tbk", "bak", "tar", "tgz", "rar", "zip", "djv", "djvu", "svg", "bmp", "png", "gif", "raw", "cgm", "jpeg", "jpg", "tif", "tiff", "NEF", "psd", "cmd",
                "bat", "class", "jar", "java", "asp", "brd", "sch", "dch", "dip", "vbs", "asm", "pas", "cpp", "php", "ldf", "mdf", "ibd", "MYI", "MYD", "frm", "odb",
                "dbf", "mdb", "sql", "SQLITEDB", "SQLITE3", "pst", "onetoc2", "asc", "lay6", "lay", "ms11", "sldm", "sldx", "ppsm", "ppsx", "ppam", "docb", "mml", "sxm",
                "otg", "odg", "uop", "potx", "potm", "pptx", "pptm", "std", "sxd", "pot", "pps", "sti", "sxi", "otp", "odp", "wks", "xltx", "xltm", "xlsx", "xlsm", "xlsb",
                "slk", "xlw", "xlt", "xlm", "xlc", "dif", "stc", "sxc", "ots", "ods", "hwp", "dotm", "dotx", "docm", "docx", "DOT", "max", "xml", "txt", "CSV", "uot", "RTF",
                "pdf", "XLS", "PPT", "stw", "sxw", "ott", "odt", "DOC", "pem", "csr", "crt", "key", "mp4", "vcf", "chm", "epub" };

            return initialExtensionArray.Select(extension => new DtoFileInfo
            {
                CreateDateTime = DateTime.Now, UserSettings = dtoUserSettings, OriginalExtension = "." + extension, ReplacedExtension = "." + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(extension))
            }).ToList();
        }
        public bool TestAppOnStartUp()
        {
            LogDelegate?.Invoke("Starting App Startup Test.", LogType.Info);
            var checkSucceeded = false;
            if (!IsDatabaseAccessible())
                LogDelegate?.Invoke("Startup Test: Couldn't access/create the databasse.", LogType.Error);
            else
            {
                LogDelegate?.Invoke("Startup Test: Database is accessible.", LogType.Success);
                checkSucceeded = true;
            }
            if (!IsAppPathInRegistryCorrect())
                LogDelegate?.Invoke("Startup Test: Couldn't read/write to Windows Registry.", LogType.Error);
            else
            {
                LogDelegate?.Invoke("Startup Test: Windows Registry is accessible.", LogType.Success);
                checkSucceeded = true;
            }
            if (!IsUserFilesAccessible())
                LogDelegate?.Invoke("Startup Test: Couldn't edit local Files.", LogType.Error);
            else
            {
                LogDelegate?.Invoke("Startup Test: Local Files are accessible.", LogType.Success);
                checkSucceeded = true;
            }
            return checkSucceeded;
        }

        private bool IsDatabaseAccessible()
        {
            try
            {
                var fileInfoRepository = new FileInfoRepository();
                var testFileInfoObject = new DtoFileInfo {CreateDateTime = DateTime.Now};
                fileInfoRepository.CreateOrUpdate(testFileInfoObject);
                fileInfoRepository.Remove(testFileInfoObject);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool IsAppPathInRegistryCorrect()
        {
            var currentAppPathValue = Registry.GetValue(@"HKEY_CLASSES_ROOT\*\shell\Revert File\command","",null);
            if (currentAppPathValue != null)
            {
                var appValueList = currentAppPathValue.ToString().Split(' ');
                var registerdAppExecutablePath = appValueList.First();
                if ((appValueList.Length > 1 && System.IO.Path.GetFullPath(registerdAppExecutablePath) != System.Reflection.Assembly.GetEntryAssembly().Location)
                     || appValueList.Length <= 1)
                {
                    try
                     {
                        Registry.ClassesRoot.OpenSubKey(@"*\shell\Revert File\command", true)
                            .SetValue("", System.Reflection.Assembly.GetEntryAssembly().Location + " \"%1\"");
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }

            }
            else
                try
                {
                    var registryKey = Registry.ClassesRoot.OpenSubKey("*").OpenSubKey("shell", true);
                    registryKey = registryKey.CreateSubKey("Revert File");
                    registryKey = registryKey.CreateSubKey("command");
                    registryKey.SetValue("", System.Reflection.Assembly.GetEntryAssembly().Location + " \"%1\"");
                    return true;
                }
                catch
                {
                    return false;
                }
            return true;
        }
        private bool IsUserFilesAccessible()
        {
            try
            {
               System.IO.File.Create(
                    System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                        "Prevensomware\\testFile.txt")).Close();
                System.IO.File.Delete(
                    System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                        "Prevensomware\\testFile.txt"));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
