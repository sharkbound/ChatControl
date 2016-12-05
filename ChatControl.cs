using Rocket.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.API.Collections;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.API;
using UnityEngine;
using SDG.Unturned;

namespace fr34kyn01535.ChatControl
{
    public class ChatControl : RocketPlugin<ChatControlConfiguration>
    {
        public static ChatControl Instance;
        public static Color MessageColor;
        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList() {
                    { "command_player_not_found", "Player not found"},
                    { "command_mute", "{0} is now muted"},
                    { "command_unmute", "{0} is now unmuted, his warnings have been reset"},
                    { "kick_ban_reason", "Too many badwords"},
                    { "you_are_muted", "You are muted, talk to the hand"},
                    { "badword_detected", "{0} is a badword, don't use it or bad things will happen to you. This is your {1}. warning."},
                    { "command", "warn \"{0}\" \"Swearing\""},
                };
            }
        }

        protected override void Load()
        {
            Instance = this;
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, Palette.SERVER);
            UnturnedPlayerEvents.OnPlayerChatted += UnturnedPlayerEvents_OnPlayerChatted;
        }

        private void UnturnedPlayerEvents_OnPlayerChatted(Rocket.Unturned.Player.UnturnedPlayer player, ref UnityEngine.Color color, string message, SDG.Unturned.EChatMode chatMode, ref bool cancel)
        {
            ChatControlPlayerComponent component = player.GetComponent<ChatControlPlayerComponent>();

            if (!player.HasPermission("ChatControl.IgnoreBadwords") && message.ToCharArray()[0] != '/' && chatMode != EChatMode.GROUP)
            {
                foreach (string badword in ChatControl.Instance.Configuration.Instance.Badwords)
                {
                    if (message.ToLower().Contains(badword.ToLower()))
                    {
                        //UnturnedChat.Say(player, Translate("badword_detected", badword, ++component.Warnings), MessageColor);
                        CommandWindow.input.onInputText(Translate("command", player.DisplayName));
                        cancel = true;
                        break;
                    }
                }
            }

            if (component.IsMuted)
            {
                cancel = true;
                UnturnedChat.Say(player, Translate("you_are_muted"), MessageColor);
                return;
            }

        }

        protected override void Unload()
        {
            UnturnedPlayerEvents.OnPlayerChatted -= UnturnedPlayerEvents_OnPlayerChatted;
        }
    }
}
