using Rocket.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.API.Collections;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.API;

namespace fr34kyn01535.ChatControl
{
    public class ChatControl : RocketPlugin<ChatControlConfiguration>
    {
        public static ChatControl Instance;
        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList() {
                    { "command_player_not_found", "Player not found"},
                    { "command_mute", "{0} is now muted"},
                    { "command_unmute", "{0} is now unmuted"},
                    { "kick_ban_reason", "Too many badwords"},
                    { "badword_detected", "{0} is a badword, don't use it or bad things will happen to you. This is your {1}. warning."}
                };
            }
        }

        protected override void Load()
        {
            Instance = this;
            UnturnedPlayerEvents.OnPlayerChatted += UnturnedPlayerEvents_OnPlayerChatted;
        }

        private void UnturnedPlayerEvents_OnPlayerChatted(Rocket.Unturned.Player.UnturnedPlayer player, ref UnityEngine.Color color, string message, SDG.Unturned.EChatMode chatMode, ref bool cancel)
        {
            ChatControlPlayerComponent component = player.GetComponent<ChatControlPlayerComponent>();

            if (!player.HasPermission("ChatControl.IgnoreBadwords"))
                foreach (string badword in ChatControl.Instance.Configuration.Instance.Badwords)
            {
                if (message.ToLower().Contains(badword.ToLower()))
                {
                    UnturnedChat.Say(player, Translate("badword_detected", badword, ++component.Warnings));
                    break;
                }
            }

            if (!player.HasPermission("ChatControl.IgnoreMute")) {
                if (component.Warnings >= Configuration.Instance.WarningsBeforeMute)
                {
                    component.IsMuted = true;
                }
                cancel = component.IsMuted;
                return;
            }

            if (Configuration.Instance.WarningsBeforeKick > 0 && component.Warnings >= Configuration.Instance.WarningsBeforeKick)
            {
                player.Kick(Translate("kick_ban_reason"));
                return;
            }
            if (Configuration.Instance.WarningsBeforeBan > 0 && component.Warnings >= Configuration.Instance.WarningsBeforeBan)
            {
                player.Ban(Translate("kick_ban_reason"), Configuration.Instance.BanDuration);
                return;
            }

        }

        protected override void Unload()
        {
            UnturnedPlayerEvents.OnPlayerChatted -= UnturnedPlayerEvents_OnPlayerChatted;
        }
    }
}
