// <copyright file="ZSC.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine, Xadet, Brett19</author>
// <date>2/25/2015 8:37 AM </date>

using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityRose.File;

namespace UnityRose.Formats
{
    /// <summary>
    /// ZSC class.
    /// </summary>
    public class ZSC
    {
        #region Enumerations

        /// <summary>
        /// Glow type.
        /// </summary>
        public enum GlowType
        {
            /// <summary>
            /// None.
            /// </summary>
            None = 0,

            /// <summary>
            /// Not set.
            /// </summary>
            NotSet = 1,

            /// <summary>
            /// Simple.
            /// </summary>
            Simple = 2,

            /// <summary>
            /// Light.
            /// </summary>
            Light = 3,

            /// <summary>
            /// Texture based.
            /// </summary>
            Texture = 4,

            /// <summary>
            /// Texture light.
            /// </summary>
            TextureLight = 5,

            /// <summary>
            /// Alpha.
            /// </summary>
            Alpha = 6
        }

        /// <summary>
        /// Blending type.
        /// </summary>
        public enum BlendingType
        {
            /// <summary>
            /// None.
            /// </summary>
            None = 0,

            /// <summary>
            /// Lighten.
            /// </summary>
            Lighten = 1,

            /// <summary>
            /// Normal.
            /// </summary>
            Normal = 2,

            /// <summary>
            /// Custom.
            /// </summary>
            Custom = 3
        }

        /// <summary>
        /// Bone type.
        /// </summary>
        public enum BoneType
        {
            /// <summary>
            /// Pelvis.
            /// </summary>
            Pelvis = 0,

            /// <summary>
            /// Head.
            /// </summary>
            Head = 4
        }

        /// <summary>
        /// Dummy bone type.
        /// </summary>
        public enum DummyType
        {
            /// <summary>
            /// Right hand.
            /// </summary>
            RightHand = 0,

            /// <summary>
            /// Left hand.
            /// </summary>
            LeftHand = 1,

            /// <summary>
            /// Left shield.
            /// </summary>
            LeftShield = 2,

            /// <summary>
            /// Back.
            /// </summary>
            Back = 3,

            /// <summary>
            /// Feet.
            /// </summary>
            Feet = 4,

            /// <summary>
            /// Face.
            /// </summary>
            Face = 5,

            /// <summary>
            /// Head
            /// </summary>
            Head = 6
        }

        /// <summary>
        /// Collision level type.
        /// </summary>
        public enum CollisionLevelType
        {
            /// <summary>
            /// None.
            /// </summary>
            None = 0,

            /// <summary>
            /// Sphere.
            /// </summary>
            Sphere = 1,

            /// <summary>
            /// Axis aligned bounding box.
            /// </summary>
            AxisAlignedBoundingBox = 2,

            /// <summary>
            /// Oriented bounding box.
            /// </summary>
            OrientedBoundingBox = 3,

            /// <summary>
            /// Polygon.
            /// </summary>
            Polygon = 4
        }

        /// <summary>
        /// Collision pick type.
        /// </summary>
        public enum CollisionPickType
        {
            /// <summary>
            /// Not moveable.
            /// </summary>
            NotMoveable = 1 << 3,

            /// <summary>
            /// Not pickable.
            /// </summary>
            NotPickable = 1 << 4,

            /// <summary>
            /// Height only.
            /// </summary>
            HeightOnly = 1 << 5,

            /// <summary>
            /// No camera collision.
            /// </summary>
            NotCameraCollision = 1 << 6
        }

        /// <summary>
        /// Effect type.
        /// </summary>
        public enum EffectType
        {
            /// <summary>
            /// Normal.
            /// </summary>
            Normal = 0,

            /// <summary>
            /// Day and night.
            /// </summary>
            DayNight = 1,

            /// <summary>
            /// Light container.
            /// </summary>
            LightContainer = 2
        }

        /// <summary>
        /// Flag type.
        /// </summary>
        public enum FlagType
        {
            /// <summary>
            /// Position.
            /// </summary>
            Position = 1,

            /// <summary>
            /// Rotation.
            /// </summary>
            Rotation = 2,

            /// <summary>
            /// Scale.
            /// </summary>
            Scale = 3,

