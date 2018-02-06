using UnityEngine;
using System.Collections.Generic;
using Yun.Linq;
using Yun.Ity;
using System.IO;

namespace Yun.Static {
    public static class Tools {

        public static void ClearChildren(Transform item) {
            var children = new List<GameObject>();
            foreach(Transform child in item)
                children.Add(child.gameObject);
            children.ForEach(child => Object.Destroy(child));
        }

        public static T GetLink<T>(GameObject target) where T:MonoBehaviour {
            var links = target.GetComponents<Link>();
            return links.First(x => x.GetTargetType() == typeof(T)).GetComponent<T>();
        }

        public static T GetLink<T>(GameObject target, string name) where T : MonoBehaviour {
            var links = target.GetComponents<Link>();
            return links.First(x => x.name == name && x.GetTargetType() == typeof(T)).GetComponent<T>();
        }

        public static T GetRandom<T>(T source) where T : IList<T> {
            return source[Random.Range(0, source.Count)];
        }

        public static T GetRandom<T>(T[] source) {
            return source[Random.Range(0, source.Length)];
        }
    }
}
