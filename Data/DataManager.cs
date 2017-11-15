/// <summary>
/// YUNity v 0.5
/// 21.10.17
/// by NoxCaos
/// </summary>
/// 

using Yun.Tools;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

namespace Yun.Data {

    /// <summary>
    /// Yun's Data Manager
    /// Create instance in your Singletone game manager
    /// and call any time to save/load data
    /// </summary>
    /// <depencies>
    /// Yun.Tools.Safe
    /// Yun.Tools.ThreadWorker
    /// </depencies>
    public sealed class DataManager {

        public Action<Exception> Error;
        public bool IsFirstLaunch {
            get { return saveData.IsFirstLaunch; }
        }

        private GameSave saveData;
        private Dictionary<string, Config> resources;

        public DataManager(GameSave save) {
            saveData = save;
            Safe.Error += Error;
        }

        public DataManager(GameSave save, params Config[] res) : this(save) {
            resources = new Dictionary<string, Config>();
            if(res != null) {
                foreach(var r in res) {
                    resources.Add(r.Name, r);
                }
            }
        }

        /// <summary>
        /// Destructor to unsubscribe all
        /// </summary>
        ~DataManager() {
            if(Error != null) {
                Delegate[] clientList = Error.GetInvocationList();
                foreach(var d in clientList)
                    Error -= (d as Action<Exception>);
            }

            Safe.Error -= Error;
        }

        /// <summary>
        /// Loads save file
        /// </summary>
        /// <typeparam name="T">
        /// Type of your save file
        /// It has to extend GameSave class
        /// </typeparam>
        public void Load<T>() where T:GameSave {
            Safe.Call(() => LoadUnsafe<T>());
        }

        /// <summary>
        /// Starts the preload of resources 
        /// </summary>
        public void PreloadResources() {
            Safe.Call(() => PreloadResourcesUnsafe());
        }

        /// <summary>
        /// Safe async saving
        /// </summary>
        /// <param name="callback">
        /// All errors will be logged to Unity Debug Log
        /// Subscribe to Error event if you want to handle them
        /// </param>
        public void SaveAsync(Action callback = null) {
            ThreadWorker worker = new ThreadWorker(Save);
            worker.ThreadDone += (() => {
                if(callback != null) callback();
            });

            var tr = new Thread(new ThreadStart(worker.Run));
            tr.Start();
        }

        /// <summary>
        /// Synchronus saving in main thread
        /// </summary>
        /// All errors will be logged to Unity Debug Log
        /// Subscribe to Error event if you want to handle them
        public void Save() {
            Safe.Call(() => GameSave.Save(saveData));
        }

        /// <summary>
        /// Gets a text resource
        /// </summary>
        /// <param name="name">Name of resource</param>
        /// <returns>
        /// Object of deserialized resource if it was loaded
        /// Convert it to type you need by using 'as'
        /// </returns>
        public object GetResource(string name) {
            if(resources.ContainsKey(name)) {
                var res = resources[name].obj;
                if(res != null) {
                    return res;
                } else {
                    Debug.LogError(string.Format("Resource with name '{0}' was not loaded. " +
                        "Please, use PreloadResources()", name));
                }
            } else {
                Debug.LogError(string.Format("Resource with name '{0}' was not found", name));
            }
            return null;
        }

#region PrivateUnsafe
        private void PreloadResourcesUnsafe() {
            foreach(var r in resources) {
                r.Value.Deserialize();
            }
        }

        private void LoadUnsafe<T>() where T:GameSave {
            var loaded = GameSave.Load<T>(saveData);
            if(loaded != null)
                saveData = loaded;
        }
#endregion
    }
}
