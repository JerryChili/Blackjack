using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Media;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        
        //
        Deck peliPakka = new Deck();
        Deck jakajaKortit = new Deck();
        Deck pelaajaKortit = new Deck();

        int dealerWins = 0;
        int playerWins = 0;

        SoundPlayer blackjackSound = new SoundPlayer(@"C:/Users/jeri.juntunen/Documents/Visual Studio 2015/Projects/WindowsFormsApplication1/WindowsFormsApplication1/Resources/pipo2.wav");

        Deck calculateOddsDeck = new Deck();

        bool hideFirst = true;

        bool autoPlay = false;

        private System.Timers.Timer timer = null;

        public static Dictionary<string, Bitmap> CARD_PICTURES = new Dictionary<string, Bitmap>();

        bool gameStarted = false;
        public Form1()
        {
            InitializeComponent();

            peliPakka.taytaPakka();
            calculateOddsDeck.taytaPakka();

            foreach (Card c in peliPakka.korttiLista)
            {
                string key = c.getPictureKey();
                CARD_PICTURES.Add(key, Card.GetPictureResourcex(key));
            }

            for (int i = 0; i <= 4; i++)
            {
                peliPakka.taytaPakka();
            }

           // peliPakka.Sekoita();
            button2.Enabled = false;
            button3.Enabled = false;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            play();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            hit();
        }

        private void updateScene()
        {
            button1.Enabled = !gameStarted;
            button2.Enabled = gameStarted;
            button3.Enabled = gameStarted;

            if (hideFirst && gameStarted)
            {
                label13.Text = "?";
                label13.Visible = false;

                label19.Text = String.Format("{0:0.00}", OddsCalculator.DealerWins(jakajaKortit.korttiLista[1], pelaajaKortit.laskePisteet()));
            }
            else
            {
                label13.Text = jakajaKortit.laskePisteet().ToString();
            }

            label22.Text = (playerWins - dealerWins).ToString();

            label23.Text = playerWins.ToString();
            label24.Text = dealerWins.ToString();

            //Points
            label13.Text = jakajaKortit.laskePisteet().ToString();
            label14.Text = pelaajaKortit.laskePisteet().ToString();

            label15.Text = String.Format("{0:0.00}", OddsCalculator.OverDraw(pelaajaKortit, calculateOddsDeck));

            

            List<Label> playerLabels = new List<Label>
            {
                label6,
                label7,
                label8,
                label9,
                label10
            };

            List<Label> dealerLabels = new List<Label>
            {
                label1,
                label2,
                label3,
                label4,
                label5
            };
            List<PictureBox> dealerBoxes = new List<PictureBox>
            {
                pictureBox1,
                pictureBox2,
                pictureBox3,
                pictureBox4,
                pictureBox5
            };
            List<PictureBox> playerBoxes = new List<PictureBox>
            {
                pictureBox6,
                pictureBox7,
                pictureBox8,
                pictureBox9,
                pictureBox10
            };

            renderCards(playerLabels, playerBoxes, pelaajaKortit.korttiLista, false);

            renderCards(dealerLabels, dealerBoxes, jakajaKortit.korttiLista, hideFirst);
        }
        private void renderCards(List<Label> labels, List<PictureBox> boxes, List<Card> cards, bool hide)
        {
            
            IEnumerator enumerator =
                cards.GetEnumerator();
            IEnumerator enumeratorBoxes =
                boxes.GetEnumerator();

            bool cardHidden = false;

            foreach (Label l in labels)
            {
                Card c = enumerator.MoveNext() ? (Card)enumerator.Current : null;
                PictureBox pb = enumeratorBoxes.MoveNext() ? (PictureBox)enumeratorBoxes.Current : null;

                if (hide == true && cardHidden == false && gameStarted)
                {
                    l.Text = "";
                    pb.Image = Card.GetPictureResourcex("BACK");
                    cardHidden = true;
                }

                else if (c != null && pb != null)
                {
                    l.Text = (c.Arvo.ToString() + " " + c.Maa);

                    pb.Image = Card.GetPictureResourcex(c.getPictureKey());
                }
                else
                {
                    l.Text = "";
                    pb.Image = null;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pass();
        }

        private void reset()
        {
            gameStarted = false;
            jakajaKortit.korttiLista.Clear();
            pelaajaKortit.korttiLista.Clear();
            hideFirst = true;

            if (!autoPlay)
            {
                updateScene();
            }
        }

        private void Lose()
        {
            dealerWins++;

            if (!autoPlay)
            {
                MessageBox.Show("Hävisit! " + "\n\nSinulla oli " + pelaajaKortit.laskePisteet() + " pistettä" + "\n\nJakajalla oli " + jakajaKortit.laskePisteet() + " pistettä.");
            }
            updateScene();


            reset();
        }

        private void Win()
        {
            playerWins++;

            if (!autoPlay)
            {
                MessageBox.Show("Voitit! " + "\n\nSinulla oli " + pelaajaKortit.laskePisteet() + " pistettä" + "\n\nJakajalla oli " + jakajaKortit.laskePisteet() + " pistettä.");
            }
            updateScene();


            reset();
        }

        private void Blackjack()
        {

            playerWins++;
            updateScene();
            blackjackSound.Play();

            if (!autoPlay)
            {
                MessageBox.Show("Blackjack! " + "\n\nSinulla oli " + pelaajaKortit.laskePisteet() + " pistettä" + "\n\nJakajalla oli " + jakajaKortit.laskePisteet() + " pistettä.");
            }

            reset();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            autoPlay = true;

            timer = new System.Timers.Timer((double)numericUpDown1.Value);
            timer.Elapsed += autoPlayTimerEvent;
            timer.AutoReset = true;
            timer.Enabled = true;

            updateScene();
        }

        private void autoPlayTimerEvent(object source, ElapsedEventArgs e)
        {
             this.Invoke(new MethodInvoker(delegate ()
           {
               if(peliPakka.korttiLista.Count() < 400)
               {
                   peliPakka.korttiLista.Clear();
                   for (int i = 0; i < 10; i++)
                   {
                       peliPakka.taytaPakka();
                   }
               }

               play();

           }));
        }

        private void play()
        {
            gameStarted = true;

            peliPakka.taytaPakka();

            jakajaKortit.korttiLista.Add(peliPakka.nostaKortti());
            jakajaKortit.korttiLista.Add(peliPakka.nostaKortti());

            pelaajaKortit.korttiLista.Add(peliPakka.nostaKortti());
            pelaajaKortit.korttiLista.Add(peliPakka.nostaKortti());

            //if (!autoPlay)
            {
                updateScene();
            }

            if (pelaajaKortit.laskePisteet() == 21)
            {
                Blackjack();
            }
            else if (autoPlay == true)
            {
                autoPlayHit();
            }
        }

        private void hit()
        {
            pelaajaKortit.korttiLista.Add(peliPakka.nostaKortti());

            updateScene();

            if (pelaajaKortit.laskePisteet() > 21 && !autoPlay)
            {
                hideFirst = false;
                updateScene();
                Lose();
            }
        }

        private void pass()
        {
            while (jakajaKortit.laskePisteet() < 17)
            {
                jakajaKortit.korttiLista.Add(peliPakka.nostaKortti());
            }
            updateScene();

            if (jakajaKortit.laskePisteet() > 21)
            {
                Win();
            }

            else if (jakajaKortit.laskePisteet() >= pelaajaKortit.laskePisteet())
            {
                Lose();
            }
            else
            {
                Win();
            }
        }

        private void autoPlayHit()
        {
            bool passing = false;
            //Draw cards
            while (passing == false)
            {
                //check if plr has >50% chance to win
                if (OddsCalculator.DealerWins(jakajaKortit.korttiLista[1], pelaajaKortit.laskePisteet()) < 50)
                {
                    passing = true;
                }
                else if (OddsCalculator.OverDraw(pelaajaKortit, calculateOddsDeck) < OddsCalculator.DealerWins(jakajaKortit.korttiLista[1], pelaajaKortit.laskePisteet()))
                {
                    hit();
                }
                else
                {
                    passing = true;
                }
            }
            //Lose if overdraw
            if(pelaajaKortit.laskePisteet() > 21)
            {
                Lose();
            }
            else
            {
                pass(); //stay
            }
            //Pass
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(timer != null)
            {
                timer.Enabled = false;
                timer.Stop();
                timer = null;
            }

            autoPlay = false;
            updateScene();
        }
    }
}





















































































//  |:=)
