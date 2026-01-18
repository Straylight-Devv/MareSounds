using MareSounds.Core.Interfaces.Services;
using MareSounds.Core.Models;
using MareSounds.UI.Models;
using Microsoft.Xaml.Behaviors.Core;
using System.ComponentModel;
using System.Windows.Input;

namespace MareSounds.UI.ViewModels
{
    class AddMareViewModel : INotifyPropertyChanged
    {
        private Mare _mare = new();
        private bool _canSaveMare;
        private ICommand _saveMareCommand;
        private ICommand _selectVoiceClipsCommand;
        private ICommand _selectMarePictureCommand;
        private List<string> _voiceClips = [];
        private IDBService<Mare> _mareService;
        private IDBService<VoiceClip> _voiceClipsService;
        private SessionContext _sessionContext;

        public AddMareViewModel(IDBService<VoiceClip> voiceClipsService, IDBService<Mare> mareService, SessionContext sessionContext)
        {
            _saveMareCommand = new ActionCommand(SaveMare);
            _selectVoiceClipsCommand = new ActionCommand(SelectVoiceClips);
            _selectMarePictureCommand = new ActionCommand(SelectMarePicture);
            _voiceClipsService = voiceClipsService;
            _mareService = mareService;
            _sessionContext = sessionContext;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        #region Properties
        public Mare Mare
        {
            get => _mare;
            set
            {
                if (value != _mare)
                {
                    _mare = value;

                    if (_mare?.Name?.Length < 1)
                    {
                        CanSaveMare = false;
                    }
                    else
                    {
                        CanSaveMare = true;
                    }

                    if (this.PropertyChanged is not null)
                    {
                        PropertyChanged.Invoke(this, new(nameof(Mare)));
                    }
                }
            }
        }

        public string? MareName
        {
            get => _mare.Name;
            set
            {
                if (value is not null && !value.Equals(Mare.Name))
                {
                    _mare.Name = value;

                    if (_mare?.Name?.Length < 1)
                    {
                        CanSaveMare = false;
                    }
                    else
                    {
                        CanSaveMare = true;
                    }
                }

                if (this.PropertyChanged is not null)
                {
                    PropertyChanged.Invoke(this, new(nameof(MareName)));
                }
            }
        }

        public string? MarePicture
        {
            get => _mare.Picture;
            set
            {
                if (value is not null && !value.Equals(Mare.Picture))
                {
                    _mare.Picture = value;
                }

                if (this.PropertyChanged is not null)
                {
                    PropertyChanged.Invoke(this, new(nameof(MarePicture)));
                }
            }
        }

        public List<string> VoiceClips
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

        public bool CanSaveMare
        {
            get => _canSaveMare;
            set
            {
                if (value != _canSaveMare)
                {
                    _canSaveMare = value;

                    if (this.PropertyChanged is not null)
                    {
                        PropertyChanged.Invoke(this, new(nameof(CanSaveMare)));
                    }
                }
            }
        }
        #endregion Properties

        #region Commands
        public ICommand SaveMareCommand
        {
            get { return _saveMareCommand; }
        }

        public ICommand SelectVoiceClipsCommand
        {
            get => _selectVoiceClipsCommand;
        }

        public ICommand SelectMarePictureCommand
        {
            get => _selectMarePictureCommand;
        }
        #endregion Commands

        #region Methods
        private void SaveMare(object sender)
        {
            _mareService.Upsert([Mare]);

            _sessionContext.CallMareUpdatedEvent(this, new());

            Mare = new();
        }

        private void SelectVoiceClips(object sender)
        {
            Microsoft.Win32.OpenFileDialog dialog = new();

            dialog.Filter = "MP3 (*.mp3)|*.mp3";
            dialog.DefaultExt = "mp3";
            dialog.AddExtension = true;
            dialog.Multiselect = true;

            dialog.ShowDialog();

            VoiceClips = dialog.FileNames.Where(_ => _.ToLower().Contains(".mp3")).ToList();

            if (VoiceClips is not null)
            {
                Mare.VoiceClips = VoiceClips.Select(_ => new VoiceClip { Location = _ }).ToList();
            }
        }

        private void SelectMarePicture(object sender)
        {
            Microsoft.Win32.OpenFileDialog dialog = new();

            dialog.Filter = "Image (*.png;*.jpg)|*.png;*.jpg";
            dialog.DefaultExt = "png";
            dialog.AddExtension = true;
            dialog.Multiselect = false;

            dialog.ShowDialog();

            MarePicture = dialog.FileName;
        }
        #endregion Methods
    }
}
