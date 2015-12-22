using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace LedMusicStudio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            NAudioEngine soundEngine = NAudioEngine.Instance;
            soundEngine.PropertyChanged += NAudioEngine_PropertyChanged;

            spectrumAnalyzer.RegisterSoundPlayer(soundEngine);
            HRecordEngine recordEngine = HRecordEngine.Instance;
            waveformTimeline.RegisterSoundPlayer(recordEngine);
        }

        private void NAudioEngine_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NAudioEngine engine = NAudioEngine.Instance;
            switch (e.PropertyName)
            {
                case "FileTag":
                    if (engine.FileTag != null)
                    {
                        TagLib.Tag tag = engine.FileTag.Tag;
                        if (tag.Pictures.Length > 0)
                        {
                            using (MemoryStream albumArtworkMemStream = new MemoryStream(tag.Pictures[0].Data.Data))
                            {
                                try
                                {
                                    BitmapImage albumImage = new BitmapImage();
                                    albumImage.BeginInit();
                                    albumImage.CacheOption = BitmapCacheOption.OnLoad;
                                    albumImage.StreamSource = albumArtworkMemStream;
                                    albumImage.EndInit();
                                    //albumArtPanel.AlbumArtImage = albumImage;
                                }
                                catch (NotSupportedException)
                                {
                                    //albumArtPanel.AlbumArtImage = null;
                                    // System.NotSupportedException:
                                    // No imaging component suitable to complete this operation was found.
                                }
                                albumArtworkMemStream.Close();
                            }
                        }
                        else
                        {
                            //albumArtPanel.AlbumArtImage = null;
                        }
                    }
                    else
                    {
                        //albumArtPanel.AlbumArtImage = null;
                    }
                    break;
                case "ChannelPosition":
                    clockDisplay.Time = TimeSpan.FromSeconds(engine.ChannelPosition);
                    break;
                default:
                    // Do Nothing
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (NAudioEngine.Instance.CanPlay)
                NAudioEngine.Instance.Play();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (NAudioEngine.Instance.CanPause)
                NAudioEngine.Instance.Pause();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            if (NAudioEngine.Instance.CanStop)
                NAudioEngine.Instance.Stop();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            NAudioEngine.Instance.OpenFile("E:\\ThoiDoiBeat-VA_4hheq_hq.mp3");
        }

        bool isRecording = false;
        private void recordButton_Click(object sender, RoutedEventArgs e)
        {

            if (isRecording)
            {
                recordButton.Content = "Start Record";
                isRecording = false;
                HRecordEngine.Instance.stopRecord();
            }
            else
            {
                recordButton.Content = "Stop Record";
                isRecording = true;
                HRecordEngine.Instance.startRecord();

            }
        }
        /*
        #region recording

        WaveIn wi;
        Polyline pl;
        double canH = 0;
        double canW = 0;
        double plH = 0;
        double plW = 0;
        //List<byte> totalbytes;
        //Queue<Point> displaypts;
        Queue<Int32> displaysht;
        long count = 0;
        int numtodisplay = 2205;
        bool isRecording = false;

        private void recordButton_Click(object sender, RoutedEventArgs e)
        {
            if (isRecording)
            {
                recordButton.Content = "Start Record";
                isRecording = false;
                try
                {
                    wi.StopRecording();
                }
                catch (Exception ex)
                {

                }
                count = 0;
            }
            else
            {
                recordButton.Content = "Stop Record";
                isRecording = true;


                wi = new WaveIn();
                wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);
                wi.RecordingStopped += wi_RecordingStopped;
                wi.WaveFormat = new WaveFormat(44100, 32, 2);

                canH = mycanvas.ActualHeight;
                canW = mycanvas.ActualWidth;

                pl = new Polyline();
                pl.Stroke = Brushes.Blue;
                pl.Name = "waveform";
                pl.StrokeThickness = 1;
                pl.MaxHeight = canH - 4;
                pl.MaxWidth = canW - 4;

                plH = pl.MaxHeight;
                plW = pl.MaxWidth;

                //displaypts = new Queue<Point>();
                //totalbytes = new List<byte>();
                displaysht = new Queue<Int32>();

                wi.StartRecording();
            }
        }

        void wi_RecordingStopped(object sender, EventArgs e)
        {
            wi.Dispose();
            wi = null;
        }


        void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            //totalbytes.AddRange(e.Buffer);

            byte[] shts = new byte[4];

            for (int i = 0; i < e.BytesRecorded - 1; i += 1000)
            {
                shts[0] = e.Buffer[i];
                shts[1] = e.Buffer[i + 1];
                shts[2] = e.Buffer[i + 2];
                shts[3] = e.Buffer[i + 3];
                if (count < numtodisplay)
                {
                    displaysht.Enqueue(BitConverter.ToInt32(shts, 0));
                    ++count;
                }
                else
                {
                    displaysht.Dequeue();
                    displaysht.Enqueue(BitConverter.ToInt32(shts, 0));
                }
            }
            this.mycanvas.Children.Clear();
            pl.Points.Clear();
            Int32[] shts2 = displaysht.ToArray();
            for (Int32 x = 0; x < shts2.Length; ++x)
            {
                pl.Points.Add(Normalize(x, shts2[x]));
            }
            
            this.mycanvas.Children.Add(pl);

        }


        Point Normalize(Int32 x, Int32 y)
        {
            Point p = new Point();
            
            p.X = 1.0 * x / numtodisplay * plW;
            p.Y = plH / 2.0 - y / (Int32.MaxValue * 1.0) * (plH / 2.0);
            return p;
        }
        #endregion
         */
    }
}
