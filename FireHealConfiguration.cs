using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireHeal
{
    public class FireHealConfiguration : IRocketPluginConfiguration
    {
        public int cooldownMin { get; set; }
        public byte healAmount { get; set; }
        public void LoadDefaults()
        {
            cooldownMin = 20;
            healAmount = 40;
        }
    }
}
