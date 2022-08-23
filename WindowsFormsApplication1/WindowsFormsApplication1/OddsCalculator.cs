using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class OddsCalculator
    {

        // Row = dealer visible card 1-10, column = chance to get 17, 18, 19, 20, 21
        private static double[,] dealerOddsTable = new double[,] 
        {
            //          17      18      19      20      21
            /* 1 */ { 0.1889, 0.1889, 0.1889, 0.1889, 0.0778 },
            /* 2 */ { 0.1398, 0.1349, 0.1296, 0.1240, 0.1179 },
            /* 3 */ { 0.1350, 0.1304, 0.1255, 0.1203, 0.1147 },
            /* 4 */ { 0.1304, 0.1259, 0.1213, 0.1164, 0.1112 },
            /* 5 */ { 0.1222, 0.1222, 0.1177, 0.1131, 0.1082 },
            /* 6 */ { 0.1654, 0.1062, 0.1062, 0.1017, 0.0971 },
            /* 7 */ { 0.3685, 0.1377, 0.0786, 0.0786, 0.0740 },
            /* 8 */ { 0.1285, 0.3593, 0.1285, 0.0693, 0.0693 },
            /* 9 */ { 0.1199, 0.1199, 0.3507, 0.1199, 0.0608 },
            /* 10 */ { 0.1207, 0.1207, 0.1207, 0.3707, 0.0373 }
        };

        public static double OverDraw(Deck pCards, Deck allCards)
        {
            double returnValue = 0;

            int maxPointsDrawable = 21 - pCards.laskePisteet();
            double overDraw = 0;

            foreach (Card c in allCards.korttiLista)
            {
                if(c.pisteet() > maxPointsDrawable && c.pisteet() != 11)
                {
                    overDraw++;
                }
            }

            if (pCards.laskePisteet() == 21)
            {
                returnValue = 100;
            }
            else if(pCards.laskePisteetAceOne() < 12)
            {
                returnValue = 0;
            }
            else
            {
                returnValue = overDraw / allCards.korttiLista.Count() * 100;
            }

            
            return returnValue;
        }

        public static double DealerWins(Card dealerVisibleCard, int pPoints)
        {
            double returnValue = 0;

            int rowIndex = dealerVisibleCard.pisteet() - 1 == 10 ? 0 : dealerVisibleCard.pisteet() - 1;

            int columnStartingIndex = pPoints - 17 > 0 ? pPoints - 17 : 0;

            for (int i = columnStartingIndex; i < dealerOddsTable.GetLength(1); i++)
            {
                returnValue += dealerOddsTable[rowIndex, i];
            }

            return returnValue * 100;

        }
    }
}
