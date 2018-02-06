using UnityEngine;
using System.Collections;

namespace Yun.Streaming {
    public static class Assets {

        public static IEnumerator LoadSprite(string path, System.Action<Sprite> callback) {
            var localFile = new WWW("file://" + path);

            yield return localFile;

            var t = localFile.texture;
            var sprite = Sprite.Create(t as Texture2D, new Rect(0, 0, t.width, t.height), Vector2.zero);

            callback(sprite);
        }

        public static IEnumerator LoadTexture(string path) {

            WWW www = new WWW("file://" + System.IO.Path.Combine(Application.streamingAssetsPath, "IMG_3020.JPG"));

            while(!www.isDone) {

                yield return null;
            }

            if(!string.IsNullOrEmpty(www.error)) {

                Debug.Log(www.error);
                yield break;
            } else {

                //photoSphere.GetComponent<Renderer>().material.mainTexture = www.texture;
            }

            yield return 0;
        }

    }
}
