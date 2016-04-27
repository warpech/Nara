using Starcounter;

namespace Nara {
    [Database]
    public class TaskCreatedEvent : Event {
        public Task Task;
    }
}
