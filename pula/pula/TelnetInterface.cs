// minimalistic telnet implementation
// conceived by Tom Janssens on 2007/06/06  for codeproject
//
// http://www.corebvba.be



using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace TelnetInterface {
    enum Verbs {
        WILL = 251,
        WONT = 252,
        DO = 253,
        DONT = 254,
        IAC = 255,
        ERR = 530
    }

    enum Options {
        SGA = 3
    }

    class TelnetConnection {
        TcpClient tcpSocket;

        int TimeOutMs = 50;

        public TelnetConnection() {
            tcpSocket = new TcpClient();
        }

        public void Connect(string Hostname, int Port) {
            tcpSocket.Connect(Hostname, Port);
            System.Threading.Thread.Sleep(1000);
        }

        public void Close() {
            tcpSocket.Close();
        }

        public void WriteLine(string cmd) {
            Write(cmd + "\n");
        }

        public void Write(string cmd) {
            if (!tcpSocket.Connected) return;
            byte[] buf = System.Text.ASCIIEncoding.ASCII.GetBytes(cmd.Replace("\0xFF", "\0xFF\0xFF"));
            tcpSocket.GetStream().Write(buf, 0, buf.Length);
        }

        const double TimeOutRead = 3000.0; //timeout in miliseconds when to stop waiting for data
        public string Read() {
            if (!tcpSocket.Connected) throw new Exception("disconnected in read method");
            StringBuilder sb = new StringBuilder();
            //oczekiwanie na dane
            using (System.Timers.Timer t = new System.Timers.Timer(TimeOutRead)) {
                bool to = false; //time out
                t.Elapsed += delegate { to = true; };
                t.Start();
                while ((tcpSocket.Available == 0) & (to == false)) ;
            }
            do {
                ParseTelnet(sb);
                System.Threading.Thread.Sleep(TimeOutMs);
            } while (tcpSocket.Available > 0);
            String s = sb.ToString();
            if (s.StartsWith("530")) {
                WriteLine("quit");
                Close();
                throw (new Exception(String.Format("Reconnect because of: {0}", s)));
            }
            return s;
        }

        public bool IsConnected {
            get { return tcpSocket.Connected; }
        }

        void ParseTelnet(StringBuilder sb) {
            while (tcpSocket.Available > 0) {
                int input = tcpSocket.GetStream().ReadByte();
                switch (input) {
                    case -1:
                        break;
                    case (int)Verbs.IAC:
                        // interpret as command
                        int inputverb = tcpSocket.GetStream().ReadByte();
                        if (inputverb == -1) break;
                        switch (inputverb) {
                            case (int)Verbs.IAC:
                                //literal IAC = 255 escaped, so append char 255 to string
                                sb.Append(inputverb);
                                break;
                            case (int)Verbs.DO:
                            case (int)Verbs.DONT:
                            case (int)Verbs.WILL:
                            case (int)Verbs.WONT:
                                // reply to all commands with "WONT", unless it is SGA (suppres go ahead)
                                int inputoption = tcpSocket.GetStream().ReadByte();
                                if (inputoption == -1) break;
                                tcpSocket.GetStream().WriteByte((byte)Verbs.IAC);
                                if (inputoption == (int)Options.SGA)
                                    tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WILL : (byte)Verbs.DO);
                                else
                                    tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WONT : (byte)Verbs.DONT);
                                tcpSocket.GetStream().WriteByte((byte)inputoption);
                                break;
                            case (int)Verbs.ERR:
                                throw (new Exception());
                            default:
                                break;
                        }
                        break;
                    default:
                        sb.Append((char)input);
                        break;
                }
            }
        }
    }
}
