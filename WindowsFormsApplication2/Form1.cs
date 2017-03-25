﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace WindowsFormsApplication2
{
    public partial class MasterMind : Form
    {
        public MasterMind()
        {
            InitializeComponent();
        }

        private void MasterMind_Load(object sender, EventArgs e)
        {
        }

        int skaitDydis = 4;     //pradinis skaitmenu kiekis
        int skaitMax = 9999;    //pradinis didziausias galimas skaicius
        int count = 1;          //pradinis bandymu speti kiekis
        string laikas;          //laiko kintamasis
        public string timeFinal = "00:13";  //galutinis laiko rezultatas
        public string countFinal = "4";     //galutinis bandymu skaicius
        private Stopwatch stopWatch;    // Taimerio objekto deklaravimas


        //string laikas2 = txtLaikas

                    //RADIO BUTTONS

        //Radio button 6 -> ijungia 6 skaitmenu skaiciaus zaidima
        private void radio6_CheckedChanged(object sender, EventArgs e)
        {
            skaitDydis = 6;
            skaitMax = 999999;
            MessageBox.Show("Paspauskite mygtuka 'Pradeti'");
        }

        //Radio button 4 -> ijungia 6 skaitmenu skaiciaus zaidima
        private void radio4_CheckedChanged(object sender, EventArgs e)
        {
            if (skaitDydis == 6)
            {
                skaitDydis = 4;
                skaitMax = 9999;
                lstItems.Text = "";
            }
        }

                        // BUTTONS

        // Mygtukas "Pradeti"
        private void button1_Click(object sender, EventArgs e)
        {
            // Istrinam visu langu turini ir nuresetinam laikmati
            lstItems.Items.Clear();
            txtResult.Text = "";
            txtResult2.Text = "";
            txtGuess.Text = "";
            _ticks = 0;
            count = 1;     // count = bandymu speti kiekis

            HashSet<int> setas1 = skaiciausGeneratorius();
            var outGen = string.Join("", setas1);
            txtGen.Text = outGen;
            stopWatch = new Stopwatch();
            stopWatch.Start();
            timer1.Start();
            txtGuess.Select();
        }

        // Mygtukas "Tikrinti"
        private void compareNum(object sender, EventArgs e)
        {
            AcceptButton = compare;
            txtGuess.Select(); // permeta kursoriu i spejimo lageli

            // patikrinam ar kas nors ivesta 
            if (txtGen.Text == "")
            {
                MessageBox.Show("Paspauskite mygtuka 'Pradeti'");
            }

            // patikrinam ar teisingo ilgio ivestas skaicius
            else if (txtGuess.Text.Length == skaitDydis)
            {
                string a = txtGuess.Text;
                string b = txtGen.Text;

                // patikrinam ar ivestas skaicius sutampa su sugeneruotu
                if (a == b)
                {
                    timer1.Stop();
                    stopWatch.Stop();
                    _ticks = 0;
                    laikas = stopWatch.Elapsed.Minutes.ToString("00") + " : " + stopWatch.Elapsed.Seconds.ToString("00");
                    string countFinal = count.ToString();
                    txtLaikas.Text = laikas;
                    txtBandymai.Text = countFinal;
                    MessageBox.Show("Atspejote is " + countFinal + " bandymu" + "\nJusu laikas: " + laikas);

                    // Sukuriamas registracijos Form2 objektas ir jis atidaroma registracijos formos langas
                    registrationForm frm2 = new registrationForm();
                    frm2.Show();
                    count = 1;

                }
                else
                // Tikrinam kiek skaitmenu yra sugeneruotame skaiciuje (n) ir kiek skaitmenu atitinka savo vieta (m)
                {
                    int n = 0; int m = 0;
                    for (int i = 0; i < skaitDydis; i++)
                    {
                        if (b.Contains(a[i]))
                        {
                            n++;
                        }
                        if (a[i] == b[i])
                        {
                            m++;
                        }
                    }

                    // I tarpiniu resulatatu langelius isvedame tarpinius rezultatus
                    txtResult.Text = "Atspeta: " + n;
                    txtResult2.Text = "Savo vietoje: " + m;

                    // I visu resulatatu saraso langa isvedame tarpinius rezultatus
                    lstItems.Items.Add(count + ".   " + a + "       " + n + " : " + m);

                    // Istrinam spejimo langelio turini
                    txtGuess.Text = "";

                }
                count++;
            }
            else
            // Jeigu skaicius neteisingo ilgio, prasome ivesti teisingo ilgio skaiciu
            {
                MessageBox.Show("Iveskite " + skaitDydis + " skaitmenu skaiciu");
            }
        }

        // Mygtukas Registruotis -> atidaro registracijos langą Form2
        private void btnRegister_Click(object sender, EventArgs e)
        {
            registrationForm frm2 = new registrationForm();
            frm2.Show();
        }

                        //METODAI

        // Metodas Random Skaiciaus Generatorius
        private HashSet<int> skaiciausGeneratorius()
        {
            // Sugeneruojam random skaiciu ir Seta 'masyvas', kuriame issaugosim to skaiciaus skaitmenis(setas tam, kad visi skaiciaus skaitmenys butu skirtingi)
            Random rand = new Random();
            int skaicius;

            HashSet<int> masyvas = new HashSet<int>();
            do
            {
                skaicius = rand.Next(0, skaitMax);
                masyvas.Clear();
                for (int i = 0; i < skaitDydis; i++)
                {
                    int skaitmuo = skaicius % 10;
                    masyvas.Add(skaitmuo);
                    skaicius = skaicius / 10;
                }

            } while (masyvas.Count < skaitDydis);
            return masyvas;
        }


        // Metodas laiko parodymui formos1 antrasteje
        private int _ticks = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            _ticks++;
            this.Text = _ticks.ToString();
        }


        //Metodas laiko isvedimas i placius vandenis

            public string LaikasBle()
        { 
            string naujasLaikas;
            naujasLaikas = txtBandymai.Text;
            return naujasLaikas;
        }


                    // MENU FUNKCIJOS

        // Menu punktas Help -> parodo zaidimo taisykles
        private void zaistiDarKartaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string help = "Žaidimo tikslas atspėti kompiuterio sugeneruotą skaičių. Tam reikia skirti kuo mažiau bandymų ir laiko.\nGalima pasirinkti 4-ių arba 6-ių skaitmenų skaičių. Visi skaitmenys skaičiuje yra skirtingi, t.y skaičiaus 1123 negali būti.\n\nSĖKMĖS!";
            MessageBox.Show(help);
        }

        // Menu punktas Exit -> uzdaro visa programa
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ar norite zaidima baigti?", "Zaidimo pabaigimas", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        

                    //TEXT BOXAI, LABEL ir 

        // Nematomas sugeneruoto skaiciaus textBox
        private void txtGen_TextChanged(object sender, EventArgs e)
        {
        }

        // Tarpinio rezultato langeliai 1 ir 2
        private void txtResult_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtResult2_TextChanged(object sender, EventArgs e)
        {
        }

        // Visu rezultatu saraso langas 1
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        //Spejimo ivedimo langas
        private void txtGuess_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtLaikas_TextChanged(object sender, EventArgs e)
        {

        }
      
        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void lblConnection_Click(object sender, EventArgs e)
        {
        }


        //Pagalbines funkcijos

        static void bandymas1()
        {
            txtGuess.Text = txtLaikas.Text;
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            txtGuess.Text = txtLaikas.Text;
        }
    }
}
