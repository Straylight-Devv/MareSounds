using SQLitePCL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MareSounds.Core.Models
{
    public class VoiceClip : INotifyPropertyChanged
    {
        private string? _location;

        public int? Id { get; set; }
        public string? Location
        { 
            get => _location;
            set
            {
                if (value.Equals(_location)) return;
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }
        public int? MareId { get; set; }
        public Mare Mare { get; set; } = null!;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
