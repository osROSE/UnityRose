using UnityEngine;
using System.Collections.Generic;

public class RoseCharacter : MonoBehaviour {

    protected RoseSkeletonData skeleton = null;
    protected List<RoseCharPartData> parts = new List<RoseCharPartData>();
    private Transform[] bones = null;
    private Matrix4x4[] bps = null;

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

    public void UpdateData()
    {
        DestroyChildren(gameObject, gameObject);

        UpdateSkeleton();
        UpdateParts();
    }

    private void UpdateSkeleton()
    {
        bones = new Transform[skeleton.bones.Count];
        bps = new Matrix4x4[skeleton.bones.Count];
        var mats = new Matrix4x4[skeleton.bones.Count];

        for (var i = 0; i < skeleton.bones.Count; ++i)
        {
            var bone = skeleton.bones[i];
            var go = new GameObject();
            go.name = "Bone_" + i.ToString();

            if (i == 0)
                go.transform.parent = transform;
            else
                go.transform.parent = bones[bone.parent];

            go.transform.localPosition = bone.translation;
            go.transform.localRotation = bone.rotation;
            go.transform.localScale = Vector3.one;

            bones[i] = go.transform;

            Matrix4x4 myMat = Matrix4x4.TRS(
                    bone.translation,
                    bone.rotation,
                    Vector3.one);

            if (i == 0)
            {
                mats[i] = myMat;
            }
            else
            {
                mats[i] = mats[bone.parent] * myMat;
            }
            bps[i] = mats[i].inverse;
        }

        bones[0].gameObject.AddComponent<BoneDebug>();

        var ani = gameObject.GetComponent<Animation>();
        if (ani != null)
        {
            ani.wrapMode = WrapMode.Loop;
        }
    }

    private void UpdateParts()
    {
        if (parts.Count == 0)
            return;

        for (int j = 0; j < parts.Count; ++j)
        {
            var part = parts[j];

            var subObjs = part.models;
            for (int i = 0; i < subObjs.Count; ++i)
            {
                var subObj = subObjs[i];

                var go = new GameObject();
                var mr = go.AddComponent<SkinnedMeshRenderer>();
                mr.sharedMesh = subObj.mesh;
                mr.sharedMaterial = subObj.material;
                mr.rootBone = transform;
                mr.bones = bones;
                mr.sharedMesh.bindposes = bps;

                go.transform.parent = transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                //go.hideFlags = HideFlags.NotEditable;
                go.name = "Mesh " + (j + 1) + "_" + (i + 1).ToString();
            }
        }
    }
}
