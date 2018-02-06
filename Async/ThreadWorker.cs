using System;
using System.Threading;

namespace Yun.Async {

    /// <summary>
    /// Starts Thread with callback
    /// </summary>
    public class Worker {

        public Action ThreadDone;

        private Action _run;

        public Worker(Action run) {
            _run = run;
        }

        public void Run() {
            _run();

            if(ThreadDone != null)
                ThreadDone();
            ThreadDone = null;
        }

    }

    public class Task {
        private static MainTreadHandler _main;

        private ThreadStart tstart;
        private Thread runningThread;

        public Task(Action task) {
            if(!_main) {
                _main = new UnityEngine.GameObject().AddComponent<MainTreadHandler>();
                _main.gameObject.name = "YunAsyncHandler";
                UnityEngine.Object.DontDestroyOnLoad(_main.gameObject);
            }

            tstart = new ThreadStart(task);
            runningThread = new Thread(tstart);
        }

        public void RunAsync(object arg, Action oncomplete) {
            var exec = new Exec {
                OnComplete = oncomplete
            };
            _main.RunTask(exec);
            tstart += () => _main.CompleteTask(exec);
            runningThread.Start(arg);
        }
    }

    public class Exec {
        public Guid Id { get; private set; }
        public bool IsRunning;
        public Action OnComplete;

        public Exec() {
            Id = new Guid();
        }
    }

}
