using Starcounter;

namespace Nara {
    [Database]
    public class Task {
        public string Description {
            get {
                return this.LastEdit == null ? null : this.LastEdit.Description;
            }
        }

        public Stack Stack {
            get {
                return Db.SQL<Stack>("SELECT e.Stack FROM Nara.TaskAddedToStackEvent e WHERE e.Task = ? ORDER BY e.ObjectNo DESC FETCH ?", this, 1).First;
            }
        }

        public TaskCreatedEvent Created {
            get {
                return Db.SQL<TaskCreatedEvent>("SELECT e FROM Nara.TaskCreatedEvent e WHERE e.Task = ? FETCH ?", this, 1).First;
            }
        }

        public TaskEditedEvent LastEdit {
            get {
                return Db.SQL<TaskEditedEvent>("SELECT e FROM Nara.TaskEditedEvent e WHERE e.Task = ? ORDER BY e.ObjectNo DESC FETCH ?", this, 1).First;
            }
        }
    }
}
