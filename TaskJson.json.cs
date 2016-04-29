using Starcounter;
using System;

namespace Nara {
    partial class TaskJson : Json, IBound<Task> {
        public User User;
        public TaskCreatedEvent TaskCreatedEvent;

        void Handle(Input.Description action) {
            var edited = new TaskEditedEvent() {
                Task = this.Data,
                User = this.User,
                When = DateTime.UtcNow,
                Description = action.Value
            };
            Transaction.Commit();
        }

        public string _CreatedAt {
            get {
                return this.TaskCreatedEvent.When.ToString("r");
            }
        }

        public string _LastEditAt {
            get {
                if (this.Data.LastEdit != null) {
                    return this.Data.LastEdit.When.ToString("r");
                }
                return "";
            }
        }
    }
}
