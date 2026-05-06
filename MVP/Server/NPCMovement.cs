using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP.Server
{
    public class NPCMovement
    {
        private Dictionary<string, Room> rooms;

        public NPCMovement(Dictionary<string, Room> rooms)
        {
            this.rooms = rooms;
        }

        public void MoveNPC(NPC npc, string newRoomId)
        {
            if (!rooms.ContainsKey(newRoomId)) return;

            npc.CurrentRoomId = newRoomId;
        }
    }
}
