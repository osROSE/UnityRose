using UnityEngine;
using System.Collections;

public class RoseNpc : RoseCharacter
{
    public RoseNpcData data;

    public void UpdateModels()
    {
        parts.Clear();
        for (var i = 0; i < data.parts.Count; ++i)
        {
            parts.Add(data.parts[i]);
        }

        skeleton = data.skeleton;

        UpdateData();
    }
}
