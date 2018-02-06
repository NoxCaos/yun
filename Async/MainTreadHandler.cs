using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Yun.Linq;

namespace Yun.Async {
    internal class MainTreadHandler : MonoBehaviour {

        private readonly List<Exec> _execution = new List<Exec>();

        private int _count;

        void Update() {
            if(_count <= 0)
                return;

            lock(_execution) {
                var c = _execution.FirstOrDefault(t => !t.IsRunning);
                if(c != null) {
                    if(c.OnComplete != null)
                        c.OnComplete();
                    _execution.Remove(c);
                    _count--;
                }
            }
        }

        private void OnDestroy() {
            
        }

        internal void RunTask(Exec task) {
            lock(_execution) {
                _execution.Add(task);
                task.IsRunning = true;
                _count++;
            }
        }

        internal void CompleteTask(Exec task) {
            task.IsRunning = false;
        }
    }
}