using System.Collections.Generic;
using UnityEngine;


    [System.Serializable]
    public class MapData
    {
        public List<ObjectData> objects = new List<ObjectData>();
        public int selectedTextureIndex; 
    }

    [System.Serializable]
    public class ObjectData
    {
        public string prefabName;
        public Vector3 position;
        public Quaternion rotation;
    }

   


