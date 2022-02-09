using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireHeal
{
    public class FireHeal : RocketPlugin<FireHealConfiguration>
    {
        public List<FH> healed { get; set; }
        protected override void Load()
        {
            Logger.Log("FireHeal is loaded. Heal by fire!");
            UnturnedPlayerEvents.OnPlayerUpdateGesture += UnturnedPlayerEvents_OnPlayerUpdateGesture;
            healed = new List<FH>();
        }

        private void UnturnedPlayerEvents_OnPlayerUpdateGesture(Rocket.Unturned.Player.UnturnedPlayer player, UnturnedPlayerEvents.PlayerGesture gesture)
        {
            if (gesture == UnturnedPlayerEvents.PlayerGesture.Rest_Start)
            {
                var fh = healed.FirstOrDefault(x => x.player == player.CSteamID);
                var fhi = healed.FindIndex(x => x.player == player.CSteamID);

                if (fh != null)
                {
                    TimeSpan time = DateTime.Now - fh.time;
                    if (time.TotalMinutes >= Configuration.Instance.cooldownMin)
                    {
                       
                        if (player.Player.life.temperature == SDG.Unturned.EPlayerTemperature.WARM)
                        {
                            player.Heal(Configuration.Instance.healAmount);
                            healed.RemoveAt(fhi);
                            healed.Add(new FH { player = player.CSteamID, time = DateTime.Now});
                            UnturnedChat.Say(player, $"Healed, please wait {Configuration.Instance.cooldownMin} minutes to be healed again");
                            return;
                        }
                        else
                        {
                            UnturnedChat.Say(player, $"You are not near a fire!");
                        }
                    }
                }
                else
                {
                    if (player.Player.life.temperature == SDG.Unturned.EPlayerTemperature.WARM)
                    {
                        player.Heal(Configuration.Instance.healAmount);
                        healed.Add(new FH { player = player.CSteamID, time = DateTime.Now });
                        UnturnedChat.Say(player, $"Healed, please wait {Configuration.Instance.cooldownMin} minutes to be healed again");
                        return;
                    }
                    else
                    {
                        UnturnedChat.Say(player, $"You are not near a fire!");
                    }
                }
            }
        }

        protected override void Unload()
        {
            UnturnedPlayerEvents.OnPlayerUpdateGesture -= UnturnedPlayerEvents_OnPlayerUpdateGesture;
            healed = null;
            Logger.Log("FireHeal is unloaded. Heal by fire!");
        }
    }
}
