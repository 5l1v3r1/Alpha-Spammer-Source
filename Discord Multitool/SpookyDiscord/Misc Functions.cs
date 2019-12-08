using System;
using System.Text;
using System.IO;
using static Discord_Multitool.Form1;

namespace Discord_Multitool.SpookyDiscord
{
    class Misc_Functions
    {
        public static void Load_Config()
        {
            try
            {
                string text = File.ReadAllText("Config.ini");


                Config.Last_Dir = text.Split(new[] { "Last token directory=" }, StringSplitOptions.None)[1].Split('\n')[0];

                //int Threads = Convert.ToInt32(text.Split(new[] { "Threads (max 400)=" }, StringSplitOptions.None)[1].Split('\n')[0]);
                //if (Threads > 500)
                //    Config.Threads = 500;
                //else
                //    Config.Threads = Threads;

                //Config.ExportPhoneLocked = Convert.ToBoolean(text.Split(new[] { "Export Phone Locked tokens =" }, StringSplitOptions.None)[1].Split('\n')[0]);
            }
            catch
            {
            }
        }

        public static bool LoadHttps()
        {
            try
            {
                foreach (string line in File.ReadLines(@"./Proxies/https proxies.txt", Encoding.UTF8))
                {
                    _Proxies.Https.Add(line);
                }
                return true;
                
            }
            catch(Exception e)
            {
                Console.WriteLine("Could not load proxies : " + e.Message);
                return false;

            }
        }
        public static bool LoadSocks4()
        {
            try
            {
                foreach (string line in File.ReadLines(@"./Proxies/socks4 proxies.txt", Encoding.UTF8))
                {
                    _Proxies.Socks4.Add(line);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not load proxies : " + e.Message);
            }
            return false;

        }
        public static bool LoadSocks5()
        {
            try
            {
                foreach (string line in File.ReadLines(@"./Proxies/socks5 proxies.txt", Encoding.UTF8))
                {
                    _Proxies.Socks5.Add(line);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not load proxies : " + e.Message);
            }
            return false;

        }

        public static bool LoadCHttps()
        {
            try
            {
                foreach (string line in File.ReadLines(@"./Proxies/Checked_Https.txt", Encoding.UTF8))
                {
                    _Proxies.Https.Add(line);
                }
                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine("Could not load proxies : " + e.Message);
                return false;

            }
        }
        public static bool LoadCSocks4()
        {
            try
            {
                foreach (string line in File.ReadLines(@"./Proxies/Checked_Socks4.txt", Encoding.UTF8))
                {
                    _Proxies.Socks4.Add(line);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not load proxies : " + e.Message);
            }
            return false;

        }
        public static bool LoadCSocks5()
        {
            try
            {
                foreach (string line in File.ReadLines(@"./Proxies/Checked_Socks5.txt", Encoding.UTF8))
                {
                    _Proxies.Socks5.Add(line);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not load proxies : " + e.Message);
            }
            return false;

        }


    }
}
