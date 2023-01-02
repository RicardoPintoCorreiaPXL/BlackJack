using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class HistoryObject
    {
        public int count { get; set; }
        public int bet { get; set; }
        public int playerPoints { get; set; }
        public int computerPoints { get; set; }

        public HistoryObject(int count, int bet, int playerPoints, int computerPoints)
        {
            this.count = count;
            this.bet = bet;
            this.playerPoints = playerPoints;
            this.computerPoints = computerPoints;
        }
    }
}
