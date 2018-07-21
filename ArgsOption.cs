using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerifyCookie
{
    class ArgsOption
    {
        [Option("hub", Required = true, HelpText = "Specify hub url, i.e. http://signalrlinux3.southeastasia.cloudapp.azure.com:5050/signalrbench")]
        public string HubUrl { get; set; }

        [Option("transport", Required = false, Default = "Websockets", HelpText = "Specify the transport type: Websockets|LongPolling|ServerSentEvents")]
        public string TransportType { get; set; }

        [Option("useHandler", Required = false, Default = "true", HelpText = "Enable HttpHanlder which triggers cookie missing issue")]
        public string UseHandler { get; set; }
    }
}
