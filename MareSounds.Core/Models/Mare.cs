using System.ComponentModel;

namespace MareSounds.Core.Models
{
    public class Mare : INotifyPropertyChanged
    {
        private string? _picture;
        private string? _name;
        private List<VoiceClip>? _voiceClips;

        public int? Id { get; set; }
        public string? Name 
        {
            get => _name;
            set
            {
                if (value.Equals(_name)) return;
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public List<VoiceClip>? VoiceClips 
        { 
            get => _voiceClips;
            set
            {
                if (value.Equals(_voiceClips)) return;
                _voiceClips = value;
                OnPropertyChanged(nameof(VoiceClips));
            }
        }
        public string? Picture 
        {
            get => _picture;
            set
            {
                if (value.Equals(_picture)) return;
                _picture = value;
                OnPropertyChanged(nameof(Picture));
            }                
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
