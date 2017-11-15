/// <summary>
/// YUNity 0.5
/// 21.10.17
/// bu NoxCaos
/// </summary>
/// 

using System.IO;
using Newtonsoft.Json;

namespace Yun.Data {

    /// <summary>
    /// Extend this in order to make a save file for a game
    /// </summary>
    /// 
    [System.Serializable]
    public abstract class GameSave {

        [JsonIgnore]
        public bool IsFirstLaunch { get; private set; }

        [JsonIgnore]
        public string FileName { get; private set; }

#if UNITY_EDITOR
        private static readonly string PATH = UnityEngine.Application.dataPath + "/../EditorSaves/";
#else
        private static readonly string PATH = UnityEngine.Application.persistentDataPath + "/save/";
#endif

        public GameSave(string filename) {
            FileName = filename;
            IsFirstLaunch = !CheckDirectory(filename);
        }

        internal static T Load<T> (GameSave obj) where T:GameSave {
            var data = "";
            using(var reader = new StreamReader(Path.Combine(PATH, obj.FileName))) {
                data = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject(data) as T;
        }

        internal static void Save(GameSave obj) {
            var serObj = JsonConvert.SerializeObject(obj);

            if(!CheckDirectory(obj.FileName)) {
                UnityEngine.Debug.LogWarning("While saving game data file was not found. " +
                    "It probably has been removed manually while game was running");
            }

            using(var writer = new StreamWriter(Path.Combine(PATH, obj.FileName))) {
                writer.Write(serObj);
            }
        }

        internal static bool CheckDirectory(string fileName) {
            if(!Directory.Exists(PATH)) {
                Directory.CreateDirectory(PATH);
                var f = File.Create(Path.Combine(PATH, fileName));
                f.Close();
                return false;
            } else if(!File.Exists(Path.Combine(PATH, fileName))) {
                var f = File.Create(Path.Combine(PATH, fileName));
                f.Close();
                return false;
            }
            return true;
        }

    }
}