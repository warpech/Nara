using Starcounter;

namespace Nara {
    [Database]
    public class Task {
        public string Description {
            get {
                return this.LastEdit == null ? null : this.LastEdit.Description;
            }
        }

        public TaskEditedEvent LastEdit {
            get {
                return Db.SQL<TaskEditedEvent>("SELECT e FROM TaskEditedEvent e WHERE e.Task = ? ORDER BY e.When DESC FETCH ?", this, 1).First;
            }
        }
    }
}
