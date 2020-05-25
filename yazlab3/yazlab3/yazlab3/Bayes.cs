using Nuve.Morphologic.Structure;
using Nuve.Lang;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System;

namespace yazlab3
{
    class Bayes
    {
        public class haber // Her haber icin bir tane uretilir. Toplam 1150tane
        {
            public string kategori; // Haberin kategorisi
            public string metin; //Haberin metni. Sadece gramlari bulmak icin kullaniyoruz
            public Dictionary<string, int> gramlar = new Dictionary<string, int>(); //iki ve uc gramlar burada tutuluyor
            public haber(string metin, string kategori) // Yapıcı metot. Haber metni ve kategori ile cagırılır
            {
                metin = Regex.Replace(metin, @"[^\w\s]", ""); // unicode karakterlerini silme
                metin = Regex.Replace(metin, @"\r", ""); // \r karakterlerini silme
                metin = metin.ToLower(); //Buyuk harfleri kucuk harf yapma
                metin = AnlamsizKelimeleriTemizle(metin); // stop wordler temizlenir. Baglaclar, zamirler, harfler vs.
                metin = CekimEkleriniTemizle(metin); // Cekim ekleri temizlenir. Yapım ekleri silinmez.Sözcüklerin gövdeleri kullanilacak.
                metin = Regex.Replace(metin, @"\s+", "");
                this.kategori = kategori;
                this.metin = metin;
                for (int i = 0; i < metin.Length - 2; i++) //iki gram bulma
                {
                    if (gramlar.ContainsKey(metin.Substring(i, 2))) // aynı gramdan fazladan varsa sayısını artırıyor
                        gramlar[metin.Substring(i, 2)]++;
                    else
                        gramlar.Add(metin.Substring(i, 2), 1);  // iki elemanlı stringler sözlüge ekleniyor
                }
                for (int i = 0; i < metin.Length - 3; i++)//üç gram bulma
                {
                    if (gramlar.ContainsKey(metin.Substring(i, 3)))// aynı gramdan fazladan varsa sayısını artırıyor
                        gramlar[metin.Substring(i, 3)]++;
                    else
                        gramlar.Add(metin.Substring(i, 3), 1);// üç elemanlı stringler sözlüge ekleniyor
                }
            }
            string AnlamsizKelimeleriTemizle(string metin) //Stop wordler temizlenir
            {
                string[] AnlamsizKelimeler = File.ReadAllLines(@"1150haber\anlamsizKelimeler.txt", Encoding.UTF8); //Stop word dosyası açılır
                for (int i = 0; i < AnlamsizKelimeler.Length; i++)
                {
                    metin.Replace(AnlamsizKelimeler[i], ""); // Temizleme işlemi
                }
                return metin;
            }

