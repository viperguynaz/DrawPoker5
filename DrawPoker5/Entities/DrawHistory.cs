using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPoker5.Entities
{
    public class DrawHistory
    {
        public Hand HandBefore { get; set; }

        public Hand HandAfter { get; set; }
        public int CardsDrawn { get; set; }

        public DrawHistory(Hand handBefore, int cardsDrawn, Hand handAfter)
        {
            HandBefore = handBefore;
            HandAfter = handAfter;
            CardsDrawn = cardsDrawn;
        }
    }
}
