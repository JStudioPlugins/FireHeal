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
        public HealGroup[] groups { get; set; }

        public void LoadDefaults()
        {
            groups = new HealGroup[]
            {
                new HealGroup()
                {
                    permission = "fireheal.standard",
                    amount = 40,
                    cooldownMin = 1
                }
            };
        }
    }
}
