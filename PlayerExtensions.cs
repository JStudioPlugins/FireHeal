using Rocket.API;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireHeal
{
    public static class PlayerExtensions
    {
        public static bool CompareFireHealPermissions(this UnturnedPlayer player)
        {
            foreach (HealGroup group in FireHeal.Instance.Configuration.Instance.groups)
            {
                if (player.HasPermission(group.permission))
                {
                    return true;
                }
            }
            return false;
        }

        public static int GetFireHealCooldown(this UnturnedPlayer player)
        {
            foreach (HealGroup group in FireHeal.Instance.Configuration.Instance.groups)
            {
                if (player.HasPermission(group.permission))
                {
                    return group.cooldownMin;
                }
            }
            return FireHeal.Instance.Configuration.Instance.groups[0].cooldownMin;
        }

        public static byte GetFireHealAmount(this UnturnedPlayer player)
        {
            foreach (HealGroup group in FireHeal.Instance.Configuration.Instance.groups)
            {
                if (player.HasPermission(group.permission))
                {
                    return group.amount;
                }
            }
            return FireHeal.Instance.Configuration.Instance.groups[0].amount;
        }
    }
}
