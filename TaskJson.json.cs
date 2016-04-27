using Starcounter;
using System;

namespace Nara {
    partial class TaskJson : Json, IBound<Task> {
        public User User;

        void Handle(Input.Description action) {
            var edited = new TaskEditedEvent() {
                Task = this.Data,
                User = this.User,
                When = DateTime.Now,
                Description = action.Value
            };
            Transaction.Commit();
        }
    }
}
