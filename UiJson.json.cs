using Starcounter;
using System;
using System.Collections.Generic;

namespace Nara {
    partial class UiJson : Json {
        public User User;
        public List<TaskCreatedEvent> TaskCreatedEvents;
        public int CurrentTaskIndex;

        void Handle(Input.CreateTask action) {
            var created = CreateNewTask();
            this.TaskCreatedEvents.Add(created);
            this.CurrentTaskIndex = TaskCreatedEvents.Count - 1;
            this.ShowTask(this.CurrentTaskIndex);
        }

        void Handle(Input.PreviousTask action) {
            if (this.CurrentTaskIndex > 0) {
                this.CurrentTaskIndex--;
                this.ShowTask(this.CurrentTaskIndex);
            }
        }

        void Handle(Input.NextTask action) {
            if (this.CurrentTaskIndex < this.TaskCreatedEvents.Count - 1) {
                this.CurrentTaskIndex++;
                this.ShowTask(this.CurrentTaskIndex);
            }
        }

        private void ShowTask(int index) {
            this.CurrentTask.Data = this.TaskCreatedEvents[index].Task;
            ((TaskJson)this.CurrentTask).Created.Data = this.TaskCreatedEvents[index];
        }

        public TaskCreatedEvent CreateNewTask() {
            var task = new Task() {
            };

            var created = new TaskCreatedEvent() {
                Task = task,
                User = this.User,
                When = DateTime.Now
            };

            return created;
        }
    }
}
