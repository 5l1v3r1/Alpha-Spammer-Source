using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Leaf.xNet;
using System.Security.Authentication;
using System.Net;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using Discord.WebSocket;
using Discord.Net;
using Discord;

namespace Discord_Multitool
{
    public partial class Form1 : Form
    {
        public static List<string> Tokens = new List<string>();
        public static List<DiscordSocketClient> Clients = new List<DiscordSocketClient>();

        public static Thread Workerthread = null;
        public static Thread ProxyCheckerThread;

        public static bool AreTokensAlive = false;
        

        public static List<Thread> Working_Threads = new List<Thread>();
        public static List<Thread> Alive_Bots = new List<Thread>();

        public static Random rnd = new Random();
        public struct Config
        {
            public static string Last_Dir = null;
        }

        public struct Attacks_Stats
        {
            public static int Joined = 0;
            public static int Join_Errors = 0;
            public static int Join_Waiting = 0;

            public static int Left = 0;
            public static int Leave_Errors = 0;
            public static int Leave_Waiting = 0;

            public static bool Spam = true;
            public static bool CFMad = false;
            public static int Sent = 0;
            public static int Sent_Errors = 0;

            public static int AddFriend = 0;
            public static int Friend_Error = 0;

            public static int Reacted = 0;
            public static int ReactErrors = 0;

        }
        public struct _Proxies
        {
            public static List<string> Https = new List<string>();
            public static List<string> Socks4 = new List<string>();
            public static List<string> Socks5 = new List<string>();

            public static List<string> C_Https = new List<string>();
            public static List<string> C_Socks4 = new List<string>();
            public static List<string> C_Socks5 = new List<string>();

            public static int proxytype = 0;

            public static int Bad_Proxies = 0;

            public static int Good_Https = 0;
            public static int Good_Socks4 = 0;
            public static int Good_Socks5 = 0;
        }


        public Form1()
        {
            InitializeComponent();


            if (Properties.Settings.Default.Code == null)
            {
                Properties.Settings.Default.Code = "code";
            }
            try
            {


                string code = File.ReadAllText(@"./key.txt");
                string D_Code = Encryption.Decrypt(code);

                Authorise(D_Code);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }


            label30.Text = "Authorised: " + Properties.Settings.Default.Authorised;
            textBox1.Text = Properties.Settings.Default.Code;


            ServicePointManager.DefaultConnectionLimit = 99999;
            foreach (string line in File.ReadLines(@"tokens.txt", Encoding.UTF8))
            {
                Tokens.Add(line);
            }




            metroButton2.Text = "Tokens: " + Tokens.Count();

            metroTabControl1.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.ShowDialog();

            try
            {
                foreach (string line in File.ReadLines(ofd.FileName, Encoding.UTF8))
                {
                    Tokens.Add(line);
                }

                metroButton2.Text = "Tokens: " + Tokens.Count();
            }
            catch
            {

            }
            




        }

