using UnityEngine;
using UnityEditor;
using UnityRose.Formats;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class ROSEImport
{
    public class ParticleSystemImport : Importer.ImportItem
    {
        public class Sequence : Importer.ImportItem
        {
            public Revise.Files.PTL.ParticleSequence SourceSeq;
            public PsMaterial Material = null;

            public Sequence(Importer parent)
            {

            }

        }

        public class PsMaterial : Importer.ImportItem<Material>
        {
            public Revise.Files.PTL.ParticleSequence SourceSeq;
            public TextureImport Texture = null;

            public PsMaterial(Importer parent)
            {

            }

            protected override void DoImport(string targetPath)
            {
                var mat = new Material(Shader.Find("Particles/Alpha Blended"));
                mat.SetTexture("_MainTex", Texture.GetData());
                AssetDatabase.CreateAsset(mat, targetPath);
            }

        }

        public string SourcePath = "";
        public List<Sequence> Sequences = null;

        public ParticleSystemImport(Importer parent, string path)
        {
            SourcePath = Utils.NormalizePath(path);

            var basePath = GenerateAssetPath(path, ".particle");
            var matBasePath = Utils.CombinePath(Path.GetDirectoryName(basePath), "Materials");
            var matBaseName = Path.GetFileNameWithoutExtension(basePath);

            var ptl = new Revise.Files.PTL.ParticleFile();
            ptl.Load(GetDataPath(SourcePath));

            Sequences = new List<Sequence>();
            for (var i = 0; i < ptl.Sequences.Count; ++i)
            {
                var seq = ptl.Sequences[i];
                var psSeq = new Sequence(parent);

                psSeq.SourceSeq = seq;

                var mat = new PsMaterial(parent);
                mat._targetPath = Utils.CombinePath(matBasePath, matBaseName + "_" + i + ".asset");
                mat.Texture = parent.MakeTexture(ptl.Sequences[i].TextureFileName);
                parent.AddItem(mat);
                psSeq.Material = mat;

                Sequences.Add(psSeq);
            }
        }

        public enum MinMaxCurveState
        {
            k_Scalar,
            k_Curve,
            k_TwoCurves,
            k_TwoScalars
        }

        public class SerializedMinMaxCurve
        {
            protected SerializedProperty _scalar;
            protected SerializedProperty _minCurve;
            protected SerializedProperty _maxCurve;
            protected SerializedProperty _minMaxState;

            public SerializedMinMaxCurve(SerializedProperty prop)
            {
                _scalar = prop.FindPropertyRelative("scalar");
                _minCurve = prop.FindPropertyRelative("minCurve");
                _maxCurve = prop.FindPropertyRelative("maxCurve");
                _minMaxState = prop.FindPropertyRelative("minMaxState");
            }

            public float Scalar
            {
                get { return _scalar.floatValue; }
                set { _scalar.floatValue = value; }
            }

            public MinMaxCurveState MinMaxState
            {
                get { return (MinMaxCurveState)_minMaxState.intValue; }
                set { _minMaxState.intValue = (int)value; }
            }

            public AnimationCurve MinCurve
            {
                get { return _minCurve.animationCurveValue; }
                set { _minCurve.animationCurveValue = value; }
            }

            public AnimationCurve MaxCurve
            {
                get { return _maxCurve.animationCurveValue; }
                set { _maxCurve.animationCurveValue = value; }
            }

            public void SetValue(float val)
            {
                Scalar = val;
                MinMaxState = MinMaxCurveState.k_Scalar;
            }

            public void SetMinMax(float min, float max)
            {
                if (min == max)
                {
                    SetValue(min);
                    return;
                }

                Scalar = max;
                var minCurve = new AnimationCurve();
                minCurve.AddKey(0, min / max);
                MinCurve = minCurve;
                var maxCurve = new AnimationCurve();
                maxCurve.AddKey(0, 1);
                MaxCurve = maxCurve;
                MinMaxState = MinMaxCurveState.k_TwoScalars;
            }

            public void SetValue(AnimationCurve curve, float scalar)
            {
                Scalar = scalar;
                MaxCurve = curve;
                MinMaxState = MinMaxCurveState.k_Curve;
            }

            public void SetMinMax(AnimationCurve minCurve, AnimationCurve maxCurve, float scalar)
            {
                if (Utils.CurvesMatch(minCurve, maxCurve) || minCurve == null || maxCurve == null)
                {
                    if (maxCurve == null)
                        SetValue(minCurve, scalar);
                    else
                        SetValue(maxCurve, scalar);
                    return;
                }
                MinCurve = minCurve;
                MaxCurve = maxCurve;
                Scalar = scalar;
                MinMaxState = MinMaxCurveState.k_TwoCurves;
            }

        }

        public class PtlCurveBuilder
        {
            private float _lifetime = 0;
            private float _scalar = 0;
            private Dictionary<float, float> _min = new Dictionary<float, float>();
            private Dictionary<float, float> _max = new Dictionary<float, float>();

            public PtlCurveBuilder(float lifetime)
            {
                _lifetime = lifetime;
            }

            public bool IsEmpty
            {
                get { return _min.Count == 0 && _max.Count == 0; }
            }

            public void AddKey(float minTime, float maxTime, float min, float max)
            {
                _min[minTime] = min;
                _max[minTime] = max;
                if (maxTime != minTime)
                {
                    //_min[maxTime] = min;
                    //_max[maxTime] = max;
                }

                if (_scalar < min * 1.1f)
                    _scalar = min * 1.1f;
                if (_scalar < max * 1.1f)
                    _scalar = max * 1.1f;
            }

            public void ApplyToMinMax(SerializedMinMaxCurve minMax)
            {
                var minCurve = new AnimationCurve();
                foreach (var i in _min)
                    minCurve.AddKey(i.Key / _lifetime, i.Value / _scalar);

                var maxCurve = new AnimationCurve();
                foreach (var i in _max)
                    maxCurve.AddKey(i.Key / _lifetime, i.Value / _scalar);

                minMax.SetMinMax(minCurve, maxCurve, _scalar);
            }

        }

        private GameObject BuildSequence(Sequence seq)
        {
            var go = new GameObject();
            go.name = "Particle System";

            var ps = go.AddComponent<ParticleSystem>();
            var psr = go.GetComponent<ParticleSystemRenderer>();
            var psData = new SerializedObject(ps);

            var lifetimeCurve = new SerializedMinMaxCurve(psData.FindProperty("InitialModule.startLifetime"));
            lifetimeCurve.SetMinMax(seq.SourceSeq.Lifetime.Minimum, seq.SourceSeq.Lifetime.Maximum);

            var speedCurve = new SerializedMinMaxCurve(psData.FindProperty("InitialModule.startSpeed"));
            speedCurve.SetValue(0);

            psData.FindProperty("InitialModule.maxNumParticles").intValue = seq.SourceSeq.ParticleCount;


            var sizeIck = false;
            var sizeCurves = new PtlCurveBuilder(seq.SourceSeq.Lifetime.Maximum);
            var velXCurves = new PtlCurveBuilder(seq.SourceSeq.Lifetime.Maximum);
            var velYCurves = new PtlCurveBuilder(seq.SourceSeq.Lifetime.Maximum);
            var velZCurves = new PtlCurveBuilder(seq.SourceSeq.Lifetime.Maximum);
            var rotCurves = new PtlCurveBuilder(seq.SourceSeq.Lifetime.Maximum);
            var uvCurves = new PtlCurveBuilder(seq.SourceSeq.Lifetime.Maximum);

            for (var i = 0; i < seq.SourceSeq.Events.Count; ++i)
            {
                var evt = seq.SourceSeq.Events[i];

                if (evt.Type == Revise.Files.PTL.ParticleEventType.Scale)
                {
                    var scaleEvt = (Revise.Files.PTL.Events.ScaleEvent)evt;

                    if (scaleEvt.Scale.Minimum.x != scaleEvt.Scale.Minimum.y)
                        sizeIck = true;

                    var minVal = (scaleEvt.Scale.Minimum.x + scaleEvt.Scale.Minimum.y) / 2;
                    var maxVal = (scaleEvt.Scale.Maximum.x + scaleEvt.Scale.Maximum.y) / 2;

                    sizeCurves.AddKey(
                        scaleEvt.TimeRange.Minimum, scaleEvt.TimeRange.Maximum,
                        minVal, maxVal);
                }
                else if (evt.Type == Revise.Files.PTL.ParticleEventType.Velocity)
                {
                    var velEvt = (Revise.Files.PTL.Events.VelocityEvent)evt;

                    velXCurves.AddKey(
                        velEvt.TimeRange.Minimum, velEvt.TimeRange.Maximum,
                        velEvt.Velocity.Minimum.x, velEvt.Velocity.Maximum.x);
                    velYCurves.AddKey(
                        velEvt.TimeRange.Minimum, velEvt.TimeRange.Maximum,
                        velEvt.Velocity.Minimum.y, velEvt.Velocity.Maximum.y);
                    velZCurves.AddKey(
                        velEvt.TimeRange.Minimum, velEvt.TimeRange.Maximum,
                        velEvt.Velocity.Minimum.z, velEvt.Velocity.Maximum.z);
                }
                else if (evt.Type == Revise.Files.PTL.ParticleEventType.VelocityX)
                {
                    var velEvt = (Revise.Files.PTL.Events.VelocityXEvent)evt;
                    velXCurves.AddKey(
                        velEvt.TimeRange.Minimum, velEvt.TimeRange.Maximum,
                        velEvt.Velocity.Minimum, velEvt.Velocity.Maximum);
                }
                else if (evt.Type == Revise.Files.PTL.ParticleEventType.VelocityY)
                {
                    var velEvt = (Revise.Files.PTL.Events.VelocityYEvent)evt;
                    velXCurves.AddKey(
                        velEvt.TimeRange.Minimum, velEvt.TimeRange.Maximum,
                        velEvt.Velocity.Minimum, velEvt.Velocity.Maximum);
                }
                else if (evt.Type == Revise.Files.PTL.ParticleEventType.VelocityZ)
                {
                    var velEvt = (Revise.Files.PTL.Events.VelocityZEvent)evt;
                    velXCurves.AddKey(
                        velEvt.TimeRange.Minimum, velEvt.TimeRange.Maximum,
                        velEvt.Velocity.Minimum, velEvt.Velocity.Maximum);
                }
                else if (evt.Type == Revise.Files.PTL.ParticleEventType.Rotation)
                {
                    var rotEvt = (Revise.Files.PTL.Events.RotationEvent)evt;

                    Debug.LogWarning("Unimplemented Particle Rotation Event");

                }
                else if (evt.Type == Revise.Files.PTL.ParticleEventType.Texture)
                {
                    var texEvt = (Revise.Files.PTL.Events.TextureEvent)evt;

                    uvCurves.AddKey(
                        texEvt.TimeRange.Minimum, texEvt.TimeRange.Maximum,
                        texEvt.TextureIndex.Minimum, texEvt.TextureIndex.Maximum);
                }
            }

            if (!sizeCurves.IsEmpty)
            {
                if (sizeIck)
                    Debug.LogWarning("Particle effect with non-square scaling... " + _targetPath);

                psData.FindProperty("SizeModule.enabled").boolValue = true;
                sizeCurves.ApplyToMinMax(new SerializedMinMaxCurve(psData.FindProperty("SizeModule.curve")));
            }
            else
            {
                psData.FindProperty("SizeModule.enabled").boolValue = false;
            }

            if (!velXCurves.IsEmpty || !velYCurves.IsEmpty || !velZCurves.IsEmpty)
            {
                psData.FindProperty("VelocityModule.enabled").boolValue = true;
                velXCurves.ApplyToMinMax(new SerializedMinMaxCurve(psData.FindProperty("VelocityModule.x")));
                velYCurves.ApplyToMinMax(new SerializedMinMaxCurve(psData.FindProperty("VelocityModule.y")));
                velZCurves.ApplyToMinMax(new SerializedMinMaxCurve(psData.FindProperty("VelocityModule.z")));
            }
            else
            {
                psData.FindProperty("VelocityModule.enabled").boolValue = false;
            }

            if (!uvCurves.IsEmpty)
            {
                psData.FindProperty("UVModule.enabled").boolValue = true;
                psData.FindProperty("UVModule.tilesX").intValue = seq.SourceSeq.TextureWidth;
                psData.FindProperty("UVModule.tilesY").intValue = seq.SourceSeq.TextureHeight;
                uvCurves.ApplyToMinMax(new SerializedMinMaxCurve(psData.FindProperty("UVModule.frameOverTime")));
            }
            else
            {
                psData.FindProperty("UVModule.enabled").boolValue = false;
            }

            psData.ApplyModifiedPropertiesWithoutUndo();

            psr.material = seq.Material.GetData();

            return go;
        }

        protected override void DoImport(string targetPath)
        {
            Debug.Log("Import Particle System " + targetPath);

            GameObject rootGo = null;
            for (var i = 0; i < Sequences.Count; ++i)
            {
                GameObject seqGo = BuildSequence(Sequences[i]);
                if (rootGo == null)
                    rootGo = seqGo;
                else
                    seqGo.transform.parent = rootGo.transform;
            }
        }
    }
}
