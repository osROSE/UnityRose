using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(RoseMapObjectData))]
public class RoseMapObjectDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var myTarget = (RoseMapObjectData)target;
        var subObjs = myTarget.subObjects;

        int numSubObj = EditorGUILayout.IntField("Mesh Count", subObjs.Count);
        if (numSubObj != subObjs.Count)
        {
            var newSubObjs = new List<RoseMapObjectData.SubObject>();
            for (var i = 0; i < numSubObj; ++i)
            {
                if (i < System.Math.Min(numSubObj, subObjs.Count))
                {
                    newSubObjs.Add(subObjs[i]);
                }
                else
                {
                    newSubObjs.Add(new RoseMapObjectData.SubObject());
                }
            }
            myTarget.subObjects = newSubObjs;
            subObjs = newSubObjs;
        }

        for (int i = 0; i < subObjs.Count; ++i)
        {
            EditorGUILayout.LabelField("Mesh " + i.ToString());
            EditorGUI.indentLevel++;
            subObjs[i].mesh = (Mesh)EditorGUILayout.ObjectField("Mesh", subObjs[i].mesh, typeof(Mesh), false);
            subObjs[i].material = (Material)EditorGUILayout.ObjectField("Material", subObjs[i].material, typeof(Material), false);
            subObjs[i].parent = EditorGUILayout.IntField("Parent", subObjs[i].parent);
            subObjs[i].position = EditorGUILayout.Vector3Field("Position", subObjs[i].position);
            subObjs[i].rotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Rotation", subObjs[i].rotation.eulerAngles));
            subObjs[i].scale = EditorGUILayout.Vector3Field("Scale", subObjs[i].scale);
            EditorGUI.indentLevel--;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private PreviewRenderUtility m_PreviewUtility = null;
    private void Init()
    {
        if (this.m_PreviewUtility == null)
        {
            this.m_PreviewUtility = new PreviewRenderUtility();
            this.m_PreviewUtility.m_CameraFieldOfView = 30f;
        }
    }

    public Vector2 previewDir = new Vector2(160f, -20f);

    private static void RenderMeshPreviewSkipCameraAndLighting(RoseMapObjectData mapObj, Matrix4x4[] mats, Bounds bounds, PreviewRenderUtility previewUtility, MaterialPropertyBlock customProperties, Vector2 direction)
    {
        bool fog = RenderSettings.fog;
        Unsupported.SetRenderSettingsUseFogNoDirty(false);

        Quaternion quaternion = Quaternion.Euler(direction.y, 0f, 0f) * Quaternion.Euler(0f, direction.x, 0f);
        Vector3 pos = quaternion * -bounds.center;
        Matrix4x4 rootTRS = new Matrix4x4();
        rootTRS.SetTRS(pos, quaternion, Vector3.one);

        for (var i = 0; i < mapObj.subObjects.Count; ++i)
        {
            var subObj = mapObj.subObjects[i];
            previewUtility.DrawMesh(subObj.mesh, rootTRS * mats[i], subObj.material, 0);
        }

        previewUtility.m_Camera.Render();
        Unsupported.SetRenderSettingsUseFogNoDirty(fog);
    }

    public static void RenderMeshPreview(RoseMapObjectData mapObj, PreviewRenderUtility previewUtility, Vector2 direction)
    {
        if (mapObj == null || previewUtility == null)
        {
            return;
        }

        Bounds bounds = new Bounds();
        Matrix4x4[] mats = new Matrix4x4[mapObj.subObjects.Count];
        for (var i = 0; i < mapObj.subObjects.Count; ++i)
        {
            var subObj = mapObj.subObjects[i];

            Matrix4x4 mat = new Matrix4x4();
            mat.SetTRS(subObj.position, subObj.rotation, subObj.scale);
            if (i != 0)
                mat = mats[subObj.parent - 1] * mat;

            Utils.EncapsulateTransformedBounds(ref bounds, mat, subObj.mesh.bounds, i == 0);

            mats[i] = mat;
        }
        
        float magnitude = bounds.extents.magnitude;
        float num = 4f * magnitude;
        previewUtility.m_Camera.transform.position = -Vector3.forward * num;
        previewUtility.m_Camera.transform.rotation = Quaternion.identity;
        previewUtility.m_Camera.nearClipPlane = num - magnitude * 1.1f;
        previewUtility.m_Camera.farClipPlane = num + magnitude * 1.1f;
        previewUtility.m_Light[0].intensity = 1.4f;
        previewUtility.m_Light[0].transform.rotation = Quaternion.Euler(40f, 40f, 0f);
        previewUtility.m_Light[1].intensity = 1.4f;
        Color ambient = new Color(0.1f, 0.1f, 0.1f, 0f);
        RenderMeshPreviewSkipCameraAndLighting(mapObj, mats, bounds, previewUtility, null, direction);
    }

    private void DoRenderPreview()
    {
        RenderMeshPreview(this.target as RoseMapObjectData, this.m_PreviewUtility, this.previewDir);
    }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        this.Init();

        m_PreviewUtility.BeginStaticPreview(new Rect(0, 0, width, height));
        DoRenderPreview();
        return m_PreviewUtility.EndStaticPreview();
    }

    public override bool HasPreviewGUI()
    {
        return this.target != null;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        this.Init();

        previewDir = PreviewGUI.Drag2D(previewDir, r);
        if (Event.current.type != EventType.Repaint)
        {
            return;
        }

        m_PreviewUtility.BeginPreview(r, background);
        DoRenderPreview();
        m_PreviewUtility.EndAndDrawPreview(r);
    }

}