            string CekimEkleriniTemizle(string metin) // çekim eklerini temizler
            {
                string[] kelimeler = metin.Split(' ');  //metindeki her kelimeyi alır
                Language tr = LanguageFactory.Create(LanguageType.Turkish); //nuve kütüphanesi
                string sonhali = "";
                for (int i = 0; i < kelimeler.Length; i++)
                {
                    IList<Word> kokler = tr.Analyze(kelimeler[i]); // kelimenin analizi
                    if (kokler.Count > 0) // sonuc varsa
                        sonhali += kokler[0].GetStem().GetSurface() + "_"; // kelimenin govdesini al(GetStem) ve stringe çevir(GetSurface)
                }
                return sonhali;
            }
        }
        public class haberler // Birden fazla haber tutulur. İki tane var. Egitim için haberler ve test için haberler
        {
            public List<haber> haberlistesi = new List<haber>(); //Haberlerin içinde her bir haber bu listte tutulur
            public Dictionary<string, int> KategoriSayilari = new Dictionary<string, int>(); // haberlerin içinde herbir katagoriden kaç tane var
            public haberler(string dir) // Birinci yapıcı metot yol alır ve o yoldaki tüm dosyaları açar
            {
                string[] categoriler = {"ekonomi", "magazin", "saglik", "siyasi", "spor"}; // katagoriler burada
                for (int i = 0; i < categoriler.Length; i++) // her katagori için tek tek dolaşılır
                {
                    string[] dosyalar = Directory.GetFiles(dir + @"\" + categoriler[i], "*.txt"); // dosya yoluna katagori ekleyip açıyoruz ve .txt ile biten dosyaları alıyoruz
                    for (int j = 0; j < dosyalar.Length; j++)
                        haberlistesi.Add(new haber(File.ReadAllText(dosyalar[j], Encoding.GetEncoding("ISO-8859-9")), categoriler[i])); // Her bir dosyayı burada açıyoruz. Dosyanın encodingi UTF olmadigi için böyle açmak zorundayız.
                }
            }
            public haberler(List<haber> haberler) // ikinci yapıcı metot. Haber listesi alıyor
            {
                haberlistesi = haberler; //atama
                for (int i = 0; i < haberlistesi.Count; i++) // hangi katagoriden kaç tane var
                {
                    if (KategoriSayilari.ContainsKey(haberlistesi[i].kategori))
                        KategoriSayilari[haberlistesi[i].kategori]++;
                    else
                        KategoriSayilari.Add(haberlistesi[i].kategori, 1);
                }
            }
            public void FrekansAyarı(int minFrekans) // Frekans ayarı. Egitim icin alınan haberlerde minfrekanstan küçük sayıda olan gramları siler.
            {
                Dictionary<string, int> gramToplam = new Dictionary<string, int>(); // Tüm gramlardan toplam kaç tane var. Kategoriye bakılmaksızın.
                for (int i = 0; i < haberlistesi.Count; i++) // gramları topluyoruz 
                {
                    var gramlar = haberlistesi[i].gramlar.ToList();
                    for (int j = 0; j < gramlar.Count; j++)
                    {
                        if (gramToplam.ContainsKey(gramlar[j].Key))
                            gramToplam[gramlar[j].Key] += gramlar[j].Value;
                        else
                            gramToplam.Add(gramlar[j].Key, gramlar[j].Value);
                    }
                }
                var list = gramToplam.ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Value < minFrekans) //min frekanstan küçük olan gramlar siliniyor
                        gramToplam.Remove(list[i].Key);
                }
                for (int i = 0; i < haberlistesi.Count; i++)
                {
                    var gramlist = haberlistesi[i].gramlar.ToList();
                    for (int j = 0; j < gramlist.Count; j++)
                    {
                        if (gramToplam.ContainsKey(gramlist[j].Key))
                            continue;
                        haberlistesi[i].gramlar.Remove(gramlist[j].Key); // har bir dosya dolaşılır ve içinde min frekanstan küçük olan gramlar silinir
                    }
                }
            }
        }
        public class naivebayes //naive bayes sınıfı egitim ve olasılık hesapları için kullanılır. 
        {
            public haberler egitim; // egitim için kullanılacak haberler
            public haberler test; //test için kullanılacak haberler
            public Dictionary<string, Dictionary<string, double>> ortalama = new Dictionary<string, Dictionary<string, double>>(); // harbir katagorideki herbir gramın ortalaması
            public Dictionary<string, Dictionary<string, double>> varyans = new Dictionary<string, Dictionary<string, double>>(); //her bir katagorideki her bir gramın varyansı
            public List<Tuple<string, string>> tahminler = new List<Tuple<string, string>>(); //İlk string gercek katagori ikinci string tahmin
            private Dictionary<string, double[,]> konfuzyonmatrisleri = new Dictionary<string, double[,]>(); // precision, Fmeasure ve recall hesabı için gereken matris
            private Dictionary<string, double> precision = new Dictionary<string, double>();
            private Dictionary<string, double> FMeasure = new Dictionary<string, double>();
            private Dictionary<string, double> recall = new Dictionary<string, double>();
            public naivebayes(string dir) // dosya yolu 
            {
                haberler tumhaberler = new haberler(dir); // tüm haberler burada hazırlanıyor
                Random rand = new Random(); // haberleri random sıralamak için
                int count = tumhaberler.haberlistesi.Count;
                while (count > 1)
                {
                    count--;
                    int k = rand.Next(count + 1);
                    haber haber = tumhaberler.haberlistesi[k];
                    tumhaberler.haberlistesi[k] = tumhaberler.haberlistesi[count];
                    tumhaberler.haberlistesi[count] = haber;
                }
                egitim = new haberler(tumhaberler.haberlistesi.GetRange(0, tumhaberler.haberlistesi.Count * 3 / 4)); // haberlerin yüzde 75lik bir kısmı egitim verisi olarak kullanılacak
                egitim.FrekansAyarı(50); // min frekans 50 olarak belirlendi
                test = new haberler(tumhaberler.haberlistesi.GetRange(tumhaberler.haberlistesi.Count * 3 / 4, tumhaberler.haberlistesi.Count - tumhaberler.haberlistesi.Count * 3 / 4)); // test verisi haberlerin yüzde yirmi beşi
                ortalamaBul(); // ortalama hesabı
                VaryansHesabı(); // varyans hesabı
            }
            public void ortalamaBul() // ortalama hesabı
            {
                for (int i = 0; i < egitim.haberlistesi.Count; i++) // tum haberler dolaşılır
                {
                    if (!ortalama.ContainsKey(egitim.haberlistesi[i].kategori))
                        ortalama.Add(egitim.haberlistesi[i].kategori, new Dictionary<string, double>()); // sözlükte eger katagori yoksa eklenir
                    var gramlist = egitim.haberlistesi[i].gramlar.ToList(); // haberin gramları dolaşılır
                    for (int j = 0; j < gramlist.Count; j++)
                    {
                        if (!ortalama[egitim.haberlistesi[i].kategori].ContainsKey(gramlist[j].Key))
                            ortalama[egitim.haberlistesi[i].kategori].Add(gramlist[j].Key, Convert.ToDouble(gramlist[j].Value)); // o gram yoksa eklenir
                        else
                            ortalama[egitim.haberlistesi[i].kategori][gramlist[j].Key] += Convert.ToDouble(gramlist[j].Value); // varsa degerleri toplanır
                    }
                }
                var list = ortalama.ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    var gramlar = list[i].Value.ToList();
                    for (int j = 0; j < gramlar.Count; j++)
                    {
                        ortalama[list[i].Key][gramlar[j].Key] = gramlar[j].Value / egitim.KategoriSayilari[list[i].Key] * 1.0; // sorasında gramın toplam sayısı bulundugu kategorinin toplam haber sayısına bölünür.
                    }
                }
            }
            public void VaryansHesabı()
            {
                var ortalamalar = ortalama.ToList();
                for (int i = 0; i < ortalamalar.Count; i++)
                {
                    if (!varyans.ContainsKey(ortalamalar[i].Key))
                        varyans.Add(ortalamalar[i].Key, new Dictionary<string, double>()); // sözlükte olmayan kategori eklenir
                    var list = ortalamalar[i].Value.ToList();
                    for (int k = 0; k < list.Count; k++)
                    {
                        double x = 0.0;
                        for (int j = 0; j < egitim.haberlistesi.Count; j++) // gramın varyansı için her haber dolaşılmalı
                        {
                            if (ortalamalar[i].Key == egitim.haberlistesi[j].kategori &&
                                egitim.haberlistesi[j].gramlar.ContainsKey(list[k].Key))
                                x += Math.Pow(egitim.haberlistesi[j].gramlar[list[k].Key] - list[k].Value, 2); // varyans hesabı. (değer - ortalama) üzeri iki

                        }
                        x /= egitim.haberlistesi.Count; // sonrasında haber sayısına bölünüyor.
                        if (!varyans[ortalamalar[i].Key].ContainsKey(list[k].Key))
                            varyans[ortalamalar[i].Key].Add(list[k].Key, x); // gramın varyansı ekleniyor
                    }
                }
            }
            private double Olasilikhesabi(double deger, double varyans, double ortalama)
            {
                return (1 / (Math.Sqrt(2 * Math.PI * varyans))) * Math.Pow(Math.E, -1 * (Math.Pow(deger - ortalama, 2) / (2 * varyans))); // normal dagılım icin
            }
            public void Tahmin() // Tahmin etme kısmı
            {
                var testlist = test.haberlistesi.ToList();
                for (int k = 0; k < testlist.Count; k++) // test icin ayırılan haberler dolaşılıyor
                {
                    List<double> sonuclar = new List<double>(10); // burada daglımın sonucları tutuluyor
                    string[] kategoriler = {"ekonomi", "magazin", "saglik", "siyasi", "spor"}; // kategoriler
                    for (int i = 0; i < 5; i++) // her katagori için ayrı işlem yapılır.
                    {
                        double sonuc = 0;
                        var gramlar = ortalama[kategoriler[i]].ToList(); // ortalamanın gramları
                        for (int j = 0; j < gramlar.Count; j++)
                        {
                            if (testlist[k].gramlar.ContainsKey(gramlar[j].Key))
                            {
                                double x = Math.Log(Olasilikhesabi(testlist[k].gramlar[gramlar[j].Key], ortalama[kategoriler[i]][gramlar[j].Key], varyans[kategoriler[i]][gramlar[j].Key])); //burada sonucun logaritması alındı çünkü diğer türlü sonuc aşırı küçük çıkıyor
                                if (!Double.IsNaN(x) && !Double.IsInfinity(x))
                                    sonuc += x;
                            }
                            else
                            {
                                double x = Math.Log(Olasilikhesabi(0, ortalama[kategoriler[i]][gramlar[j].Key], varyans[kategoriler[i]][gramlar[j].Key])); // aynısı burada da geçerli
                                if (!Double.IsNaN(x) && !Double.IsInfinity(x))
                                    sonuc += x;
                            }
                        }
                        sonuclar.Add(sonuc);
                    }
                    tahminler.Add(new Tuple<string, string>(testlist[k].kategori, kategoriler[sonuclar.IndexOf(sonuclar.Max())])); // en büyük sonuc istedigimiz sonuc. En büyük sonucun indexi ile tahmin edilen kategorinin indexi aynı
                }
                KonfuzyonMatrisleri(); // işlem sonrası konfuzyon matrisi hesabı
                precisionHesapla();// işlem sonrası precision  hesabı
                recallHesapla();// işlem sonrası recall  hesabı
                FmeasureHesapla();// işlem sonrası fmeasure  hesabı
            }

