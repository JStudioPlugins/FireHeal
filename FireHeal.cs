using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace FireHeal
{
    public class FireHeal : RocketPlugin<FireHealConfiguration>
    {
        public Dictionary<CSteamID, DateTime> Healed;
        public static FireHeal Instance { get; private set; }

        protected override void Load()
        {
            Instance = this;

            Logger.Log("FireHeal is loaded. Heal by fire!");
            UnturnedPlayerEvents.OnPlayerUpdateGesture += UnturnedPlayerEvents_OnPlayerUpdateGesture;

            Healed = new Dictionary<CSteamID, DateTime>();
        }

        private void UnturnedPlayerEvents_OnPlayerUpdateGesture(Rocket.Unturned.Player.UnturnedPlayer player, UnturnedPlayerEvents.PlayerGesture gesture)
        {
            if (gesture == UnturnedPlayerEvents.PlayerGesture.Rest_Start && player.CompareFireHealPermissions())
            {
                int cooldown = player.GetFireHealCooldown();
                byte amount = player.GetFireHealAmount();

                if (Healed.ContainsKey(player.CSteamID))
                {
                    TimeSpan time = DateTime.Now - Healed[player.CSteamID];

                    if (time.TotalMinutes >= cooldown)
                    {
                       
                        if (player.Player.life.temperature == SDG.Unturned.EPlayerTemperature.WARM)
                        {
                            player.Heal(amount);
                            Healed[player.CSteamID] = DateTime.Now;
                            UnturnedChat.Say(player, Translate("HealFire", cooldown));
                            return;
                        }
                        else
                        {
                            UnturnedChat.Say(player, Translate("NotNearFire"));
                        }
                    }
                }
                else
                {
                    if (player.Player.life.temperature == SDG.Unturned.EPlayerTemperature.WARM)
                    {
                        player.Heal(amount);
                        Healed.Add(player.CSteamID, DateTime.Now);
                        UnturnedChat.Say(player, Translate("HealFire", cooldown));
                        return;
                    }
                    else
                    {
                        UnturnedChat.Say(player, Translate("NotNearFire"));
                        
                    }
                }
            }
        }

        protected override void Unload()
        {
            UnturnedPlayerEvents.OnPlayerUpdateGesture -= UnturnedPlayerEvents_OnPlayerUpdateGesture;
            Healed = null;
            Logger.Log("FireHeal is unloaded. Heal by fire!");
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "NotNearFire", "You are not near a fire!" },
            { "HealFire", "Healed, please wait {0} minutes to be healed again" }
        };
    }
}
