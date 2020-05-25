using System;
using System.Threading;
using System.Windows.Forms;

namespace yazlab3
{
    public partial class Form1 : Form
    {
        Bayes.naivebayes naivebayes;
        public Form1()
        {
            InitializeComponent();
        }

        private void run_Click(object sender, EventArgs e)
        {
            mesgul.Visible = true;


            //naive bayes basladı
            Thread x = new Thread(()=> { 
            naivebayes = new Bayes.naivebayes(@"1150haber\raw_texts");
            naivebayes.Tahmin();
            });
            x.Start();
            x.Join();
            //tum islem burada bitti

            var tahminler = naivebayes.tahminler; // tahminleri alıyoruz 
            string[] kategoriler = { "ekonomi", "magazin", "saglik", "siyasi", "spor" };

            //artık ekrandaki nesneler gozukabilir
            asil.Visible = true; 
            label1.Visible = true;
            label2.Visible = true;


            //tablo doldurma islemleri
            Label baslik = new Label();
            baslik.Text = "kategoriler";
            degerler.Controls.Add(baslik, 0, 0);

            Label baslik1 = new Label();
            baslik1.Text = "Precision";
            degerler.Controls.Add(baslik1, 1, 0);

            Label baslik2 = new Label();
            baslik2.Text = "Recall";
            degerler.Controls.Add(baslik2, 2, 0);

            Label baslik3 = new Label();
            baslik3.Text = "Fmeasure";
            degerler.Controls.Add(baslik3, 3, 0);



            for (int i = 0; i < kategoriler.Length; i++)
            {
                Label text = new Label();
                text.Text = kategoriler[i];
                degerler.Controls.Add(text, 0,i+1);
            }

            for (int i = 0; i < kategoriler.Length; i++)
            {
                Label text = new Label();
                text.Text = Convert.ToString(naivebayes.Precision(kategoriler[i]));
                degerler.Controls.Add(text, 1, i + 1);
            }

            for (int i = 0; i < kategoriler.Length; i++)
            {
                Label text = new Label();
                text.Text = Convert.ToString(naivebayes.Recall(kategoriler[i]));
                degerler.Controls.Add(text, 2, i + 1);
            }

            for (int i = 0; i < kategoriler.Length; i++)
            {
                Label text = new Label();
                text.Text = Convert.ToString(naivebayes.Fmeasure(kategoriler[i]));
                degerler.Controls.Add(text, 3, i + 1);
            }//tablo doldurma bitti


            //asıl degerler ve tahminleri yazdırma
            for(int i = 0; i < tahminler.Count; i++)
            {
                asil.Text += i+")\t" +tahminler[i].Item1+"\t\t\t\t\t"+ tahminler[i].Item2 + "\n";
                
            }
            mesgul.Text = "İşlem Tamamlandı";



        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}
