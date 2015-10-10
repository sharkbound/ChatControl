using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fr34kyn01535.ChatControl
{
    public class ChatControlConfiguration : IRocketPluginConfiguration
    {
        public bool AnnounceMute;
        public bool AnnounceUnmute;

        public List<String> Badwords;

        public short WarningsBeforeMute;
        public short WarningsBeforeKick;
        public short WarningsBeforeBan;

        public uint BanDuration;

        public string MessageColor;

        public void LoadDefaults()
        {
            AnnounceMute = true;
            AnnounceUnmute = true;

            Badwords = new List<string> { "suck", "fuck", "ass", "blame sven" };

            WarningsBeforeMute = 3;
            WarningsBeforeKick = 5;
            WarningsBeforeBan = -1;

            BanDuration = 3600;

            MessageColor = "Yellow";
        }
    }
}
