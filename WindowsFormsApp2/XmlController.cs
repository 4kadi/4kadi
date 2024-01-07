using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Security;
using System.Xml;
using System.Windows.Forms;

namespace KontrolaKadi
{
    public static class XmlController
    {
        static string XmlNotEncriptedPath;
        static string XmlEncriptedPath;
        static string XmlEncriptedPath_tmp;
        static bool forceRefresh = false;
        public static bool encriptedMode = false;
        static bool savingFilePleaseWait = false;

        public static bool XmlControllerInitialized = false;

        public static EventHandler XmlChanged = new EventHandler(XmlHasChanged);

        static void XmlHasChanged(object sender, EventArgs e)
        { 
            
        }

        // xml file
        private static XDocument _XmlFile;
        public static XDocument XmlFile
        {
            get
            {
                if (_XmlFile != null)
                {
                    return _XmlFile;
                }
                return null;
            }

            private set
            {
                _XmlFile = value;
            }
        }

        // General section of xml file
        private static XElement _XmlGeneral;
        public static XElement XmlGeneral
        {
            get
            {
                return _XmlGeneral;
            }

            private set
            {
                _XmlGeneral = value;
            }
        }

        // "Kadi" section of xml file
        private static XElement _XmlKadi;
        public static XElement XmlKadi
        {
            get
            {
                return _XmlKadi;
            }

            private set
            {
                _XmlKadi = value;
            }
        }


        // LOGO connection section of xml file
        private static XElement _XmlConn;
        public static XElement XmlConn
        {
            get
            {
                return _XmlConn;
            }

            private set
            {
                _XmlConn = value;
            }
        }

        private static void setBaseDirPath()
        {

            Val.BaseDirectoryPath = Directory.GetParent(Directory.GetParent(Application.StartupPath).FullName).FullName;
        }

        private static XDocument LoadNotEncriptedXML(string XmlPath)
        {
            try
            {
                string read;

                using (StreamReader s = new StreamReader(XmlPath))
                {
                    read = s.ReadToEnd();
                }

                return XDocument.Parse(read);
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading xml from file (before encription): " + ex.Message);
            }
        }

        private static XDocument LoadNotEncriptedXML()
        {
            return LoadNotEncriptedXML(XmlNotEncriptedPath);
        }

        private static XDocument LoadAndDecriptXml()
        {
            try
            {
                string read;

                using (StreamReader s = new StreamReader(XmlEncriptedPath))
                {
                    read = s.ReadToEnd();
                }

                var decripted = XmlEncription.Decrypt(read);

                return XDocument.Parse(decripted, LoadOptions.PreserveWhitespace);
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading xml from encripted file: " + ex.Message);
            }

        }

