using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RoseMapObjectData : ScriptableObject
{
    [System.Serializable]
    public class SubObject
    {
        public Mesh mesh;
        public Material material;
        public AnimationClip animation;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public int parent;
        public int colMode;
    }

    public List<SubObject> subObjects = new List<SubObject>();

}
