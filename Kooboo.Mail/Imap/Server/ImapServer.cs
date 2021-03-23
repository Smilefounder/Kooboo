//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com 
//All rights reserved.
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

using LumiSoft.Net;
using Newtonsoft.Json;

namespace Kooboo.Mail.Imap
{
    public class ImapServer : Kooboo.Tasks.IWorkerStarter
    {
        internal static Logging.ILogger _logger;
        private static long _nextConnectionId;
        static ImapServer()
        {
            _logger = Logging.LogProvider.GetLogger("imap", "socket");
        }

        private CancellationTokenSource _cancellationTokenSource;
        private TcpListener _listener;
        private Task _listenerTask;
        internal ConnectionManager _connectionManager;

        public ImapServer(int port)
        {
            Port = port;

            Timeout = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;

            Heartbeat = Heartbeat.Instance;
            _connectionManager = new ConnectionManager(Options.MaxConnections);
            Heartbeat.Add(_connectionManager);
        }

        public ImapServer(int port, SslMode mode, X509Certificate certificate)
            : this(port)
        {
            SslMode = mode;
            Certificate = certificate;
        }

        public string Name
        {
            get
            {
                return "Imap";
            }
        }

        public int Port { get; set; }

        public string HostName
        {
            get
            {
                return EmailEnvironment.FQDN;
            }
        }

        public int Timeout { get; set; }

        [JsonIgnore]
        public X509Certificate Certificate { get; private set; }

        public SslMode SslMode { get; private set; }

        public ImapServerOptions Options { get; set; } = new ImapServerOptions();

        internal Heartbeat Heartbeat { get; }

        public void Start()
        {
            if (Lib.Helper.NetworkHelper.IsPortInUse(Port))
                return;

            _listener = new TcpListener(new IPEndPoint(IPAddress.Any, Port));
            _listener.Start();

            _listenerTask = Task.Run(() => RunAcceptLoopAsync());
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
            _listener?.Stop();
        }

        private async Task RunAcceptLoopAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cid = _nextConnectionId++;
                    var tcpClient = await _listener.AcceptTcpClientAsync();

                    var session = new ImapSession(this, tcpClient, cid);
                    _ = session.Start();
                }
                catch
                {
                }
            }
        }
    }

    public class ImapServerOptions
    {
        public ImapServerOptions()
        {
#if DEBUG
            this.LiveTimeout = TimeSpan.FromHours(1);
#else
            this.LiveTimeout = TimeSpan.FromMinutes(1);
#endif
        }

        public TimeSpan LiveTimeout { get; set; }

        public int? MaxConnections { get; set; }
    }
}
