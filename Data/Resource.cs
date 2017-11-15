/// <summary>
/// YUNity v 0.5
/// 14.11.17
/// by NoxCaos
/// </summary>
/// 

using Newtonsoft.Json;
using UnityEngine;

namespace Yun.Data {

    /// <summary>
    /// Stores basic information about resource file
    /// Used for text assets that store JSON data
    /// required by game
    /// </summary>
    public class Config {

        internal string Name { get; private set; }
        internal string Path { get; private set; }

        internal object obj;

        public Config(string filename, string path) {
            Name = filename;
            Path = path;
        }

        public void Deserialize() {
            var asset = Resources.Load<TextAsset>(System.IO.Path.Combine(Path, Name));
            obj = JsonConvert.DeserializeObject(asset.text);
        }
    }
}
