using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.ComponentModel;


namespace poker_client
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool Answering = false;
        public string labeltext;
        private BackgroundWorker worker = new BackgroundWorker();
        private BackgroundWorker worker2 = new BackgroundWorker();
        private TcpClient klient = null;
        delegate void SetTextCallBack(string tekst);
        delegate void ChangeMinSliderCallBack(int min );
        delegate void ChangeSliderCallBack(double value);
        delegate void IPReadCallBack();
        delegate void PortReadCallBack();
        delegate void AnswerCallBack();
        delegate void ImgCallBack(Image image, string adress);
        delegate void ReadLabelCallBack(Label label);
        delegate void EnableButtonCallBack(Button button);
        delegate void ChangeLabelCallBack(Label label, string text);
        private bool serwer = false;
        private BinaryReader czytanie = null;
        private BinaryWriter pisanie = null;
        string ip = "";
        string port = "";
        public int yourpoosition = 0;
        public MainWindow()
        {
            InitializeComponent();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker2.DoWork += new DoWorkEventHandler(worker2_DoWork);
        }
        int Cache = 10000;


        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Rate.Text = (Math.Ceiling(slider.Value * (double)(Cache) / 100000.00)).ToString();
        }

        private void Rate_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                slider.Value = (Double)(Math.Ceiling(double.Parse(Rate.Text) / (double)(Cache) * 100000.00));
            }
            catch { }
        }

        private void Fold_Click(object sender, RoutedEventArgs e)
        {
            if (Answering == true)
            {
                pisanie.Write("-1");
                Fold.IsEnabled = false;
                Check.IsEnabled = false;
                Call.IsEnabled = false;
                Raise.IsEnabled = false;
                ChangeMinSlider(1);
            }
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            if (Answering == true)
            {
                pisanie.Write("0");
                Fold.IsEnabled = false;
                Check.IsEnabled = false;
                Call.IsEnabled = false;
                Raise.IsEnabled = false;
            }
        }

        private void Call_Click(object sender, RoutedEventArgs e)
        {
            if (Answering == true)
            {
                pisanie.Write("0");
                Fold.IsEnabled = false;
                Check.IsEnabled = false;
                Call.IsEnabled = false;
                Raise.IsEnabled = false;
                ChangeMinSlider(1);
            }
        }

        private void Raise_Click(object sender, RoutedEventArgs e)
        {
            if (Answering == true)
            {
                if (Int32.Parse(Rate.Text) == Cache)
                {
                    pisanie.Write("50000");
                }
                else
                {
                    pisanie.Write(Rate.Text);
                }
                Fold.IsEnabled = false;
                Check.IsEnabled = false;
                Call.IsEnabled = false;
                Raise.IsEnabled = false;
                ChangeMinSlider(1);

            }

        }

        private void Conect_Click(object sender, RoutedEventArgs e)
        {

            IP.IsEnabled = false;
            Port.IsEnabled = false;
            if (serwer == true) { }
            else
            {
                serwer = true;
                worker.RunWorkerAsync();
            }
        }

        private void ChangeLabel(Label label,string text)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new ChangeLabelCallBack(ChangeLabel),label, text);
                return;
            }
            label.Content = text;
        }
        private void EnableButton(Button button)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new EnableButtonCallBack(EnableButton), button);
                return;
            }
            button.IsEnabled = !button.IsEnabled;
        }
        private void ReadLabel(Label label)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new ReadLabelCallBack(ReadLabel), label);
                return;
            }
            labeltext = label.Content.ToString() ;
        }
        private void ChangeMinSlider(int min)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new ChangeMinSliderCallBack(ChangeMinSlider), min);
                return;
            }
            slider.Minimum = (double)min;
        }
        private void ChangeSlider(double value)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new ChangeSliderCallBack(ChangeSlider), value);
                return;
            }
            slider.Value = value;
        }
        private void Answer()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new AnswerCallBack(Answer));
                return;
            }
            Answering = !Answering;
        }
        private void IMG(Image image, string adress) {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new ImgCallBack(IMG), image,adress);
                return;
            }
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(adress, UriKind.Relative);
            img.EndInit();

            image.Source = img;
        }
        private void IPRead()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new IPReadCallBack(IPRead));
                return;
            }
            ip = IP.Text;
        }
        private void PortRead()
        {
            if (!Dispatcher.CheckAccess())
            {

                Dispatcher.Invoke(new PortReadCallBack(PortRead));
                return;
            }

            port = Port.Text;
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {

            int portint = 0;
            IPAddress ipv4 = null;
            try
            {
                IPRead();
                ipv4 = IPAddress.Parse(ip);

            }
            catch
            {

                MessageBox.Show("niewłaściwy adres ip");
                serwer = false;
                return;

            }
            try
            {
                PortRead();

                portint = Int32.Parse(port);

            }
            catch
            {

                MessageBox.Show("niewłaściwy numer portu");
                serwer = false;
                return;

            }

            try
            {
                ChangeLabel( Lebel ,"Oczekiwanie na połączenie");
                klient = new TcpClient(ip, portint);
                NetworkStream ns = klient.GetStream();
                czytanie = new BinaryReader(ns);
                pisanie = new BinaryWriter(ns);

                ChangeLabel(Lebel, "Nawiązano połączenie");
                worker2.RunWorkerAsync();

            }
            catch
            {

                ChangeLabel(Lebel, "Połączenie zostało przerwane");
                serwer = false;
            }
        }
        private void worker2_DoWork(object sender, DoWorkEventArgs e)
        {
            string wiadomosc;
            try
            {
                while ((wiadomosc = czytanie.ReadString()) != "exit")
                {
                    
                    string[] words = wiadomosc.Split(null as string[], StringSplitOptions.RemoveEmptyEntries);
                    if (words[0] == "start")
                    {
                        yourpoosition = Int32.Parse(words[1]);
                        MessageBox.Show(String.Concat("Grasz jako player numer", words[1]));
                    }
                    if (words[0] == "card" ) {
                        if (words[1] == "1") {
                            string adress = String.Concat("carts/", words[2], ".png");
                            IMG(KartaN1, adress);
                            adress = String.Concat("carts/", words[3], ".png");
                            IMG(KartaN2, adress);
                        }
                        if (words[1] == "2")
                        {
                            string adress = String.Concat("carts/", words[2], ".png");
                            IMG(KartaE1, adress);
                            adress = String.Concat("carts/", words[3], ".png");
                            IMG(KartaE2, adress);
                        }
                        if (words[1] == "3")
                        {
                            string adress = String.Concat("carts/", words[2], ".png");
                            IMG(KartaS1, adress);
                            adress = String.Concat("carts/", words[3], ".png");
                            IMG(KartaS2, adress);
                        }
                        if (words[1] == "4")
                        {
                            string adress = String.Concat("carts/", words[2], ".png");
                            IMG(KartaW1, adress);
                            adress = String.Concat("carts/", words[3], ".png");
                            IMG(KartaW2, adress);
                        }
                        if (words[1] == "C1")
                        {
                            string adress = String.Concat("carts/", words[2], ".png");
                            IMG(KartaC1, adress);
                            adress = String.Concat("carts/", words[3], ".png");
                            IMG(KartaC2, adress);
                            adress = String.Concat("carts/", words[4], ".png");
                            IMG(KartaC3, adress);
                        }
                        if (words[1] == "C2")
                        {
                            string adress = String.Concat("carts/", words[2], ".png");
                            IMG(KartaC4, adress);
                        }
                        if (words[1] == "C3")
                        {
                            string adress = String.Concat("carts/", words[2], ".png");
                            IMG(KartaC5, adress);
                        }

                    }
                    if (words[0] == "play") {
                        Answer();
                        int Max = Int32.Parse(Nplay.Content.ToString());

                        if (Max< Int32.Parse(Eplay.Content.ToString()))
                        {
                            Max = Int32.Parse(Eplay.Content.ToString());
                        }
                        if (Max < Int32.Parse(Splay.Content.ToString())) 
                        {
                            Max = Int32.Parse(Splay.Content.ToString());
                        }
                        if (Max < Int32.Parse(Wplay.Content.ToString()))
                        {
                            Max = Int32.Parse(Wplay.Content.ToString());
                        }
                        if(yourpoosition ==0)
                        {
                            if (Max == Int32.Parse(Nplay.Content.ToString()))
                            {
                                EnableButton(Fold);
                                EnableButton(Check);
                                EnableButton(Raise);

                            }
                            else
                            {
                                EnableButton(Fold);
                                EnableButton(Call);
                                EnableButton(Raise);
                                ChangeMinSlider(Max - Int32.Parse(Nplay.Content.ToString()));
                            }
                        }
                        if (yourpoosition == 1)
                        {
                            if (Max == Int32.Parse(Eplay.Content.ToString()))
                            {
                                EnableButton(Fold);
                                EnableButton(Check);
                                EnableButton(Raise);

                            }
                            else
                            {
                                EnableButton(Fold);
                                EnableButton(Call);
                                EnableButton(Raise);
                                ChangeMinSlider(Max - Int32.Parse(Eplay.Content.ToString()));
                            }
                        }
                        if (yourpoosition == 2)
                        {
                            if (Max == Int32.Parse(Splay.Content.ToString()))
                            {
                                EnableButton(Fold);
                                EnableButton(Check);
                                EnableButton(Raise);

                            }
                            else
                            {
                                EnableButton(Fold);
                                EnableButton(Call);
                                EnableButton(Raise);
                                ChangeMinSlider(Max - Int32.Parse(Splay.Content.ToString()));
                            }
                        }
                        if (yourpoosition == 3)
                        {
                            if (Max == Int32.Parse(Wplay.Content.ToString()))
                            {
                                EnableButton(Fold);
                                EnableButton(Check);
                                EnableButton(Raise);

                            }
                            else
                            {
                                EnableButton(Fold);
                                EnableButton(Call);
                                EnableButton(Raise);
                                ChangeMinSlider(Max - Int32.Parse(Wplay.Content.ToString()));
                            }
                        }
                    }
                    if (words[0] == "coin")
                    {
                        if (words[1] == yourpoosition.ToString()) { Cache = Int32.Parse(words[3]); }
                        if(words[1] == "0")
                        {
                            ChangeLabel(Nplay, words[2]);
                            ChangeLabel(Ncoins, words[3]);
                        }
                        if (words[1] == "1")
                        {
                            ChangeLabel(Eplay, words[2]);
                            ChangeLabel(Ecoins, words[3]);
                        }
                        if (words[1] == "2")
                        {
                            ChangeLabel(Splay, words[2]);
                            ChangeLabel(Scoins, words[3]);
                        }
                        if (words[1] == "3")
                        {
                            ChangeLabel(Wplay, words[2]);
                            ChangeLabel(Wcoins, words[3]);
                        }
                    }
                    if(words[0] == "koniec")
                    {
                        MessageBox.Show("Rozgrywke wygrał gracz numer" + words[1]);
                    }
                }

                klient.Close();
                ChangeLabel(Lebel, "Połączenie zostało przerwane");
            }
            catch
            {
                serwer = false;
                klient.Close();
                ChangeLabel(Lebel, "Połączenie zostało przerwane\n");
            }
        }
    }
}
