using Starcounter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nara {
    partial class UiJson : Json {
        public User User;
        public Stack Stack;
        public List<Task> TasksInStack;
        public int CurrentTaskIndex;
        public List<Stack> Stacks;
        public int CurrentStackIndex;

        void Handle(Input.CreateTask action) {
            var created = CreateNewTask();
            this.TasksInStack.Add(created);
            this.ShowTask(TasksInStack.Count - 1);
        }

        void Handle(Input.PreviousTask action) {
            if (this.CurrentTaskIndex > 0) {
                this.ShowTask(this.CurrentTaskIndex - 1);
            }
        }

        void Handle(Input.NextTask action) {
            if (this.CurrentTaskIndex < this.TasksInStack.Count - 1) {
                this.ShowTask(this.CurrentTaskIndex + 1);
            }
        }

        void Handle(Input.CreateStack action) {
            var stack = CreateNewStack();
            this.Stacks.Add(stack);
            this.ShowStack(this.Stacks.Count - 1);
        }

        void Handle(Input.PreviousStack action) {
            if (this.CurrentStackIndex > 0) {
                this.CurrentStackIndex--;
                this.ShowStack(this.CurrentStackIndex);
            }
        }

        void Handle(Input.NextStack action) {
            if (this.CurrentStackIndex < this.Stacks.Count - 1) {
                this.CurrentStackIndex++;
                this.ShowStack(this.CurrentStackIndex);
            }
        }

        public void ShowTask(int index) {
            this.CurrentTaskIndex = index;
            this.CurrentTask.Data = this.TasksInStack[index];
        }

        public void ShowStack(int index) {
            this.CurrentStackIndex = index;
            this.Stack = this.Stacks[index];
            this.TasksInStack = Db.SQL<Task>("SELECT t FROM Task t WHERE t.Stack = ?", this.Stack).ToList();

            if (this.TasksInStack.Count == 0) {
                var created = this.CreateNewTask();
                this.TasksInStack.Add(created);
            }
            this.ShowTask(this.TasksInStack.Count - 1);
        }

        public int _PreviousTasksCount {
            get {
                return this.CurrentTaskIndex;
            }
        }

        public int _NextTasksCount {
            get {
                return this.TasksInStack.Count - 1 - this.CurrentTaskIndex;
            }
        }

        public int _TasksCount {
            get {
                return this.TasksInStack.Count;
            }
        }

        public int _CurrentTaskNumber {
            get {
                return this.CurrentTaskIndex + 1;
            }
        }

        public Task CreateNewTask() {
            var task = new Task() {
            };

            var created = new TaskCreatedEvent() {
                Task = task,
                User = this.User,
                When = DateTime.UtcNow
            };

            var stacked = new TaskAddedToStackEvent() {
                Task = task,
                User = this.User,
                When = DateTime.UtcNow,
                Stack = this.Stack
            };

            return task;
        }

        public Stack CreateNewStack() {
            var stack = new Stack() {
            };

            return stack;
        }
    }
}