        static void SaveXML(string newContent, bool ChangesFromUser)
        {
            string text;

            savingFilePleaseWait = true;

            try
            {
                text = Settings.XmlDeclaration + newContent;
                var encriptedText = XmlEncription.Encrypt(text);

                using (StreamWriter s = new StreamWriter(XmlEncriptedPath_tmp, false, Encoding.UTF8))
                {
                    s.Write(Environment.NewLine + encriptedText);
                    s.Flush();
                    s.Dispose();
                }

                if (ChangesFromUser)
                {
                    SysLog.SetMessage("ConfigFile was changed, by user.");
                }
            }
            catch (Exception ex)
            {
                savingFilePleaseWait = false;
                var message = "Problem saving encripted config File." + ex.Message;
                throw new Exception(message);
            }

            try
            {
                if (!encriptedMode)
                {
                    if (File.Exists(XmlNotEncriptedPath))
                    {
                        using (StreamWriter s = new StreamWriter(XmlNotEncriptedPath, false, Encoding.UTF8))
                        {
                            s.Write(text);
                            s.Flush();
                            s.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                savingFilePleaseWait = false;
                var message = "Problem saving unecripted config File." + ex.Message;
                throw new Exception(message);
            }

            TmpToFile();
            savingFilePleaseWait = false;
        }

        static void TmpToFile()
        {
            try
            {
                if (File.Exists(XmlEncriptedPath_tmp))
                {
                    File.Copy(XmlEncriptedPath_tmp, XmlEncriptedPath, true);
                    File.Delete(XmlEncriptedPath_tmp);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Problem replacing tmp config file: " + ex.Message);
            }
        }

        static void FindFileAndEncript()
        {
            XmlNotEncriptedPath = Val.BaseDirectoryPath + "\\" + Settings.pathToConfigFile;
            XmlEncriptedPath = Val.BaseDirectoryPath + "\\" + Settings.pathToConfigFileEncripted;
            XmlEncriptedPath_tmp = Val.BaseDirectoryPath + "\\" + Settings.pathToConfigFileEncripted + "_tmp";

            var ii = "\"";

            // if there is Nonencripted config file (will be deleted automatically after publish)

            try
            {
                if (File.Exists(XmlNotEncriptedPath))
                {
                    if (File.Exists(XmlEncriptedPath))
                    {
                        try
                        {
                            File.Delete(XmlEncriptedPath);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error deleting encripted config file: " + ex.Message);
                        }

                    }

                    FileEncript(XmlNotEncriptedPath);
                }
                else
                {
                    if (!File.Exists(XmlEncriptedPath))
                    {
                        throw new Exception("Config file could not be found. Search was performed at locations: " + ii + XmlEncriptedPath + ii + " and " + ii + XmlNotEncriptedPath + ii + ".");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Procedure failed: " + ex.Message);
            }
        }

        static void FileEncript(string XmlPath)
        {
            string xmlText;
            string encriptedText;

            try
            {
                xmlText = LoadNotEncriptedXML(XmlPath).ToString();
                encriptedText = XmlEncription.Encrypt(xmlText);

                using (StreamWriter s = new StreamWriter(XmlEncriptedPath))
                {
                    s.Write(encriptedText);
                    s.Flush();
                    s.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Config file encription failed: " + ex.Message);
            }

        }

        public static void XmlControllerInitialize()
        {
            try
            {
                setBaseDirPath(); // get XML path                

                FindFileAndEncript();

                XmlFile = LoadAndDecriptXml();
                XmlChanged.Invoke("initialisation", null);
                SetClass();

                Misc.SmartThread refresher = new Misc.SmartThread(() => Refresher_Thread());
                refresher.Start("XmlRefresher", System.Threading.ApartmentState.MTA, true);

            }
            catch (Exception e)
            {
                var message = "Method XmlController() encountered an error with configuration file. " +
                    "Please copy proper xml file inside project folder and name it: XMLFile-Settings.xml. Error description:" + e.Message;
                throw new Exception(message);
            }

        }

        static void Refresher_Thread()
        {
            var dt1 = DateTime.Now;

            while (true)
            {
                TrackChangesFromEncryptedXml();
                TrackChangesFromUnencryptedXml();

                // Manage loopTime
                while (DateTime.Now < (dt1 + TimeSpan.FromMilliseconds(Settings.XmlRefreshrate))) // wait for some time
                {
                    System.Threading.Thread.Sleep(Settings.defaultCheckTimingInterval);

                    if (forceRefresh)  // periodically check for force refresh flag (immediately refreshes)
                    {
                        break;
                    }
                }

                forceRefresh = false;  // reset flag (notifies other methods that fresh copy was aquired)
                XmlControllerInitialized = true;
                dt1 = DateTime.Now;
                System.Threading.Thread.Sleep(100); // mandatory wait              
            }
        }

        static void TrackChangesFromEncryptedXml()
        {
            try
            {                
                XDocument newXml;

                if (!savingFilePleaseWait)
                {
                    try
                    {
                        newXml = LoadAndDecriptXml();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error loading XML file: " + ex.Message);
                    }


                    if (newXml.Element("root").Value != XmlFile.Element("root").Value)
                    {
                        // Write changes to encrypted xml - that will trigger on change event later on
                        SaveXML(XmlFile.ToString(), true);
                    }                    
                }               

            }
            catch (Exception ex)
            {
                throw new Exception("Error while refreshing data from xml file. Please check XML path and data. More info: location of error - TrackChangesFromEncryptedXml(). - Error message: " + ex.Message);
            }
        }

        static void TrackChangesFromUnencryptedXml()
        {
            try
            {
                XDocument newXml;

                if (!savingFilePleaseWait)
                {
                    try
                    {
                        newXml = LoadNotEncriptedXML();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error loading XML file: " + ex.Message);
                    }


                    if (newXml.Element("root").Value != XmlFile.Element("root").Value)
                    {
                        RefreshCache(newXml); // refresh if different
                        XmlChanged.Invoke(null, null);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while refreshing data from xml file. Please check XML path and data. More info: location of error - TrackChangesFromUnencryptedXml(). - Error message: " + ex.Message);
            }
        }

        static void RefreshCache(XDocument FreshLoadedXML)
        {
            XmlFile = FreshLoadedXML;
            SetClass();
        }


        private static void SetClass()
        {
            try
            {
                XmlGeneral = XmlFile.Element("root").Element("GENERAL");
                XmlConn = XmlFile.Element("root").Element("CONNECTION");
                XmlKadi = XmlFile.Element("root").Element("KADI");
            }

            catch (Exception)
            {
                throw;
            }
        }

        // PUBLIC

        public static IPAddress GetLogoIP(int n)
        {
            if (n < 0 || n > Settings.Devices)
            {
                throw new Exception("getLogoIP() method internal error. Index out of range");
            }

            try
            {
                var IP = XmlConn.Element("LOGO" + n).Element("serverIP").Value;

                if (!string.IsNullOrEmpty(IP) && IPAddress.TryParse(IP, out IPAddress result))
                {
                    return result;
                }

                else
                {
                    throw new Exception("IP addres in config file is not valid IP. " +
                        "Correct the IP address in XMLFile-Settings.xml file at LOGO" + n + " entry");
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static string GetLogoLocalTsap(int n)
        {
            if (n < 0 || n > Settings.Devices)
            {
                throw new Exception("getLogoLocalTsap() method internal error. Index out of range");
            }

            try
            {
                var LocalTSAP = XmlConn.Element("LOGO" + n).Element("localTSAP").Value;

                if (LocalTSAP.Length != 5 && !LocalTSAP.Contains("."))
                {
                    throw new Exception("LocalTSAP addres in config file is not valid LocalTSAP. " +
                        "Correct the LocalTSAP address in XMLFile-Settings.xml file at LOGO" + n + " entry. format must be ##.## (03.00).");
                }

                return LocalTSAP;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetLogoRemoteTsap(int n)
        {
            if (n < 0 || n > Settings.Devices)
            {
                throw new Exception("getLogoRemoteTsap() method internal error. Index out of range");
            }

            try
            {
                var RemoteTSAP = XmlConn.Element("LOGO" + n).Element("remoteTSAP").Value;

                if (RemoteTSAP.Length != 5 && !RemoteTSAP.Contains("."))
                {
                    throw new Exception("remoteTSAP addres in config file is not valid remoteTSAP. " +
                        "Correct the remoteTSAP address in XMLFile-Settings.xml file at LOGO" + n + " entry. format must be ##.## (02.00).");
                }

                return RemoteTSAP;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static PlcVars.DoubleWordAddress GetWDAddress(int device)
        {
            if (device < 0 || device > Settings.Devices)
            {
                throw new Exception("GetWDAddress() method internal error. Index out of range");
            }

            try
            {
                var xmlVal = XmlConn.Element("LOGO" + device).Element("watchdogAddress").Value;
                return new PlcVars.DoubleWordAddress(ushort.Parse(xmlVal));
            }
            catch (Exception)
            {
                throw new Exception(
                    "watchdogAddress value in config file is not valid watchdogAddress. " +
                    "Correct the watchdogAddress value in XMLFile-Settings.xml file at LOGO" + device + " entry. " +
                    "format must be 300.");
            }
        }

        public static bool GetEnabledLogo(int device)
        {

            try
            {
                if (device < 1 || device > Settings.Devices)
                {
                    throw new Exception();
                }
                else
                {
                    return Convert.ToBoolean(XmlConn.Element("LOGO" + device).Element("enabled").Value);
                }

            }
            catch (Exception)
            {

                throw new Exception(
                    "enabled value in config file is not valid enabled value. " +
                    "Correct the enabled value in XMLFile-Settings.xml file: at LOGO" + device + " entry. " +
                    "format must be true ore false.");
            }

        }

        public static int GetReadWriteCycle(int device)
        {
            try
            {
                if (device < 1 || device > Settings.Devices)
                {
                    throw new Exception();
                }
                else
                {
                    return Convert.ToInt16(XmlConn.Element("LOGO" + device).Element("ReadWriteCycle").Value);
                }
            }
            catch (Exception)
            {

                throw new Exception(
                    "ReadWriteCycle value in config file is not valid ReadWriteCycle value. " +
                    "Correct the ReadWriteCycle value in XMLFile-Settings.xml file at LOGO" + device + " entry. " +
                    "format must be number (example: 500).");
            }

        }

        public static bool IsLoginRequired()
        {
            var searchValue = "LogInRequired";

            try
            {
                return !Convert.ToBoolean(XmlGeneral.Element(searchValue).Value);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in XMLFile-Settings.xml file. " + "Exception message: " + ex.Message);
            }
        }

        public static string GetUserFromID(Int64 ID)
        {
            try
            {
                for (int i = 1; i < 31; i++)
                {
                    if (ID == Convert.ToInt64(XmlFile.Element("root").Element("USERS").Element("User" + i).Element("ID").Value))
                    {
                        return XmlFile.Element("root").Element("USERS").Element("User" + i).Element("Name").Value;
                    }
                }

                return "ERROR! No such UserID.";
            }
            catch
            {
                throw new Exception("Error Retrievinng IDs of Users From XML File.");
            }

        }

        public static List<long> GetUserIDs()
        {
            List<long> buff = new List<long>();
            int i = 0;

            try
            {
                while (true)
                {
                    i++;
                    var parsed = XmlFile.Element("root").Element("USERS").Element("User" + i).Element("ID").Value; 
                    var reading = Convert.ToInt64(parsed);
                    buff.Add(reading);

                    if (i >= 30)
                    {
                        return buff;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while loading user IDs and passwords from database. You will not be able to login: " + e.Message);
                return buff;
            }

        }

        public static string GetUserName(long index)
        {
            try
            {
                string a = XmlFile.Element("root").Element("USERS").Element("User" + index).Element("Name").Value.Replace("\"", "");
                return a;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while loading user Names from database. You will not be able to login: " + e.Message);
                return null;
            }

        }

        public static bool GetPermision(int userID, int permissionNum)
        {

            // 1  - Lahko se prijavi na startu programa
            // 2  - Lahko vstopa v meni nastavitev komunikacije
            // 3  - Lahko dostopa do nastavitev posamezne kadi (globalno)
            // 4  - Lahko poveže ali prekine povezavo s krmilniki z glavnega zaslona
            // 5  - Lahko zažene ali ustavi povezavo s sistemom glavnega zaslona
            // 6  - Lahko zažene ali ustavi posamezne kadi z glavnega zaslona
            // 7  - Lahko nastavlja prisilne zagone grelnikov
            // 8  - 
            // 9  - 
            // 10 - 

            try
            {
                string a = XmlFile.Element("root").Element("USERS").Element("User" + userID).Element("permission" + permissionNum).Value.Replace("\"", "");
                return Convert.ToBoolean(a);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while loading permissions from XML file. You will not be able acces this functionality: " + e.Message);
                return false;
            }
        }

        public static int GetPCWD_Address()
        {
            try
            {
                return Convert.ToInt32(XmlController.XmlGeneral.Element("AddressPC_WD").Value);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading AddressPC_WD from XML file. Please provide valid entry for that, for example 100. " + "Exception message: "+ ex.Message);
            }
        }

        public static bool IsLogoEnabled(int device)
        {
            try
            {
                return Convert.ToBoolean(XmlConn.Element("LOGO" + device).Element("enabled").Value);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading enabled value under LOGO" + device + " category. Please provide valid entry for that. " + "Exception message: " + ex.Message);
            }

        }

        public static string LogoServerIp(int device)
        {
            try
            {
                return XmlConn.Element("LOGO" + device).Element("serverIP").Value;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading serverIP value under LOGO" + device + " category. Please provide valid entry for that. " + "Exception message: " + ex.Message);
            }

        }

        public static string GetImeKadi(int kadIndex)
        {
            try
            {
                var buff = XmlKadi.Element("Kad" + kadIndex).Element("Ime").Value;
                return buff;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading Ime (Kadi) from xml file." + "Exception message: " + ex.Message);
            }
        }
    }
}