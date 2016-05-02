using Starcounter;

namespace Nara {
    [Database]
    public class TaskAddedToStackEvent : Event {
        public Task Task;
        public Stack Stack;
    }
}
