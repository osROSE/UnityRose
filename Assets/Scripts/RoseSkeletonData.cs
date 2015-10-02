using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RoseSkeletonData : ScriptableObject
{
    [System.Serializable]
    public class Bone
    {
        public string name;
        public Vector3 translation;
        public Quaternion rotation;
        public int parent;
    }

    public List<Bone> bones = new List<Bone>();
    public List<Bone> dummies = new List<Bone>();

}
