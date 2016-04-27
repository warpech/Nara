using Starcounter;
using System;

namespace Nara {
    [Database]
    public class Event {
        public User User;
        public DateTime When;
        public string Description;
    }
}
