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

            //migration: add stacks

            Stack stack = Db.SQL<Stack>("SELECT s FROM Stack s").First;
            if (stack == null) {
                Db.Transact(() => {
                    stack = new Stack() {
                    };
                });
            }

            var tasks = Db.SQL<Task>("SELECT t FROM Task t");
            foreach (var task in tasks) {
                if (task.Stacked == null) {
                    Db.Transact(() => {
                        new TaskAddedToStackEvent() {
                            Task = task,
                            User = me,
                            When = DateTime.UtcNow,
                            Stack = stack
                        };
                    });
                }
            }

            //app

            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            Handle.GET("/Nara", () => {
                return Db.Scope(() => {
                    var json = new UiJson() {
                        User = me
                    };

                    json.CurrentTask = new TaskJson() {
                        User = me
                    };

                    json.Stacks = Db.SQL<Stack>("SELECT s FROM Stack s").ToList();
                    json.ShowStack(0);

                    json.Session = new Session(SessionOptions.PatchVersioning);
                    return json;
                });
            });
        }
    }
}