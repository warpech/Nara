using Starcounter;

namespace Nara {
    [Database]
    public class TaskEditedEvent : Event {
        public Task Task;
    }
}
