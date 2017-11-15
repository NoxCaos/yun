using UnityEngine;
using System.Collections;
using System;

namespace Yun.Tools {
    public class ThreadWorker {

        public Action ThreadDone;

        private Action _run;

        public ThreadWorker(Action run) {
            _run = run;
        }

        public void Run() {
            _run();

            if(ThreadDone != null)
                ThreadDone();
        }

    }
}
