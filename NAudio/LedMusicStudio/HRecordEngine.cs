using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSoundVisualizationLib;

namespace LedMusicStudio
{
    class HRecordEngine : INotifyPropertyChanged, ISpectrumPlayer, IWaveformPlayer
    {
        private static HRecordEngine instance;
        private WaveIn wi;
        #region Constant
        private const int defaultSampleRate = 44100;
        #endregion

        #region Singleton Pattern
        public static HRecordEngine Instance
        {
            get
            {
                if (instance == null)
                    instance = new HRecordEngine();
                return instance;
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion

        #region Public functions
        public bool startRecord(int deviceIndex = 0)
        {
            try
            {
                wi = new WaveIn();
                wi.DeviceNumber = deviceIndex;
                wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);
                wi.RecordingStopped += wi_RecordingStopped;
                wi.WaveFormat = new WaveFormat(defaultSampleRate, WaveIn.GetCapabilities(deviceIndex).Channels);
                wi.StartRecording();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool stopRecord()
        {
            try
            {
                wi.StopRecording();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        #endregion
        private void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void wi_RecordingStopped(object sender, StoppedEventArgs e)
        {
            wi.Dispose();
            wi = null;
        }
    }
}