        private void StartJoinFlood_Click(object sender, EventArgs e)
        {
            Workerthread = null;

            Workerthread = new Thread(delegate ()
            {
                Attacks_Stats.Joined = 0;
                Attacks_Stats.Join_Errors = 0;
                Attacks_Stats.Join_Waiting = 0;


                Attacks_Stats.Join_Waiting = Tokens.Count();
                foreach(string token in Tokens)
                {
                    //Thread.Sleep(5000 + rnd.Next(0, 2000));
                    new Thread(delegate ()
                    {
                        
                        if (SpookyDiscord.Functions.Join(metroTextBox1.Text, token))
                            Attacks_Stats.Joined++;
                        else
                            Attacks_Stats.Join_Errors++;


                        Attacks_Stats.Join_Waiting--;

                        label3.Invoke(new MethodInvoker(delegate () { label3.Text = "Joined: " + Attacks_Stats.Joined; label4.Text = "Errors: " + Attacks_Stats.Join_Errors; label5.Text = "Waiting: " + Attacks_Stats.Join_Waiting; }));
                    }).Start();
                    Thread.Sleep(50);
                }
            });

            if (!Properties.Settings.Default.Authorised)
            {
                MessageBox.Show("Program has not been authorised, add Sheepy for a auth code :P", "Alert");
            }
            else
            {
                Workerthread.Start();
            }
            
            
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Workerthread.Abort();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            

            try
            {
                Workerthread = null;
                Workerthread = new Thread(delegate ()
                {
                    Attacks_Stats.Left = 0;
                    Attacks_Stats.Leave_Errors = 0;
                    Attacks_Stats.Leave_Waiting = 0;

                    Attacks_Stats.Leave_Waiting = Tokens.Count();

                    foreach (string token in Tokens)
                    {
                        new Thread(delegate ()
                        {
                            if (SpookyDiscord.Functions.QuitGuild(metroTextBox2.Text, token))
                                Attacks_Stats.Left++;
                            else
                                Attacks_Stats.Leave_Errors++;


                            Attacks_Stats.Leave_Waiting--;
                            label6.Invoke(new MethodInvoker(delegate () { label6.Text = "Left: " + Attacks_Stats.Left; label7.Text = "Errors: " + Attacks_Stats.Leave_Errors; label8.Text = "Waiting: " + Attacks_Stats.Leave_Waiting; }));

                        }).Start();

                    }
                });
            }
            catch(Exception b)
            {

                Console.WriteLine(b.Message);
            }
            
            if (!Properties.Settings.Default.Authorised)
            {
                MessageBox.Show("Program has not been authorised, add Sheepy for a auth code :P", "Alert");
            }
            else
            {
                Workerthread.Start();
            }
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            try
            {
                Workerthread = null;
                Attacks_Stats.Spam = true;

                Attacks_Stats.CFMad = false;
                Log("[+] Initializing spammer :)");
                Workerthread = new Thread(delegate ()
                {

                    Attacks_Stats.Sent = 0;
                    Attacks_Stats.Sent_Errors = 0;
                    foreach (string token in Tokens)
                    {
                        Thread t = new Thread(delegate ()
                        {
                            string current_token = token;
                            while (true)
                            {
                                if (!Attacks_Stats.Spam)
                                    break;

                                //if (SpookyDiscord.Functions.sendMessaageX(metroTextBox3.Text, metroTextBox4.Text, current_token, pc))
                                //    Attacks_Stats.Sent++;

                                string responseS = SpookyDiscord.Functions.sendMessaage(metroTextBox3.Text, metroTextBox4.Text, current_token);


                                Console.WriteLine(responseS);

                                if (responseS.Contains("The owner of this website (discordapp.com) has banned you temporarily from accessing this website."))
                                {

                                    Attacks_Stats.Spam = false;
                                    Attacks_Stats.CFMad = true;
                                    Thread.Sleep(100);
                                    stopMessageSpam();

                                }

                                if ((responseS.Contains("Unauthorized")) || (responseS.Contains("You need to verify your account in order to perform this action")) || (responseS.Contains("Missing Access")))
                                {
                                    Attacks_Stats.Sent_Errors++;

                                    Log("[!] Could not join/send message leaving...");

                                    break;

                                }

                                if (responseS.Contains("attachments"))
                                {
                                    Log("[+] Successfully sent " + metroTextBox4.Text + " to " + metroTextBox3.Text);
                                    Attacks_Stats.Sent++;

                                }

                                if (responseS.Contains("\"retry_after\": "))
                                {
                                    int sleepfor = Convert.ToInt32(responseS.Split(new[] { "\"retry_after\": " }, StringSplitOptions.None)[1].Split('}')[0]);
                                    Log("[!] Rate limited, waiting " + sleepfor + " ms");

                                    Thread.Sleep(sleepfor);

                                }


                                label13.Invoke(new MethodInvoker(delegate () { label13.Text = "Sent: " + Attacks_Stats.Sent; label12.Text = "Errors: " + Attacks_Stats.Sent_Errors; }));
                                Thread.Sleep(150);
                            }

                        });
                        t.Start();
                        Working_Threads.Add(t);
                        Thread.Sleep(15);

                    }


                    while (true)
                    {
                        int lastnumofmessages = Attacks_Stats.Sent;


                        Thread.Sleep(1000);

                        label27.Invoke(new MethodInvoker(delegate () { label27.Text = "MPS: " + (Attacks_Stats.Sent - lastnumofmessages); }));
                    }
                });
            }
            catch (Exception b)
            {

                Console.WriteLine(b.Message);
            }
            

            if (!Properties.Settings.Default.Authorised)
            {
                MessageBox.Show("Program has not been authorised, add Sheepy for a auth code :P", "Alert");
            }
            else
            {
                Workerthread.Start();
            }
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            stopMessageSpam();
        }
        private void stopMessageSpam()
        {
            try
            {
                Attacks_Stats.Spam = false;
                Workerthread.Abort();

                //for (int i =0; i != Working_Threads.Count() -1; i++)
                //{
                //    try
                //    {
                //        Thread TH = Working_Threads.ElementAt(i);
                //    }
                //    catch { }
                //}

                foreach (Thread TH in Working_Threads)
                {
                    try
                    {
                        TH.Abort();

                    }
                    catch { }

                }
                Working_Threads.Clear();
                if (Attacks_Stats.CFMad)
                {
                    MessageBox.Show("Cloudflare limited :(", "Alert");

                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
        
        private void metroButton8_Click(object sender, EventArgs e)
        {
            Workerthread = null;
            Workerthread = new Thread(delegate ()
            {
                Attacks_Stats.AddFriend = 0;
                Attacks_Stats.Friend_Error = 0;

                Attacks_Stats.Leave_Waiting = Tokens.Count();

                foreach (string token in Tokens)
                {
                    new Thread(delegate ()
                    {
                        if (SpookyDiscord.Functions.Add_Friend(metroTextBox5.Text, token))
                            Attacks_Stats.AddFriend++;
                        else
                            Attacks_Stats.Friend_Error++;

                        label16.Invoke(new MethodInvoker(delegate () { label17.Text = "Added: " + Attacks_Stats.AddFriend; label16.Text = "Errors: " + Attacks_Stats.Friend_Error; }));
                    }).Start();

                }
            });
            if (!Properties.Settings.Default.Authorised)
            {
                MessageBox.Show("Program has not been authorised, add Sheepy for a auth code :P", "Alert");
            }
            else
            {
                Workerthread.Start();
            }
        }

        private void metroButton7_Click(object sender, EventArgs e)
        {
            Workerthread.Abort();
        }

        private void Proxies_Click(object sender, EventArgs e)
        {

        }


        private void Log(string text)
        {
            richTextBox1.Invoke(new MethodInvoker(delegate () { richTextBox1.AppendText(text + "\r\n"); richTextBox1.ScrollToCaret();}));
        }

        private void metroButton10_Click(object sender, EventArgs e)
        {
            Workerthread = null;
            Workerthread = new Thread(delegate ()
            {
                Attacks_Stats.Reacted = 0;
                Attacks_Stats.ReactErrors = 0;


                foreach (string token in Tokens)
                {
                    new Thread(delegate ()
                    {
                        if (SpookyDiscord.Functions.AddReaction(metroTextBox6.Text, metroTextBox7.Text, metroTextBox8.Text, token))
                            Attacks_Stats.Reacted++;
                        else
                            Attacks_Stats.ReactErrors++;

                        label16.Invoke(new MethodInvoker(delegate () { label24.Text = "Reacted: " + Attacks_Stats.Reacted; label16.Text = "Errors: " + Attacks_Stats.Friend_Error; }));
                    }).Start();

                }
            });
            if (!Properties.Settings.Default.Authorised)
            {
                MessageBox.Show("Program has not been authorised, add Sheepy for a auth code :P", "Alert");
            }
            else
            {
                Workerthread.Start();
            }
        }

        private void metroButton9_Click(object sender, EventArgs e)
        {
            Workerthread = null;
            Workerthread = new Thread(delegate ()
            {
                Attacks_Stats.Reacted = 0;
                Attacks_Stats.ReactErrors = 0;


                foreach (string token in Tokens)
                {
                    new Thread(delegate ()
                    {
                        if (SpookyDiscord.Functions.RemoveReaction(metroTextBox6.Text, metroTextBox7.Text, metroTextBox8.Text, token))
                            Attacks_Stats.Reacted++;
                        else
                            Attacks_Stats.ReactErrors++;

                        label16.Invoke(new MethodInvoker(delegate () { label24.Text = "Removed: " + Attacks_Stats.Reacted; label16.Text = "Errors: " + Attacks_Stats.Friend_Error; }));
                    }).Start();

                }
            });
            if (!Properties.Settings.Default.Authorised)
            {
                MessageBox.Show("Program has not been authorised, add Sheepy for a auth code :P", "Alert");
            }
            else
            {
                Workerthread.Start();
            }
        }

        private void Floods_Click(object sender, EventArgs e)
        {

        }

        

        private void metroTextBox10_Click(object sender, EventArgs e)
        {

        }
        private void Authorise(string code)
        {
            try
            {
                string[] parts = code.Split('-');

                ulong total = 0;

                foreach (string part in parts)
                {
                    Console.WriteLine(part);
                    byte[] ba = Encoding.Default.GetBytes(part);
                    var hexString = BitConverter.ToString(ba);
                    hexString = hexString.Replace("-", "");
                    ulong value = ulong.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
                    total = total + value;

                }
                Console.WriteLine("total " + total);
                if (total == 2349400061)
                {
                    Properties.Settings.Default.Authorised = true;
                    label30.Text = "Authorised: " + Properties.Settings.Default.Authorised;

                    string E_Code = Encryption.Crypt(code);

                    File.WriteAllText(@"./key.txt", E_Code);

                }
                else
                {
                    Properties.Settings.Default.Authorised = false;
                    label30.Text = "Authorised: " + Properties.Settings.Default.Authorised;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string code = textBox1.Text;

            Authorise(code);
        }

        private void metroButton12_Click(object sender, EventArgs e)
        {
            foreach(string token in Tokens)
            {
                Thread t = new Thread(async delegate ()
                {
                    DiscordSocketClient client = await SpookyDiscord.Functions.Revive_Bot(token);
                    if (!Clients.Contains(client))
                    {
                        Clients.Add(client);
                    }
                    Thread.Sleep(-1);
                });
                t.Start();
                Alive_Bots.Add(t);

                
            }
            AreTokensAlive = true;
            
        }

        private void metroButton11_Click(object sender, EventArgs e)
        {
            foreach(Thread t in Alive_Bots)
            {
                t.Abort();
            }
        }

        

        private void metroButton13_Click(object sender, EventArgs e)
        {
            stopMessageSpam();
        }

        private void label32_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBox9_Click(object sender, EventArgs e)
        {

        }
    }
}
