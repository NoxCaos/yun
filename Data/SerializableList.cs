using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Yun.Data {
	[Serializable]
	public class SerializableList<T>:List<T>,ISerializable {
		#region ISerializable implementation

		public void GetObjectData (SerializationInfo info, StreamingContext context) {
			info.AddValue("1", this.ToArray());
		}

		#endregion

		public SerializableList(SerializationInfo info, StreamingContext context) {
			AddRange((T[])info.GetValue("1",typeof(T[])));
		}

		public SerializableList () { }

        public SerializableList(T[] array) {
            foreach(var a in array)
                Add(a);
        }
    }
}