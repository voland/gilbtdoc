using System;
using System.Threading;
using TelnetInterface;
using System.Net.Sockets;
using System.IO;
using System.Drawing;

namespace Pula {
    class MainClass {
        private static int _pula1 = 0;
        private static int _pula2 = 0;
        private static object locker = new object();

        private static int pula1 {
            set {
                lock (locker) {
                    _pula1 = value;
                }
            }
            get {
                lock (locker) {
                    return _pula1;
                }
            }
        }

        private static int pula2 {
            set {
                lock (locker) {
                    _pula2 = value;
                }
            }
            get {
                lock (locker) {
                    return _pula2;
                }
            }
        }

        private static bool exit {
            set {
                lock (locker) {
                    _exit = value;
                }
            }
            get {
                lock (locker) {
                    return _exit;
                }
            }
        }

        private static bool _exit = false;

        private static string IP = "";

        private const int ParNumber = 1;

        private static string PassiveIP = "";
        private static int PassivePort = 0;

        private static void ExtractAddr(String input) {
            string[] stab = input.Split(new char[] { '(', ',', ')', '\x0' });
            if (stab.Length > 7) {
                PassiveIP = String.Format("{0}.{1}.{2}.{3}", stab[1], stab[2], stab[3], stab[4]);
                int a = int.Parse(stab[5]);
                int b = int.Parse(stab[6]);
                PassivePort = 0x100 * a + b;
                Console.WriteLine(string.Format("Adres {0}:{1}", PassiveIP, PassivePort));
            } else {
                throw new Exception(string.Format("Cos nie tak z wejsciem w tryb pasywny {0}:{1}", PassiveIP, PassivePort));
            }
        }

        private static void PulaThread() {
            while (exit == false) {
                System.Threading.Thread.Sleep(100);
                if (pula2 == 0) {
                    pula2 = 1000;
                    pula1 = 0;
                }
                pula1++;
                pula2--;
            }
        }

        static void SendStreamPasiveMode(Stream data) {
            using (TcpClient dataClient = new TcpClient()) {
                try {
                    dataClient.Connect(PassiveIP, PassivePort);
                    string response = tc.Read();
                    Console.Write(response);
                    if (response.StartsWith("150 Connection accepted.")) {
                        Stream sendStream = dataClient.GetStream();
                        data.Seek(0, SeekOrigin.Begin);
                        int tempbyte;
                        while ((tempbyte = data.ReadByte()) > -1) {
                            sendStream.WriteByte((byte)tempbyte);
                        }
                    }
                } catch (Exception e) {
                    Console.WriteLine("Connection on data socket lost: {0}", e.Message);
                } finally {
                    Console.WriteLine("Closing data socket");
                    dataClient.Close();
                }
            }
        }

        static void Login() {
            Console.Write("Connecting... ");
            tc = new TelnetConnection();
            tc.Connect(IP, 21);
            String response = tc.Read();
            Console.Write(response);
            if (response.StartsWith("220")) {
                tc.WriteLine("user anonymouse");
                response = tc.Read();
                Console.Write(response);
                if (response.StartsWith("331")) {
                    tc.WriteLine("pass none");
                    response = tc.Read();
                    Console.Write(response);
                    if (response.StartsWith("230")) {
                    } else {
                        throw new Exception("Could not connect");
                    }
                } else {
                    throw new Exception("Could not connect");
                }
            } else {
                throw new Exception("Could not connect");
            }
        }

        static void Logout() {
            Console.WriteLine("Logout");
            //tc.WriteLine("quit");
            tc.Close();
            //Console.Write(tc.Read());
        }

        static void EnterPasiveMode() {
            tc.WriteLine("pasv"); //enter pasive mode
            Sleep();
            string temp = tc.Read();
            Console.Write(temp);
            ExtractAddr(temp);
            Sleep();
            Sleep();
        }

        static void ExecCommand(string cmd) {
            tc.WriteLine(cmd);
            Console.Write(tc.Read());
            System.Threading.Thread.Sleep(100);
        }

        static void SendStrings(string s1, string s2, string s3, string s4) {
            tc.WriteLine(String.Format("money \"{0}\" \"{1}\" \"{2}\" \"{3}\"", s1, s2, s3, s4));
            Console.Write(tc.Read());
            System.Threading.Thread.Sleep(100);
        }

        static void SendGraphic(cLim graphic) {
            using (Stream s = new MemoryStream()) {
                graphic.Compile(s);
                tc.WriteLine("STOR image.lim");
                SendStreamPasiveMode(s);
                Console.Write(tc.Read());
            }
            System.Threading.Thread.Sleep(20000);
        }

        static void Sleep() {
            System.Threading.Thread.Sleep(500);
        }

        static private TelnetConnection tc;
        private static void ConnectionThread() {
            do {
                try {
                    Login();
                    EnterPasiveMode();
                    //ExecCommand( "netconf \"192.168.1.110\" \"255.255.255.0\" \"192.168.1.1\"" );
                    ExecCommand("font abold32.fnt"); // czcionka z pliku na karcie SD
                    //ExecCommand("font fontnormal");  //czcionka 8 systemowa
                    //ExecCommand("font fontfat");  //czcionka gróba 8 systemowa
                    ExecCommand("txtpos \"c\" \"l\" \"2\" \"p\"");
                    ExecCommand("contrast 2");
                    int i = 32;
                    int j = 0;
                    while (exit == false) {
                        //using (Bitmap b = new Bitmap(128, 48)) {
                        //    using (Graphics g = Graphics.FromImage(b)) {
                        //        g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, 128, 48));
                        //        g.DrawString(
                        //            String.Format("graf{0}", j++),

                        //            new Font(new FontFamily("Arial"), 24f, FontStyle.Bold),
                        //            new SolidBrush(Color.White),
                        //            new Point(0, 0));
                        //        j %= 800;
                        //    }
                        //    SendGraphic(new cLim(b));
                        //}
                        string hexi = i.ToString("X2");
                        i++;
                        if (i == 256) i = 32;
                        String s1 = String.Format("P {1}", hexi, pula1);
                        String s2 = String.Format("{0}", pula1 % 100);
                        SendStrings(s1, "123", "s2", "Linia4");
                        //SendStrings(s1, "", "", "");
                        j++;
                    }
                } catch (Exception e) {
                    Console.WriteLine("Conn break. message: {0}", e.Message);
                } finally {
                    Logout();
                }
            } while (exit == false);
        }

        public static void Main(string[] args) {
            IP = "192.168.1.206";

            if (args.Length == ParNumber) {
                IP = args[ParNumber - 1];
            } else {
                Console.WriteLine("Enter ip address as parameter, example: Pula.exe \"10.0.0.4\"");
            }

            Console.WriteLine("Trying connect with {0}", IP);
            //wlaczamy odliczanie w osobnym watku
            Thread pulaT = new Thread(new ThreadStart(PulaThread));
            pulaT.Start();
            //wlaczamy polaczenie w osobnym watku
            Thread connT = new Thread(new ThreadStart(ConnectionThread));
            connT.Start();
            //wcisnij enter aby zakonczyc program
            Console.ReadKey();
            exit = true;
            while (pulaT.IsAlive & connT.IsAlive) { };
            Console.WriteLine("Pula1 = {0}, Pula2 = {1}", pula1, pula2);
        }
    }
}
