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
            waveformTimeline.RegisterSoundPlayer(soundEngine);
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
    }
}
