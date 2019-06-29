using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleOyunu
{
    public partial class Form1 : Form
    {
        public struct fotograf
        {
            public String[,] pixel;
        };

        public Form1()
        {
            InitializeComponent();
        }

        Bitmap[,] ResimMatrisi = new Bitmap[4,4];
        Image resim1;
        Image resim2;
        Button ilkbuton;
        int sayac = 0;
        fotograf[] orjinal;
        fotograf[] karismis;
        Button[] butonDizi;
        int buton1x = 0, buton1y = 0, buton2x, buton2y;
        Bitmap tut;
        int gecicidogruadedi = 0;
        int toplamHamle = 0, dogruhamleadedi = 0, deger = 0;
        int butonClickKontrol = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            dosyadanOku();
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            ResimOkuma();
            resimParcala(pictureBox2.Image);
            OrjinalpixelBulma(diziYapma());
            
        }

        private void ResimOkuma()
        {
            pictureBox1.Image = null;
            OpenFileDialog dosya = new OpenFileDialog();
            if (dosya.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(dosya.FileName);
                pictureBox1.Visible = false;
                pictureBox2.Image = ResimBoyutlandirma(pictureBox1.Image, 640, 640);
                pictureBox2.Visible = false;
                btnkaristir.Visible = true;
            }
        }

        private static Image ResimBoyutlandirma(Image ResimBoyutu, int width, int height)
        {
            Bitmap nesne = new Bitmap(width,height);
            Graphics g = Graphics.FromImage((Image)nesne);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(ResimBoyutu, 0, 0, width, height);
            g.Dispose();
            return (Image)nesne;
        }
        
        private void resimParcala(Image Resim)
        {
            int kare = 160;
            int yatay = Resim.Width / kare;
            int dikey = Resim.Height / kare;
            Rectangle cropAlani = new Rectangle(0, 0, kare, kare);
            
            for (int y = 0; y < yatay; y++)
            {
                for (int d = 0; d < dikey; d++)
                {
                    cropAlani.X = y * kare;
                    cropAlani.Y = d * kare;
                    Bitmap parcaResim = parcala(Resim, cropAlani);
                    parcaResim.Save(@"C:\\Users\\yagmur\\Desktop\\yazlab\\puzzleOyunu\\Parcalar\\" + d + "x" + y + ".jpg", ImageFormat.Jpeg);
                    ResimMatrisi[y, d] = parcaResim;
                }
            }
        }

        private static Bitmap parcala(Image resim, Rectangle kare)
        {
            Bitmap resimnesne = new Bitmap(resim);
            Bitmap Resimparcasi = resimnesne.Clone(kare, resimnesne.PixelFormat);

            return Resimparcasi;
        }
        
        private void butonOlustur()
        {
            Button[,] butonMatris = new Button[4, 4];
            butonDizi = new Button[16];
            int index = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Button buton = new Button
                    {
                        Name = j +"-"+ i.ToString()
                    };

                    var butoncontrol = this.Controls.Find(buton.Name, true).FirstOrDefault();
                    if (butoncontrol != null)
                    {
                        buton = (Button)butoncontrol;

                    }
                    else
                    {
                        this.Controls.Add(buton);
                    }

                    buton.Height = 160;
                    buton.Width = 160;
                    buton.Image = ResimMatrisi[i, j];
                    buton.Location = new System.Drawing.Point(900 + i * buton.Width, 117 + j * buton.Height);
                    buton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    buton.Click += new System.EventHandler(this.buton_Click);
                    butonMatris[i, j] = buton;
                    buton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    buton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
                    if (butonClickKontrol == 1)
                        buton.Enabled = false;
                    if(butonClickKontrol == 0)
                    {
                        buton.Enabled = true;
                        buton.Text = "";
                    }
                        

                }
            }

            butonClickKontrol = 0;
            for (int x= 0; x<4; x++)
            {
                for(int y = 0; y<4; y++)
                {
                    butonDizi[index] = butonMatris[y,x];
                    index++;
                }
            }
        }
        
		public void buton_Click(object sender, EventArgs e)
        {
            sayac++;
            Button gecicibuton = new Button();
			
			if (sayac == 1)
            {
                ilkbuton = new Button();
                ilkbuton = sender as Button;
                resim1 = ilkbuton.Image;
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        if (resim1.Equals(ResimMatrisi[x,y]))
                        {
                            buton1x = x;
                            buton1y = y;
                        }
                    }
                }
                tut = ResimMatrisi[buton1x, buton1y];
            }
            
            else if (sayac == 2)
            {
                Button ikincibuton = new Button();
                ikincibuton = sender as Button;

                resim2 = ikincibuton.Image;
                ikincibuton.Image = resim1;
                ilkbuton.Image = resim2;

                for(int x =0; x<4; x++)
                {
                    for(int y = 0; y<4; y++)
                    {
                        if(resim2.Equals(ResimMatrisi[x,y]))
                        {
                            buton2x = x;
                            buton2y = y;
                            
                        }
                    }
                }
                ResimMatrisi[buton1x, buton1y] = ResimMatrisi[buton2x, buton2y];
                ResimMatrisi[buton2x, buton2y] = tut;
                toplamHamle++;
				KarismispixelBulma(diziYapma());
				int kontrol1 = kontrol();
				if(kontrol1 == 1)
				{
                    if (deger == 1)
                        dogruhamleadedi++;
                    else if (deger == 2)
                        dogruhamleadedi = dogruhamleadedi + 2;
                    deger = 0;
                }
                sayac = 0;
            }
        }

        private void btnkaristir_Click(object sender, EventArgs e)
        {
            pictureBox2.Visible = false;
            karistir();
            KarismispixelBulma(diziYapma());
            butonOlustur();
            int kontrol1 = kontrol();
        }

        private void karistir()
        {
            int sütun = 0, satir = 0;
            Image[,] yeniResimMatrisi = new Image[4, 4];
            Bitmap[] resimDizi = new Bitmap[16];
            int index = 0;

            List<String> liste = new List<String>();
            liste.Add("00");
            liste.Add("01");
            liste.Add("02");
            liste.Add("03");
            liste.Add("10");
            liste.Add("11");
            liste.Add("12");
            liste.Add("13");
            liste.Add("20");
            liste.Add("21");
            liste.Add("22");
            liste.Add("23");
            liste.Add("30");
            liste.Add("31");
            liste.Add("32");
            liste.Add("33");
            
            Random rastgele = new Random();

            while (liste.Count != 0)
            {
                satir = rastgele.Next(0, 4);
                sütun = rastgele.Next(0, 4);

                if(liste.Contains(satir.ToString()+ sütun.ToString()) )
                {
                    liste.Remove(satir.ToString() + sütun.ToString());
                    resimDizi[index] = ResimMatrisi[satir, sütun];
                    index++;
                }
            }

            int sayac = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    ResimMatrisi[i, j] = resimDizi[sayac];
                    sayac++;
                }
            }
        }

        private Bitmap[] diziYapma()
        {
            Bitmap[] parcalar = new Bitmap[16];
            int index = 0;
            for(int i=0; i<4; i++)
            {
                for(int j= 0; j<4; j++)
                {
                    parcalar[index] = ResimMatrisi[j, i];
                    index++;
                }
            }
            return parcalar;
        }

        private void OrjinalpixelBulma(Bitmap[] Parca)
        {
            orjinal = new fotograf[16];

            for (int j = 0; j < orjinal.Length; j++)
            {
                orjinal[j].pixel = new string[160,160];
            }

            for (int i=0; i<orjinal.Length; i++)
            {
                for(int y=0; y<160; y++)
                {
                    for(int z=0; z<160; z++)
                    {
                        orjinal[i].pixel[y,z] = Parca[i].GetPixel(y,z).Name; 
                    }
                }
            }
        }

        private void KarismispixelBulma(Bitmap[] Parca)
        {
            karismis = new fotograf[16];

            for (int j = 0; j < karismis.Length; j++)
            {
                karismis[j].pixel = new string[160, 160];
            }

            for (int i = 0; i < karismis.Length; i++)
            {
                for (int y = 0; y < 160; y++)
                {
                    for (int z = 0; z < 160; z++)
                    {
                        karismis[i].pixel[y, z] = Parca[i].GetPixel(y, z).Name;
                    }
                }
            }
        }
        
        private int kontrol()
        {
            int dogruadedi = 0;
            int count =0 ;
            int bayrak = 0;
            for (int i = 0; i < 16; i++)
            {
                for (int y = 0; y < 160; y++)
                {
                    for (int z = 0; z < 160; z++)
                    {
                        if (orjinal[i].pixel[y, z] == karismis[i].pixel[y, z])
                            count++;
                    }
                }
				if (count == (160 * 160))
				{
					butonDizi[i].Text = "DOĞRU";
					btnkaristir.Visible = false;
                    dogruadedi++;
                    bayrak = 1;
                    butonDizi[i].Enabled = false;
                }
				
				count = 0;
            }
            
            if (dogruadedi - gecicidogruadedi == 2)
            {
                deger = 2;
                gecicidogruadedi = dogruadedi;
            }
            else if (dogruadedi -gecicidogruadedi  == 1)
            {
                deger = 1;
                gecicidogruadedi = dogruadedi;
            }
            if (dogruadedi == 16)
            {
                deger = 2;
                MessageBox.Show("TAMAMLANDI");
                skor();
                butonClickKontrol = 1;
                butonOlustur();

            }
			return bayrak;
        }
        
		private void skor()
		{
			double puan = 100 / (double)toplamHamle;
			double skor = (double)dogruhamleadedi * puan;
            dosyayaYaz(skor);
			MessageBox.Show(skor +" ");
		}

        private void dosyayaYaz(double skor)
        {
            string dosya_yolu = @"C:\\Users\\yagmur\\Desktop\\yazlab\\puzzleOyunu\\enyüksekskor.txt";

            FileStream fileStream = new FileStream(dosya_yolu, FileMode.OpenOrCreate, FileAccess.Write);
            
            StreamWriter streamWriter = new StreamWriter(fileStream);

            streamWriter.Close();
            StreamWriter add = File.AppendText(dosya_yolu);
            add.WriteLine(skor);
            add.Flush();
            add.Close();
        }

        private void dosyadanOku()
        {
            string skor;
            List<double> input = new List<double>();
            double buyuk;
            double kucuk;

            string dosya_yolu = @"C:\\Users\\yagmur\\Desktop\\yazlab\\puzzleOyunu\\enyüksekskor.txt";
            FileStream file = new FileStream(dosya_yolu, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(file);

            skor = streamReader.ReadLine();

            while (skor != null)
            {
                input.Add(Convert.ToDouble(skor));
                skor = streamReader.ReadLine();
            }
            buyuk = input[0];
            kucuk = input[0];
            foreach (double sayi in input)
            {
                if (sayi > buyuk)
                {
                    buyuk = sayi;
                }
                if (sayi < kucuk)
                {
                    kucuk = sayi;
                }
            }

            txtSkor.Text = buyuk.ToString();

            streamReader.Close();
            file.Close();
        }

    }
}
