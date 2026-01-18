using MareSounds.Core.Interfaces.Services;
using MareSounds.Core.Models;
using MareSounds.UI.Models;
using MareSounds.UI.Views;
using Microsoft.Xaml.Behaviors.Core;
using NAudio.Wave;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;

namespace MareSounds.UI.ViewModels
{
    public class MareListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Mare> _buttonData = new ObservableCollection<Mare>();
        private string _test = "Test String Label Data";
        private ICommand _addMareButtonClickCommand;
        private ICommand _playMareSoundCommand;
        private ICommand _deleteSoundCommand;
        private ICommand _addSoundCommand;
        private ICommand _deleteMareCommand;
        private ICommand _updateMareNameCommand;
        private ICommand _updateMarePictureCommand;
        private Mare _selectedMare;
        private List<string> _voiceClips = [];
        private MediaPlayer _player = new();
        private IDBService<Mare> _mareService;
        private IDBService<VoiceClip> _voiceClipsService;
        private SessionContext _sessionContext;

        public MareListViewModel(IDBService<VoiceClip> voiceClipsService, IDBService<Mare> mareService, SessionContext sessionContext)
        {
            _addMareButtonClickCommand = new ActionCommand(AddMare);
            _playMareSoundCommand = new ActionCommand(PlayMareSound);
            _deleteSoundCommand = new ActionCommand(DeleteSound);
            _addSoundCommand = new ActionCommand(AddSound);
            _deleteMareCommand = new ActionCommand(DeleteMare);
            _updateMareNameCommand = new ActionCommand(UpdateMareName);
            _updateMarePictureCommand = new ActionCommand(UpdateMarePicture);

            _voiceClipsService = voiceClipsService;
            _mareService = mareService;
            _sessionContext = sessionContext;

            _sessionContext.MareUpdated += OnMareUpdate;
        }

        #region Properties
        public event PropertyChangedEventHandler? PropertyChanged;

        public Mare SelectedMare
        {
            get { return _selectedMare; }
            set 
            { 
                _selectedMare = value;

                if (value is not null)
                {
                    VoiceClips = value.VoiceClips?.Select(_ => _?.Location).ToList() ?? [];
                }
                else
                {
                    VoiceClips = [];
                }

                if (PropertyChanged is not null)
                {
                    PropertyChanged.Invoke(this, new(nameof(SelectedMare)));
                }
            }
        }

        public List<string>? VoiceClips
        {
            get
            {
                return _voiceClips;
            }
            set
            {
                _voiceClips = value;

                if (PropertyChanged is not null)
                {
                    PropertyChanged.Invoke(this, new(nameof(VoiceClips)));
                }
            }
        }
        #endregion Properties

        #region Commands
        public ICommand PlayMareSoundCommand
        {
            get => _playMareSoundCommand;
        }

        public ICommand AddMareButtonClick
        {
            get { return _addMareButtonClickCommand; }
        }

        public ICommand DeleteSoundCommand
        {
            get => _deleteSoundCommand;
        }

        public ICommand AddSoundCommand
        {
            get => _addSoundCommand;
        }

        public ICommand DeleteMareCommand
        {
            get => _deleteMareCommand;
        }

        public ICommand UpdateMareNameCommand
        {
            get => _updateMareNameCommand;
        }

        public ICommand UpdateMarePictureCommand
        {
            get => _updateMarePictureCommand;
        }
        #endregion Commands

        #region Methods
        public ObservableCollection<Mare> MareList
        {
            get { return ConvertToObservableList<Mare>(_mareService.Get()); } 
            set 
            { 
                _buttonData = value;

                if (PropertyChanged is not null)
                {
                    PropertyChanged.Invoke(this, new(nameof(MareList)));
                }
            }
        }

        private void PlayMareSound(object sender)
        {
            if (SelectedMare?.VoiceClips is null || SelectedMare.VoiceClips.Count < 1) return;
            int randIndex = new Random().Next(0, SelectedMare.VoiceClips.Count);

            var waveoutEvent = new WaveOutEvent();
            var audioFile = new AudioFileReader(SelectedMare.VoiceClips[randIndex].Location);

            waveoutEvent.Init(audioFile);
            waveoutEvent.Play();
        }

        private ObservableCollection<T> ConvertToObservableList<T>(List<T> list)
        {
             ObservableCollection<T> result = new();

            foreach (var item in list)
            {
                result.Add(item);
            }

            return result;
        }

        private void AddMare(object sender)
        {
            AddMareWindow addMare = new();
            addMare.ShowDialog();
        }

        private void OnMareUpdate(object? sender, EventArgs e)
        {
            MareList = ConvertToObservableList<Mare>(_mareService.Get());
        }

        private void AddSound(object sender)
        {
            Microsoft.Win32.OpenFileDialog dialog = new();

            dialog.Filter = "MP3 (*.mp3)|*.mp3";
            dialog.DefaultExt = "mp3";
            dialog.AddExtension = true;
            dialog.Multiselect = true;

            dialog.ShowDialog();

            List<string> clips = dialog.FileNames.Where(_ => _.ToLower().Contains(".mp3")).Where(_ => !VoiceClips.Contains(_)).ToList();

            if (clips is not null)
            {
                SelectedMare.VoiceClips.AddRange(clips.Select(_ => new VoiceClip { Location = _ }));

                _mareService.Upsert([SelectedMare]);

                VoiceClips = SelectedMare.VoiceClips.Select(_ => _.Location).ToList();
            }
        }

        private void DeleteSound(object sender)
        {
            VoiceClip? vc = SelectedMare.VoiceClips?.Where(_ => _.Location.Equals(sender)).FirstOrDefault();
            if (vc is not null)
            {
                SelectedMare.VoiceClips?.Remove(vc);

                _voiceClipsService.Delete([vc]);

                VoiceClips = SelectedMare.VoiceClips.Select(_ => _.Location).ToList();
            }
        }

        private void DeleteMare(object sender)
        {
            if (sender is Mare)
            {
                _mareService.Delete([(Mare)sender]);

                MareList = ConvertToObservableList(_mareService.Get());
            }
        }

        private void UpdateMareName(object sender)
        {
            if (sender is string)
            {
                SelectedMare.Name = sender.ToString();

                _mareService.Upsert([SelectedMare]);
            }
        }

        private void UpdateMarePicture(object sender)
        {
            Microsoft.Win32.OpenFileDialog dialog = new();

            dialog.Filter = "Image (*.png;*.jpg)|*.png;*.jpg";
            dialog.DefaultExt = "png";
            dialog.AddExtension = true;
            dialog.Multiselect = false;

            dialog.ShowDialog();

            SelectedMare.Picture = dialog.FileName;

            _mareService.Upsert([SelectedMare]);

            MareList = ConvertToObservableList(_mareService.Get());
        }
        #endregion Methods
    }
}
