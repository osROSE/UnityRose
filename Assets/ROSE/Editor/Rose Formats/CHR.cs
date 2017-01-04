using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityRose.File;

namespace UnityRose.Formats
{
    public class CHR
    {
        public enum AnimationType : short
        {
            Stop = 0,
            Move,
            Attack,
            Hit,
            Die,
            Rie,
            Cast1,
            SkillAction1,
            Cast2,
            SkillAction2,
            Etc,
        }

        public class CharacterAnimation
        {
            #region Properties

            /// <summary>
            /// Gets or sets the animation type.
            /// </summary>
            public AnimationType Type
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the animation file index.
            /// </summary>
            public short Animation
            {
                get;
                set;
            }

            #endregion
        }

        public class CharacterEffect
        {
            #region Properties

            /// <summary>
            /// Gets or sets the bone index.
            /// </summary>
            public short Bone
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the effect file index.
            /// </summary>
            public short Effect
            {
                get;
                set;
            }

            #endregion
        }

        public class CharacterObject
        {
            #region Properties

            /// <summary>
            /// Gets or sets the object linked to the corresponding ZSC file.
            /// </summary>
            public short Object
            {
                get;
                set;
            }

            #endregion
        }

        public class Character
        {
            #region Properties

            /// <summary>
            /// Gets or sets a value indicating whether this character is enabled.
            /// </summary>
            public bool IsEnabled
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the ID.
            /// </summary>
            public short ID
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            public string Name
            {
                get;
                set;
            }

            /// <summary>
            /// Gets the character objects.
            /// </summary>
            public List<CharacterObject> Objects
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets the character animations.
            /// </summary>
            public List<CharacterAnimation> Animations
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets the character effects.
            /// </summary>
            public List<CharacterEffect> Effects
            {
                get;
                private set;
            }

            #endregion

            /// <summary>
            /// Initializes a new instance of the <see cref="Character"/> class.
            /// </summary>
            public Character()
            {
                Name = string.Empty;
                IsEnabled = true;

                Objects = new List<CharacterObject>();
                Animations = new List<CharacterAnimation>();
                Effects = new List<CharacterEffect>();
            }
        }

        public List<string> SkeletonFiles
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the motion files.
        /// </summary>
        public List<string> MotionFiles
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the effect files.
        /// </summary>
        public List<string> EffectFiles
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the characters.
        /// </summary>
        public List<Character> Characters
        {
            get;
            private set;
        }

        public CHR(string filePath)
        {
            SkeletonFiles = new List<string>();
            MotionFiles = new List<string>();
            EffectFiles = new List<string>();
            Characters = new List<Character>();

            Load(filePath);
        }

        public void Load(string filePath)
        {
            var fh = new FileHandler(filePath, FileHandler.FileOpenMode.Reading, Encoding.UTF8);

            short skeletonFileCount = fh.Read<short>();

            for (int i = 0; i < skeletonFileCount; i++)
            {
                string skeletonFile = fh.Read<ZString>();
                SkeletonFiles.Add(skeletonFile);
            }

            short motionFileCount = fh.Read<short>();

            for (int i = 0; i < motionFileCount; i++)
            {
                string motionFile = fh.Read<ZString>();

                MotionFiles.Add(motionFile);
            }

            short effectFileCount = fh.Read<short>();

            for (int i = 0; i < effectFileCount; i++)
            {
                string effectFile = fh.Read<ZString>();

                EffectFiles.Add(effectFile);
            }

            short characterCount = fh.Read<short>();

            for (int i = 0; i < characterCount; i++)
            {
                Character character = new Character();
                character.IsEnabled = fh.Read<char>() != 0;

                if (character.IsEnabled)
                {
                    character.ID = fh.Read<short>();
                    character.Name = fh.Read<ZString>();

                    short objectCount = fh.Read<short>();

                    for (int j = 0; j < objectCount; j++)
                    {
                        CharacterObject @object = new CharacterObject();
                        @object.Object = fh.Read<short>();

                        character.Objects.Add(@object);
                    }

                    short animationCount = fh.Read<short>();

                    for (int j = 0; j < animationCount; j++)
                    {
                        CharacterAnimation animation = new CharacterAnimation();
                        animation.Type = (AnimationType)fh.Read<short>();
                        animation.Animation = fh.Read<short>();

                        character.Animations.Add(animation);
                    }

                    short effectCount = fh.Read<short>();

                    for (int j = 0; j < effectCount; j++)
                    {
                        CharacterEffect effect = new CharacterEffect();
                        effect.Bone = fh.Read<short>();
                        effect.Effect = fh.Read<short>();

                        character.Effects.Add(effect);
                    }
                }

                Characters.Add(character);
            }
        }
    }
}
