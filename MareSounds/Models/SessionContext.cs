using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MareSounds.UI.Models
{
    public class SessionContext
    {
        public void CallMareUpdatedEvent(object? sender, EventArgs e)
        {
            MareUpdated?.Invoke(sender, e);
        }

        public event EventHandler? MareUpdated;
    }
}
