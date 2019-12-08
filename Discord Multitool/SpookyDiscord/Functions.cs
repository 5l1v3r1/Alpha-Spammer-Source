using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Threading;
using Leaf.xNet;
using Discord.WebSocket;
using Discord.Net;
using Discord;

namespace Discord_Multitool.SpookyDiscord
{
    class Functions
    {
        public static bool Join(string invite, string token)
        {
            CookieContainer cookieContainer = new CookieContainer();

            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://discord.gg/" + invite);
                request.Headers["authorization"] = token;
                request.CookieContainer = cookieContainer;
                var response = (HttpWebResponse)request.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


                var request3 = (HttpWebRequest)WebRequest.Create("https://discordapp.com/api/v6/invites/" + invite);
                request3.Headers["authorization"] = token;
                request3.Method = "POST";
                request3.ContentType = "application/json";
                request3.CookieContainer = cookieContainer;
                request3.ContentLength = 0;

                var response3 = (HttpWebResponse)request3.GetResponse();
                var responseString3 = new StreamReader(response3.GetResponseStream()).ReadToEnd();
                Console.WriteLine(responseString3);
                return true;
            }
            catch (WebException ex)
            {
                string response = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                Console.WriteLine(response);
                return false;
            }

        }
        public static bool Add_Friend(string id, string token)
        {
            CookieContainer cookieContainer = new CookieContainer();

            try
            {
                var request3 = (HttpWebRequest)WebRequest.Create("https://discordapp.com/api/v6/users/@me/relationships/" + id);

                var postData = "{}";
                var data = Encoding.ASCII.GetBytes(postData);
                request3.Headers["authorization"] = token;
                request3.Method = "PUT";
                request3.ContentType = "application/json";
                request3.ContentLength = data.Length;
                request3.CookieContainer = cookieContainer;
                using (var stream = request3.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response3 = (HttpWebResponse)request3.GetResponse();
                var responseString3 = new StreamReader(response3.GetResponseStream()).ReadToEnd();
                Console.WriteLine(responseString3);
                //Console.WriteLine("Bot joined");
                return true;
            }
            catch (WebException ex)
            {
                string response = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                Console.WriteLine(response);
                return false;
            }
        }
        public static string sendMessaage(string id, string content, string token)
        {

            CookieContainer cookieContainer = new CookieContainer();

            try
            {

                string payload = "{\"content\":\"" + content + "\",\"tts\":false}";

                var request2 = (HttpWebRequest)WebRequest.Create("https://discordapp.com/api/v6/channels/" + id + "/messages");
                request2.Accept = "application/json, text/javascript, */*; q=0.01";

                var postData = payload;
                var data = Encoding.ASCII.GetBytes(postData);
                request2.Headers["authorization"] = token;
                request2.Method = "POST";
                request2.ContentType = "application/json";
                request2.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.75 Safari/537.36";
                request2.ContentLength = data.Length;
                using (var stream = request2.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = request2.GetResponse();
                string responsestring = new StreamReader(response.GetResponseStream()).ReadToEnd();

                
                //Console.WriteLine(responsestring);
                return responsestring;
            }
            catch (WebException e)
            {
                var errresp = e.Response;
                string respstring = new StreamReader(errresp.GetResponseStream()).ReadToEnd();
                //Console.WriteLine(respstring);
                return respstring;
            }
            catch
            {
                return "";
            }


        }
        public static bool AddReaction(string C_ID, string M_ID, string emoji, string token)
        {
            try
            {
                emoji = Uri.EscapeUriString(emoji);
                var request3 = (HttpWebRequest)WebRequest.Create("https://discordapp.com/api/v6/channels/" + C_ID + "/messages/" + M_ID + "/reactions/" + emoji + "/%40me");
                 
                request3.Headers["authorization"] = token;
                request3.Method = "PUT";
                request3.ContentType = "application/json";
                request3.ContentLength = 0;
                var response3 = (HttpWebResponse)request3.GetResponse();
                var responseString3 = new StreamReader(response3.GetResponseStream()).ReadToEnd();
                Console.WriteLine(responseString3);
                //Console.WriteLine("Bot joined");
                return true;
            }
            catch (WebException ex)
            {
                string response = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                Console.WriteLine(response);
                return false;
            }
        }
        public static bool RemoveReaction(string C_ID, string M_ID, string emoji, string token)
        {
            try
            {
                emoji = Uri.EscapeUriString(emoji);
                var request3 = (HttpWebRequest)WebRequest.Create("https://discordapp.com/api/v6/channels/" + C_ID + "/messages/" + M_ID + "/reactions/" + emoji + "/%40me");

                request3.Headers["authorization"] = token;
                request3.Method = "DELETE";
                request3.ContentType = "application/json";
                request3.ContentLength = 0;
                var response3 = (HttpWebResponse)request3.GetResponse();
                var responseString3 = new StreamReader(response3.GetResponseStream()).ReadToEnd();
                Console.WriteLine(responseString3);
                //Console.WriteLine("Bot joined");
                return true;
            }
            catch (WebException ex)
            {
                string response = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                Console.WriteLine(response);
                return false;
            }
        }
        public static string GetDM(string U_id, string token)
        {
            string botid = "";
            string Channel_ID = "";
            string html;
            try //get bot id
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://discordapp.com/api/v7/users/@me");
                request.Headers["authorization"] = token;
                request.AutomaticDecompression = DecompressionMethods.GZip;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }

                botid = html.Split(new[] { "\"id\": \"" }, StringSplitOptions.None)[1].Split('"')[0];
            }
            catch (WebException e)
            {
                var errresp = e.Response;
                string respstring = new StreamReader(errresp.GetResponseStream()).ReadToEnd();
                //Console.WriteLine(respstring);

                return respstring;
            }


            try
            {

                string payload = "{\"recipients\":[\"" + U_id + "\"]}";

                var request2 = (HttpWebRequest)WebRequest.Create("https://discordapp.com/api/v6/users/" + botid + "/channels");
                request2.Accept = "application/json, text/javascript, */*; q=0.01";

                var postData = payload;
                var data = Encoding.ASCII.GetBytes(postData);
                request2.Headers["authorization"] = token;
                request2.Method = "POST";
                request2.ContentType = "application/json";
                request2.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.75 Safari/537.36";
                request2.ContentLength = data.Length;
                using (var stream = request2.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = request2.GetResponse();
                string responsestring = new StreamReader(response.GetResponseStream()).ReadToEnd();

                Channel_ID = responsestring.Split(new[] { "\"id\": \"" }, StringSplitOptions.None)[1].Split('"')[0];


            }
            catch (WebException e)
            {
                var errresp = e.Response;
                string respstring = new StreamReader(errresp.GetResponseStream()).ReadToEnd();
                //Console.WriteLine(respstring);

                return respstring;
            }

            return Channel_ID;

        }
        public static bool QuitGuild(string id, string token)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://discordapp.com/api/v6/users/@me/guilds/" + id);
                request.Method = "DELETE";
                request.Headers["authorization"] = token;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (((int)response.StatusCode == 204) || ((int)response.StatusCode == 200))
                {
                    return true;
                }
                return false;

            }
            catch(WebException e)
            {
                var resp = e.Response;
                string respstring = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                Console.WriteLine(respstring);

                return false;

            }


        }
        public static async Task<DiscordSocketClient> Revive_Bot(string token)
        {
            try
            {
                DiscordSocketClient _client;
                _client = new DiscordSocketClient();
                await _client.LoginAsync(TokenType.User, token);
                await _client.StartAsync();
                await _client.SetGameAsync("Alpha spammer <3", null, StreamType.Twitch);
                await _client.SetStatusAsync(UserStatus.DoNotDisturb);

                return _client;
                

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            

        }
        public static string GetAllUsers(string token, string C_ID)
        {
            string text = "";
            try//
            {


                var request = (HttpWebRequest)WebRequest.Create("https://discordapp.com/api/v6/channels/" +C_ID +  "/messages?limit=50");
                request.Headers["authorization"] = token;
                var response = (HttpWebResponse)request.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return text;


        }
    }
}
