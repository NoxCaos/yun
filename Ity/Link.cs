using UnityEngine;
using System.Collections;

namespace Yun.Ity {
    public sealed class Link : MonoBehaviour {

        [SerializeField] private MonoBehaviour target;

        public System.Type GetTargetType() {
            return target.GetType();
        }

    }
}
