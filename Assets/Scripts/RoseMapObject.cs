using UnityEngine;
using System.Collections;

public class RoseMapObject : MonoBehaviour
{
    public RoseMapObjectData data;

    private static void DestroyChildren(GameObject go, GameObject butIgnore = null)
    {
        while (go.transform.childCount > 0)
        {
            DestroyChildren(go.transform.GetChild(0).gameObject, butIgnore);
        }
        if (go != butIgnore)
        {
            DestroyImmediate(go);
        }
    }

    public void UpdateModels()
    {
        DestroyChildren(gameObject, gameObject);

        if (!data)
            return;

        var subObjs = data.subObjects;
        var lkp = new GameObject[subObjs.Count];
        for (int i = 0; i < subObjs.Count; ++i)
        {
            var subObj = subObjs[i];

            var go = new GameObject();
            var mf = go.AddComponent<MeshFilter>();
            mf.mesh = subObj.mesh;

            var mr = go.AddComponent<MeshRenderer>();
            mr.material = subObj.material;

            if (i == 0)
            {
                go.transform.parent = transform;
            }
            else
            {
                go.transform.parent = lkp[subObj.parent - 1].transform;
            }

            go.transform.localPosition = subObj.position;
            go.transform.localRotation = subObj.rotation;
            go.transform.localScale = subObj.scale;
            go.hideFlags = HideFlags.NotEditable;
            go.name = "Mesh " + (i + 1).ToString();

            if (subObj.colMode == 1)
            {
                //go.AddComponent<MeshCollider>();
            }

            if (subObj.animation != null)
            {
                var an = go.AddComponent<Animation>();
                an.clip = subObj.animation;
            }

            lkp[i] = go;
        }
    }
}
