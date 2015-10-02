using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RoseNpcData : ScriptableObject
{
    public RoseSkeletonData skeleton;
    public List<RoseCharPartData> parts = new List<RoseCharPartData>();

}
