using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Card
    {
        public enum Maat
        {
            Hertat, // 0
            Ruudut, // 1
            Padat, // 2
            Ristit // 3
        }

        public int Arvo
        {
            get;
            set;
        }
        public Maat Maa
        {
            get;
            set;
        }
        public Card(int Arvo, Maat Maa)
        {
            this.Arvo = Arvo;
            this.Maa = Maa;
        }

        public int pisteet()
        {
            switch (Arvo)
            {
                case 1:
                    return 11;
                case 11:
                case 12:
                case 13:
                    return 10;
                default:
                    return Arvo;
            }
        }
        public static Bitmap GetPictureResourcex(string key)
        {
            return WindowsFormsApplication1.Resource1.ResourceManager.GetObject(key) as Bitmap;
        }

        public string getPictureKey()
        {
            char suite = getSuiteId();
            string value = getValueId();
            return suite + value;
        }

        private char getSuiteId()
        {
            switch (Maa.ToString())
            {
                case "Hertat":return 'H';
                case "Ruudut":return 'D';
                case "Ristit":return 'C';
                case "Padat":return 'S';
                default: return 'x';
            }
        }

        private string getValueId()
        {
            switch (Arvo)
            {
                case 11: return "J";
                case 12: return "Q";
                case 13: return "K";
                default: return Arvo.ToString();
            }
        }
    }
}
