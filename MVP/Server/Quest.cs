using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP.Server
{
    public class Quest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TargetNPC { get; set; }
        public string TargetRoom { get; set; }
        public string TargetItem { get; set; }
        public string RequiredItem { get; set; }
        public int RewardMoney { get; set; }
    }
}
