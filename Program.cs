using System;
using Starcounter;
using System.Collections.Generic;
using System.Linq;

namespace Nara {
    class Program {
        static void Main() {
            User me = Db.SQL<User>("SELECT u FROM User u").First;
            if (me == null) {
                Db.Transact(() => {
                    me = new User() {
                    };
                });
            }

            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            Handle.GET("/Nara", () => {
                return Db.Scope(() => {
                    var json = new UiJson();
                    json.TaskCreatedEvents = Db.SQL<TaskCreatedEvent>("SELECT e FROM TaskCreatedEvent e WHERE e.User = ? ORDER BY e.When ASC", me).ToList();
                    json.User = me;
                    var taskJson = new TaskJson();
                    taskJson.User = me;
                    if (json.TaskCreatedEvents.Count == 0) {
                        json.CreateNewTask();
                    }
                    json.CurrentTaskIndex = json.TaskCreatedEvents.Count - 1;
                    taskJson.Data = json.TaskCreatedEvents[json.CurrentTaskIndex].Task;
                    taskJson.Created.Data = json.TaskCreatedEvents[json.CurrentTaskIndex];
                    json.CurrentTask = taskJson;

                    json.Session = new Session(SessionOptions.PatchVersioning);
                    return json;
                });
            });
        }
    }
}