            public Dictionary<string, double[,]> KonfuzyonMatrisleri() // konfuzyon matrisi hesabı
            {
                string[] kategoriler = { "ekonomi", "magazin", "saglik", "siyasi", "spor" }; // her katagorinin konfuzyon matrisi var
                for (int i = 0; i < kategoriler.Length; i++)
                {
                    konfuzyonmatrisleri.Add(kategoriler[i], new double[2, 2] { { 0, 0 }, { 0, 0 } }); // 2x2 lik bir matris
                    for (int j = 0; j < tahminler.Count; j++)
                    {
                        if (tahminler[j].Item1 == kategoriler[i] && tahminler[j].Item2 == kategoriler[i]) // tahmin dogruysa ve kategoriyle aynıysa
                            konfuzyonmatrisleri[kategoriler[i]][0, 0]++; //true positive
                        else if (tahminler[j].Item1 == kategoriler[i] && tahminler[j].Item2 != kategoriler[i]) // tahmin yanlışsa ve kategoriden farklıysa
                            konfuzyonmatrisleri[kategoriler[i]][1, 1]++; // false negative
                        else if (tahminler[j].Item1 != kategoriler[i] && tahminler[j].Item2 == kategoriler[i]) //tahmin yanlışsa ve katagoriyle aynıysa
                            konfuzyonmatrisleri[kategoriler[i]][1, 0]++; //false positive
                        else //tahmin dogruysa ve katagoriden farklıysa
                            konfuzyonmatrisleri[kategoriler[i]][0, 1]++; //true negative 
                    }
                }
                return konfuzyonmatrisleri;
            }
            private void precisionHesapla()
            {
                string[] kategoriler = {"ekonomi", "magazin", "saglik", "siyasi", "spor"};
                for (int i = 0; i < kategoriler.Length; i++)
                {
                    precision.Add(kategoriler[i], konfuzyonmatrisleri[kategoriler[i]][0, 0] / (konfuzyonmatrisleri[kategoriler[i]][0, 0] + konfuzyonmatrisleri[kategoriler[i]][1, 1]));
                }
            }
            private void recallHesapla()
            {
                string[] kategoriler = {"ekonomi", "magazin", "saglik", "siyasi", "spor"};
                for (int i = 0; i < kategoriler.Length; i++)
                {
                    recall.Add(kategoriler[i], konfuzyonmatrisleri[kategoriler[i]][0, 0] / (konfuzyonmatrisleri[kategoriler[i]][0, 0] + konfuzyonmatrisleri[kategoriler[i]][1, 0]));

                }
            }
            private void FmeasureHesapla()
            {
                string[] kategoriler = {"ekonomi", "magazin", "saglik", "siyasi", "spor"};
                for (int i = 0; i < kategoriler.Length; i++)
                {
                    FMeasure.Add(kategoriler[i], (2 * precision[kategoriler[i]] * recall[kategoriler[i]]) / (precision[kategoriler[i]] + recall[kategoriler[i]]));
                }
            }


            public double Precision(string kategori)
            {
                return precision[kategori];
            }
            public double Recall(string kategori)
            {
                return recall[kategori];
            }
            public double Fmeasure(string kategori)
            {
                return FMeasure[kategori];
            }
        }
    }
}