            /// <summary>
            /// Axis rotation.
            /// </summary>
            AxisRotation = 4,

            /// <summary>
            /// Bone index.
            /// </summary>
            BoneIndex = 5,

            /// <summary>
            /// Dummy index.
            /// </summary>
            DummyIndex = 6,

            /// <summary>
            /// Parent.
            /// </summary>
            Parent = 7,

            /// <summary>
            /// Collision
            /// </summary>
            Collision = 29,

            /// <summary>
            /// Motion.
            /// </summary>
            Motion = 30,

            /// <summary>
            /// Range set.
            /// </summary>
            RangeSet = 31,

            /// <summary>
            /// Lightmap.
            /// </summary>
            Lightmap = 32,

            /// <summary>
            /// End loop.
            /// </summary>
            End = 0
        }

        #endregion

        #region Sub Classes

        /// <summary>
        /// Texture class.
        /// </summary>
        public class Texture
        {
            #region Member Declarations

            /// <summary>
            /// Gets or sets the path.
            /// </summary>
            /// <value>The path.</value>
            public string Path { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="Texture"/> is skin.
            /// </summary>
            /// <value><c>true</c> if skin; otherwise, <c>false</c>.</value>
            public bool Skin { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether [alpha enabled].
            /// </summary>
            /// <value><c>true</c> if [alpha enabled]; otherwise, <c>false</c>.</value>
            public bool AlphaEnabled { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether [Z test enabled].
            /// </summary>
            /// <value><c>true</c> if [Z test enabled]; otherwise, <c>false</c>.</value>
            public bool ZTestEnabled { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether [two sided].
            /// </summary>
            /// <value><c>true</c> if [two sided]; otherwise, <c>false</c>.</value>
            public bool TwoSided { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether [alpha test enabled].
            /// </summary>
            /// <value><c>true</c> if [alpha test enabled]; otherwise, <c>false</c>.</value>
            public bool AlphaTestEnabled { get; set; }

            /// <summary>
            /// Gets or sets the alpha reference.
            /// </summary>
            /// <value>The alpha reference.</value>
            public short AlphaReference { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether [Z write enabled].
            /// </summary>
            /// <value><c>true</c> if [Z write enabled]; otherwise, <c>false</c>.</value>
            public bool ZWriteEnabled { get; set; }

            /// <summary>
            /// Gets or sets the blending mode.
            /// </summary>
            /// <value>The blending mode.</value>
            public BlendingType BlendingMode { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether [specular enabled].
            /// </summary>
            /// <value><c>true</c> if [specular enabled]; otherwise, <c>false</c>.</value>
            public bool SpecularEnabled { get; set; }

            /// <summary>
            /// Gets or sets the type of the glow.
            /// </summary>
            /// <value>The type of the glow.</value>
            public GlowType GlowType { get; set; }

            /// <summary>
            /// Gets or sets the alpha.
            /// </summary>
            /// <value>The alpha.</value>
            public float Alpha { get; set; }

            /// <summary>
            /// Gets or sets the glow.
            /// </summary>
            /// <value>The glow.</value>
            public Vector3 Glow { get; set; }

            #endregion
        }

        /// <summary>
        /// Object class.
        /// </summary>
        public class Object
        {
            #region Sub Classes

            /// <summary>
            /// Model class.
            /// </summary>
            public class Model
            {
                #region Member Declarations

                /// <summary>
                /// Gets or sets the model ID.
                /// </summary>
                /// <value>The model ID.</value>
                public short ModelID { get; set; }

                /// <summary>
                /// Gets or sets the texture ID.
                /// </summary>
                /// <value>The texture ID.</value>
                public short TextureID { get; set; }

                /// <summary>
                /// Gets or sets the index of the bone.
                /// </summary>
                /// <value>The index of the bone.</value>
                public BoneType BoneIndex { get; set; }

                /// <summary>
                /// Gets or sets the index of the dummy.
                /// </summary>
                /// <value>The index of the dummy.</value>
                public DummyType DummyIndex { get; set; }

                /// <summary>
                /// Gets or sets the parent.
                /// </summary>
                /// <value>The parent.</value>
                public short Parent { get; set; }

                /// <summary>
                /// Gets or sets the collision level.
                /// </summary>
                /// <value>The collision level.</value>
                public CollisionLevelType CollisionLevel { get; set; }

                /// <summary>
                /// Gets or sets the collision pick.
                /// </summary>
                /// <value>The collision pick.</value>
                public CollisionPickType CollisionPick { get; set; }

                /// <summary>
                /// Gets or sets the range set.
                /// </summary>
                /// <value>The range set.</value>
                public short RangeSet { get; set; }

                /// <summary>
                /// Gets or sets a value indicating whether [use lightmap].
                /// </summary>
                /// <value><c>true</c> if [use lightmap]; otherwise, <c>false</c>.</value>
                public bool UseLightmap { get; set; }

                /// <summary>
                /// Gets or sets the position.
                /// </summary>
                /// <value>The position.</value>
                public Vector3 Position { get; set; }

                /// <summary>
                /// Gets or sets the scale.
                /// </summary>
                /// <value>The scale.</value>
                public Vector3 Scale { get; set; }

                /// <summary>
                /// Gets or sets the rotation.
                /// </summary>
                /// <value>The rotation.</value>
                public Quaternion Rotation { get; set; }

                /// <summary>
                /// Gets or sets the axis rotation.
                /// </summary>
                /// <value>The axis rotation.</value>
                public Quaternion AxisRotation { get; set; }

                /// <summary>
                /// Gets or sets the motion.
                /// </summary>
                /// <value>The motion.</value>
                public string Motion { get; set; }

                #endregion
            }

            /// <summary>
            /// Effect class.
            /// </summary>
            public class Effect
            {
                #region Member Declarations

                /// <summary>
                /// Gets or sets the effect ID.
                /// </summary>
                /// <value>The effect ID.</value>
                public short EffectID { get; set; }

                /// <summary>
                /// Gets or sets the type of the effect.
                /// </summary>
                /// <value>The type of the effect.</value>
                public EffectType EffectType { get; set; }

                /// <summary>
                /// Gets or sets the parent.
                /// </summary>
                /// <value>The parent.</value>
                public short Parent { get; set; }

                /// <summary>
                /// Gets or sets the position.
                /// </summary>
                /// <value>The position.</value>
                public Vector3 Position { get; set; }

                /// <summary>
                /// Gets or sets the scale.
                /// </summary>
                /// <value>The scale.</value>
                public Vector3 Scale { get; set; }

                /// <summary>
                /// Gets or sets the rotation.
                /// </summary>
                /// <value>The rotation.</value>
                public Quaternion Rotation { get; set; }

                #endregion
            }

            #endregion

            #region Member Declarations

            /// <summary>
            /// Gets or sets the cylinder radius.
            /// </summary>
            /// <value>The cylinder radius.</value>
            public int CylinderRadius { get; set; }

            /// <summary>
            /// Gets or sets the cylinder X.
            /// </summary>
            /// <value>The cylinder X.</value>
            public int CylinderX { get; set; }

            /// <summary>
            /// Gets or sets the cylinder Y.
            /// </summary>
            /// <value>The cylinder Y.</value>
            public int CylinderY { get; set; }

            /// <summary>
            /// Gets or sets the bounding box.
            /// </summary>
            /// <value>The bounding box.</value>
            public Bounds boundingBox { get; set; }

            #endregion

            #region List Declarations

            /// <summary>
            /// Gets or sets the models.
            /// </summary>
            /// <value>The models.</value>
            public List<Model> Models { get; set; }

            /// <summary>
            /// Gets or sets the effects.
            /// </summary>
            /// <value>The effects.</value>
            public List<Effect> Effects { get; set; }

            #endregion
        }

        #endregion

        #region List Declarations

        /// <summary>
        /// Gets or sets the textures.
        /// </summary>
        /// <value>The textures.</value>
        public List<Texture> Textures { get; set; }

        /// <summary>
        /// Gets or sets the models.
        /// </summary>
        /// <value>The models.</value>
        public List<string> Models { get; set; }

        /// <summary>
        /// Gets or sets the effects.
        /// </summary>
        /// <value>The effects.</value>
        public List<string> Effects { get; set; }

        /// <summary>
        /// Gets or sets the objects.
        /// </summary>
        /// <value>The objects.</value>
        public List<Object> Objects { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ZSC"/> class.
        /// </summary>
        public ZSC()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZSC"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public ZSC(string filePath)
        {
            Load(filePath);
        }

        private TextAsset asset;
        private FileHandler fh;

        /// <summary>
        /// Loads the specified file.  If resource is found, it is loaded as a Text asset.  Otherwise, the function
        /// assumes this is an editor load and reads from disk
        /// </summary>
        /// <param name="filePath">The file path of the Text Asset resource (without extension) or file (with extension) to load</param>
        public void Load(string filePath)
        {
            asset = Resources.Load(filePath) as TextAsset;
            if (asset != null)
				fh = new FileHandler(asset, Encoding.UTF8);
            else
				fh = new FileHandler(filePath, FileHandler.FileOpenMode.Reading, Encoding.UTF8);

            Load();

        }

        /// <summary>
        /// Loads the specified file.
        /// </summary>
        private void Load()
        {

            short modelCount = fh.Read<short>();
            Models = new List<string>(modelCount);

            for (int i = 0; i < modelCount; i++)
                Models.Add(fh.Read<ZString>());

            short textureCount = fh.Read<short>();
            Textures = new List<Texture>(textureCount);

            for (int i = 0; i < textureCount; i++)
            {
                Textures.Add(new Texture()
                {
                    Path = fh.Read<ZString>(),
                    Skin = fh.Read<short>() > 0,
                    AlphaEnabled = fh.Read<short>() > 0,
                    TwoSided = fh.Read<short>() > 0,
                    AlphaTestEnabled = fh.Read<short>() > 0,
                    AlphaReference = fh.Read<short>(),
                    ZWriteEnabled = fh.Read<short>() > 0,
                    ZTestEnabled = fh.Read<short>() > 0,
                    BlendingMode = (BlendingType)fh.Read<short>(),
                    SpecularEnabled = fh.Read<short>() > 0,
                    Alpha = fh.Read<float>(),
                    GlowType = (GlowType)fh.Read<short>(),
                    Glow = fh.Read<Vector3>()
                });
            }

            short effectCount = fh.Read<short>();
            Effects = new List<string>(effectCount);

            for (int i = 0; i < effectCount; i++)
                Effects.Add(fh.Read<ZString>());

            short objectCount = fh.Read<short>();
            Objects = new List<Object>(objectCount);

            for (int i = 0; i < objectCount; i++)
            {
                Objects.Add(new Object()
                {
                    CylinderRadius = fh.Read<int>() / 100,
                    CylinderX = fh.Read<int>() / 100,
                    CylinderY = fh.Read<int>() / 100,
                    Models = new List<Object.Model>(),
                    Effects = new List<Object.Effect>()
                });

                short objectModelCount = fh.Read<short>();

                if (objectModelCount == 0)
                    continue;

                for (int j = 0; j < objectModelCount; j++)
                {
                    Objects[i].Models.Add(new Object.Model()
                    {
                        ModelID = fh.Read<short>(),
                        TextureID = fh.Read<short>(),
                        Scale = Vector3.one
                    });

                    while (true)
                    {
                        FlagType command = (FlagType)fh.Read<byte>();

                        if (command == FlagType.End)
                            break;

                        byte flagSize = fh.Read<byte>();

                        switch (command)
                        {
                            case FlagType.Position:
                                Objects[i].Models[j].Position = Utils.r2uPosition(new Vector3()
                                {
                                    x = fh.Read<float>(),
                                    y = fh.Read<float>(),
                                    z = fh.Read<float>()
                                });

                                break;
                            case FlagType.Rotation:
                                {

                                    Objects[i].Models[j].Rotation = Utils.r2uRotation(new Quaternion()
                                    {
                                        w = fh.Read<float>(),
                                        x = fh.Read<float>(),
                                        y = fh.Read<float>(),
                                        z = fh.Read<float>()
                                    });
                                }
                                break;
                            case FlagType.Scale:
                                Objects[i].Models[j].Scale = Utils.r2uScale(new Vector3()
                                {
                                    x = fh.Read<float>(),
                                    y = fh.Read<float>(),
                                    z = fh.Read<float>()
                                });
                                break;
                            case FlagType.AxisRotation:
                                Objects[i].Models[j].AxisRotation = Utils.r2uRotation(fh.Read<Quaternion>());
                                break;
                            case FlagType.BoneIndex:
                                Objects[i].Models[j].BoneIndex = (BoneType)fh.Read<short>();
                                break;
                            case FlagType.DummyIndex:
                                Objects[i].Models[j].DummyIndex = (DummyType)fh.Read<short>();
                                break;
                            case FlagType.Parent:
                                Objects[i].Models[j].Parent = fh.Read<short>();
                                break;
                            case FlagType.Collision:
                                {
                                    short bit = fh.Read<short>();

                                    Objects[i].Models[j].CollisionLevel = CollisionLevel(bit);
                                    Objects[i].Models[j].CollisionPick = CollisionPick(bit);
                                }
                                break;
                            case FlagType.Motion:
                                Objects[i].Models[j].Motion = fh.Read<NString>(flagSize);
                                break;
                            case FlagType.RangeSet:
                                Objects[i].Models[j].RangeSet = fh.Read<short>();
                                break;
                            case FlagType.Lightmap:
                                Objects[i].Models[j].UseLightmap = fh.Read<short>() > 0;
                                break;
                            default:
                                fh.Seek(flagSize, SeekOrigin.Current);
                                break;
                        }
                    }
                }

                short effectsCount = fh.Read<short>();

                for (int j = 0; j < effectsCount; j++)
                {
                    Objects[i].Effects.Add(new Object.Effect()
                    {
                        EffectID = fh.Read<short>(),
                        EffectType = (EffectType)fh.Read<short>()
                    });

                    while (true)
                    {
                        FlagType command = (FlagType)fh.Read<byte>();

                        if (command == FlagType.End)
                            break;

                        byte flagSize = fh.Read<byte>();

                        switch (command)
                        {
                            case FlagType.Position:
                                Objects[i].Effects[j].Position = Utils.r2uPosition(fh.Read<Vector3>() / 100.0f);
                                break;
                            case FlagType.Rotation:
                                Objects[i].Effects[j].Rotation = Utils.r2uRotation(fh.Read<Quaternion>());
                                break;
                            case FlagType.Scale:
                                Objects[i].Effects[j].Scale = Utils.r2uScale(fh.Read<Vector3>());
                                break;
                            case FlagType.Parent:
                                Objects[i].Effects[j].Parent = (short)(fh.Read<short>() - 1);
                                break;
                            default:
                                fh.Seek(flagSize, SeekOrigin.Current);
                                break;
                        }
                    }
                }

                /*
                Objects[i].boundingBox = new Bounds()
                {
                	
                    Min = fh.Read<Vector3>() / 100.0f,
                    Max = fh.Read<Vector3>() / 100.0f
                };
                */

                fh.Read<Vector3>();
                fh.Read<Vector3>();
                Objects[i].boundingBox = new Bounds();

            }

            fh.Close();

            if (asset != null)
                Resources.UnloadAsset(asset);

        }

        #region Static Functions

        /// <summary>
        /// Gets the collisions level.
        /// </summary>
        /// <param name="bit">The bit.</param>
        /// <returns>The collision level type</returns>
        public static CollisionLevelType CollisionLevel(int bit)
        {
            return (CollisionLevelType)(bit & 0x7);
        }

        /// <summary>
        /// Gets the collisions pick.
        /// </summary>
        /// <param name="bit">The bit.</param>
        /// <returns>The collision pick type</returns>
        public static CollisionPickType CollisionPick(int bit)
        {
            if ((bit & (int)CollisionPickType.HeightOnly) > 0)
                return CollisionPickType.HeightOnly;
            else if ((bit & (int)CollisionPickType.NotCameraCollision) > 0)
                return CollisionPickType.NotCameraCollision;
            else if ((bit & (int)CollisionPickType.NotMoveable) > 0)
                return CollisionPickType.NotMoveable;

            return CollisionPickType.NotPickable;
        }

        #endregion
    }
}