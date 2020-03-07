using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Clients.WebApp
{
    public class AppState
    {
        public event Action OnChange;

        public bool IsNavOpen {get; set;}
        public bool IsNavMinified { get; set; }
        public string LastVisitedUrl {get; private set;} = string.Empty;

        public void SaveLastVisitedUri(string uri)
        {
            LastVisitedUrl = uri;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
