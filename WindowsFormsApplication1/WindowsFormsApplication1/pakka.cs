using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Deck
    {
        private static Random rng = new Random();
        public List<Card> korttiLista = new List<Card>();

        public void taytaPakka()
        {
            Card.Maat maaHertta = (Card.Maat)(0);
            Card.Maat maaRuutu = (Card.Maat)(1);
            Card.Maat maaPata = (Card.Maat)(2);
            Card.Maat maaRisti = (Card.Maat)(3);

            for (int i = 1; i <= 13; i++)
            {

                /* foreach (Card.Maat Maa in Card.Maat)
                 {
                     // 1 H ... 13 H
                     // 1 D ... 13 D
                 }*/
                korttiLista.Add(new Card(i, maaHertta));
                korttiLista.Add(new Card(i, maaRuutu));
                korttiLista.Add(new Card(i, maaPata));
                korttiLista.Add(new Card(i, maaRisti));
            }
        }
        public void Sekoita()
        {
            int n = korttiLista.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = korttiLista[k];
                korttiLista[k] = korttiLista[n];
                korttiLista[n] = value;
            }
        }
        public Card nostaKortti()
        {

            int randomIndex = rng.Next(korttiLista.Count);
            Card temp = korttiLista[randomIndex];
            korttiLista.RemoveAt(randomIndex);
            return temp;
        }

        public int laskePisteet()
        {
            int total = 0;
            int aces = 0;

            foreach(Card c in korttiLista)
            {
                if(c.Arvo == 1)
                {
                    aces++;
                }
                total += c.pisteet();
            }

            for (int i = 0; i < aces; i++)
            {
                if(total > 21)
                {
                    total -= 10;
                }
            }

            return total;
        }

        public int laskePisteetAceOne()
        {
            int returnValue = 0;
            int aces = 0;

            foreach(Card c in korttiLista)
            {
                if(c.Arvo == 1)
                {
                    aces++;
                }
                returnValue += c.pisteet();
            }

            for (int i = 0; i < aces; i++)
            {
                returnValue -= 10;
            }

            return returnValue;
        }
    }
}
