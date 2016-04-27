using Starcounter;

namespace Nara {
    [Database]
    public class TaskResolvedEvent : Event {
        public Task Task;
    }
}
