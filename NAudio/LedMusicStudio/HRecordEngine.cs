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
    class HRecordEngine : INotifyPropertyChanged, IWaveformPlayer
    {
        private static HRecordEngine instance;
        private WaveIn wi;
        private bool isRecording;
        Queue<float> sampleData;
        #region Constant
        private const int defaultSampleRate = 44100;
        const float sampleMinValue = 0f;
        const float sampleMaxValue = 5f;
        int numSampleData = 441;
        #endregion
        public HRecordEngine()
        {
            sampleData = new Queue<float>();
            for (int i = 0; i < numSampleData * 2; i++)
            {
                sampleData.Enqueue(0);
            }
        }
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
            if (isRecording) return true;

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
            isRecording = true;
            return true;
        }

        public bool stopRecord()
        {
            if (!isRecording) return true;
            try
            {
                wi.StopRecording();
            }
            catch (Exception ex)
            {
                return false;
            }
            isRecording = false;
            return true;
        }
        #endregion
        private void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] shts = new byte[4];
            float scale = sampleMaxValue - sampleMinValue;
            for (int i = 0; i < e.BytesRecorded - 1; i += 100)
            {
                shts[0] = e.Buffer[i];
                shts[1] = e.Buffer[i + 1];
                shts[2] = e.Buffer[i + 2];
                shts[3] = e.Buffer[i + 3];
                sampleData.Dequeue();
                sampleData.Dequeue();
                sampleData.Enqueue(BitConverter.ToInt32(shts, 0) * scale /Int32.MaxValue);
                sampleData.Enqueue(0);
            }
            NotifyPropertyChanged("WaveformData");
        }

        private void wi_RecordingStopped(object sender, StoppedEventArgs e)
        {
            wi.Dispose();
            wi = null;
        }
        #region Implement IWaveformPlayer

        public double ChannelPosition
        {
            get
            {
                return 0;
            }
            set
            {
                
            }
        }

        public double ChannelLength
        {
            get { return 0; }
        }

        public float[] WaveformData
        {
            get { return sampleData.ToArray(); }
        }

        public TimeSpan SelectionBegin
        {
            get
            {
                return TimeSpan.Zero;
            }
            set
            {
                
            }
        }

        public TimeSpan SelectionEnd
        {
            get
            {
                return TimeSpan.Zero;
            }
            set
            {
                
            }
        }

        public bool IsPlaying
        {
            get { return isRecording; }
        }
        #endregion
    }
}
