// <copyright file="RosePlayer.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine</author>
// <date>6/13/2015 7:01 PM </date>
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityRose.Formats;
using System.IO;
using UnityRose;

#if UNITY_EDITOR
    using UnityEditor;
#endif


namespace UnityRose
{
    /// <summary>
    /// This singleton class loads common resources into memory for quick access.
    /// Avoids having to reload certain resources that are almost always needed.
    /// For example ZSC files, every time a piece of armor is equipped or appears 
    /// on another player.
    /// 
    /// TODO: This class should be improved to cache most common equipment.
    /// </summary>
    public class ResourceManager
    {
		public static Dictionary<int, WeaponType> weapon_type_lookup = new Dictionary<int, WeaponType>() {
			{ 211, WeaponType.OHSWORD},
			{ 212, WeaponType.OHMACE},
			{ 271, WeaponType.XBOX},
			{ 221, WeaponType.THSWORD},
			{ 223, WeaponType.THBLUNT},
			{ 222, WeaponType.THSPEAR},
			{ 231, WeaponType.BOW},
			{ 232, WeaponType.GUN},
			{ 233, WeaponType.CANNON},
			{ 241, WeaponType.STAFF},
			{ 242, WeaponType.WAND},
			{ 251, WeaponType.KATAR},
			{ 252, WeaponType.DSW},
		};

        // Male ZSC's (equipment model links)
        public ZSC zsc_body_male;
        public ZSC zsc_arms_male;
        public ZSC zsc_foot_male;
        public ZSC zsc_face_male;
        public ZSC zsc_hair_male;
        public ZSC zsc_cap_male;

        // Female ZSC's
        public ZSC zsc_body_female;
        public ZSC zsc_arms_female;
        public ZSC zsc_foot_female;
        public ZSC zsc_face_female;
        public ZSC zsc_hair_female;
        public ZSC zsc_cap_female;

        // Unisex ZSC's
        public ZSC zsc_back;
        public ZSC zsc_faceItem;
        public ZSC zsc_weapon;
        public ZSC zsc_subweapon;

        // ZMD's (skeleton)
        public ZMD zmd_male;
        public ZMD zmd_female;

        // STB's
        public STB stb_animation_list;
        public STB stb_animation_type;
		public STB stb_weapon_list;
        public STB stb_cap_list;
        public STB stb_hair_list;


        // TODO: add any other common persistent resources here

        
        // TODO: determine optimal cache size
        // Possibly have a different cache for each resource type ?
        private const int CACHE_SIZE = 250;
        private Cache cache;

        // Static reference to a singleton instance of ResourceManager
        public static ResourceManager instance;

        private ResourceManager()
        {

            zsc_body_male = (ZSC)loadResource("3DDATA/AVATAR/LIST_MBODY.ZSC");
			zsc_arms_male = (ZSC)loadResource("3DData/Avatar/LIST_MARMS.zsc");
			zsc_foot_male = (ZSC)loadResource("3DData/Avatar/LIST_MFOOT.zsc");
			zsc_face_male = (ZSC)loadResource("3DData/Avatar/LIST_MFACE.zsc");
			zsc_hair_male = (ZSC)loadResource("3DData/Avatar/LIST_MHAIR.zsc");
			zsc_cap_male = (ZSC)loadResource("3DData/Avatar/LIST_MCAP.zsc");

            zsc_body_female = (ZSC)loadResource("3DData/Avatar/LIST_WBODY.zsc");
            zsc_arms_female = (ZSC)loadResource("3DData/Avatar/LIST_WARMS.zsc");
            zsc_foot_female = (ZSC)loadResource("3DData/Avatar/LIST_WFOOT.zsc");
            zsc_face_female = (ZSC)loadResource("3DData/Avatar/LIST_WFACE.zsc");
            zsc_hair_female = (ZSC)loadResource("3DData/Avatar/LIST_WHAIR.zsc");
            zsc_cap_female = (ZSC)loadResource("3DData/Avatar/LIST_WCAP.zsc");

            zsc_back = (ZSC)loadResource("3DData/Avatar/LIST_BACK.zsc");
            zsc_faceItem = (ZSC)loadResource("3DData/Avatar/LIST_FACEIEM.zsc");

            zsc_weapon = (ZSC)loadResource("3DDATA/WEAPON/LIST_WEAPON.zsc");
            zsc_subweapon = (ZSC)loadResource("3DDATA/WEAPON/LIST_SUBWPN.zsc");

            stb_animation_list = (STB)loadResource("3Ddata/STB/FILE_MOTION.STB");
            stb_animation_type = (STB)loadResource("3DDATA/STB/TYPE_MOTION.STB");

			stb_weapon_list = (STB)loadResource("3DDATA/STB/LIST_WEAPON.STB");
            stb_cap_list = (STB)loadResource("3DDATA/STB/LIST_CAP.STB");
            stb_hair_list = (STB)loadResource("3DDATA/STB/LIST_HAIR.STB");

            cache = new Cache(this, CACHE_SIZE);
        }

        public static ResourceManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ResourceManager();

                return instance;
            }
        }

        /// <summary>
        /// Get the ZSC object associated with the given gender and bodyPart
        /// </summary>
        /// <param name="gender"></param>
        /// <param name="bodyPart"></param>
        /// <returns></returns>
        public ZSC getZSC( GenderType gender, BodyPartType bodyPart)
        {
            bool male = gender == GenderType.MALE;
            switch(bodyPart)
            {
                case BodyPartType.BODY:
                    return male ? zsc_body_male : zsc_body_female;
                case BodyPartType.ARMS:
                    return male ? zsc_arms_male : zsc_arms_female;
                case BodyPartType.FOOT:
                    return male ? zsc_foot_male : zsc_foot_female;
                case BodyPartType.FACE:
                    return male ? zsc_face_male : zsc_face_female;
                case BodyPartType.HAIR:
                    return male ? zsc_hair_male : zsc_hair_female;
                case BodyPartType.CAP:
                    return male ? zsc_cap_male : zsc_cap_female;
                case BodyPartType.BACK:
                    return zsc_back;
                case BodyPartType.FACEITEM:
                    return zsc_faceItem;
				case BodyPartType.WEAPON:
					return zsc_weapon;
				case BodyPartType.SUBWEAPON:
					return zsc_subweapon;
                default:
                    return null;
            }
        }
        /// <summary>
        /// Load a Rose Asset from text asset resource file to memory. Not cached.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public object loadResource(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            switch (dir.Extension)
            {
                case ".zms":
                case ".ZMS":
                    return new ZMS(path);
                case ".zmd":
                case ".ZMD":
                    return new ZMD(path);
                case ".zsc":
                case ".ZSC":
                    return new ZSC(path);
                case ".stb":
                case ".STB":
                    return new STB(path);
                case ".zmo":
                case ".ZMO":
                    return new ZMO(path);
                // TODO: add all other rose formats here
                case ".png":
                case ".PNG":
                    return Resources.Load(path.Replace(dir.Extension, ""));
                default:
                    return null;
            }
        }

        public void unloadResource(UnityEngine.Object resource)
        {
            Resources.UnloadAsset(resource);
        }

        /// <summary>
        /// Checks the cache to see if the resource has already been loaded recently
        /// If found, returns the cached resource from memory (fast)
        /// If not found, loads the resource from file (slow) and caches the resource
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public object cachedLoad(string path)
        {
            return cache.request(path);
        }

        /// <summary>
        /// Load a skeleton game object from resources prefab
        /// </summary>
        /// <param name="gender"></param>
        /// <param name="weapon"></param>
        /// <returns></returns>
        public GameObject loadSkeleton(GenderType gender, WeaponType weapon)
        {
			var prefab = Resources.Load ("Animation/" + gender.ToString () + "/" + weapon.ToString () + "/skeleton");
			var clone =  GameObject.Instantiate ( prefab );
			return clone as GameObject;
        }

        public GameObject loadSkeleton(GenderType gender, RigType rig)
        {
            var prefab = Resources.Load("Animation/" + gender.ToString() + "/" + rig.ToString() + "/skeleton");
            var clone = GameObject.Instantiate(prefab);
            return clone as GameObject;
        }


        /// <summary>
        /// Load bindposes and bones matrices from resources scriptable object
        /// </summary>
        /// <param name="gender"></param>
        /// <param name="weapon"></param>
        /// <returns></returns>
        public BindPoses loadBindPoses(GameObject skeleton, GenderType gender, WeaponType weapon)
		{
			BindPoses poses =  ScriptableObject.Instantiate<BindPoses>((BindPoses)Resources.Load ("Animation/" + gender.ToString () + "/" + weapon.ToString () + "/bindPoses"));
			for (int i = 0; i < poses.boneNames.Length; i++) {
				poses.boneTransforms[i] = Utils.findChild(skeleton, poses.boneNames[i]);
			}
			return poses;
		}


        /// <summary>
        /// Get Animation ZMO File path
        /// </summary>
        /// <param name="WeaponType">Equiped Weapon</param>
        /// <param name="Animation">Player Action</param>
        /// <param name="Gender">Player Gender</param>
        /// <returns> The file path of the given animation </returns>
        public string GetZMOPath(WeaponType WeaponType, ActionType Action, GenderType Gender)
        {
            string filePath = stb_animation_list.Cells[int.Parse(stb_animation_type.Cells[(int)Action][(int)WeaponType])][(int)Gender];

            //if no female animation then use male one
            if (filePath == "")
                filePath = stb_animation_list.Cells[int.Parse(stb_animation_type.Cells[(int)Action][(int)WeaponType])][(int)GenderType.MALE];


            return filePath;
        }

        public WeaponType getWeaponType(int weaponID)
        {
			int typeID = 0; 
			WeaponType type = WeaponType.EMPTY;
			try {
				typeID = int.Parse(stb_weapon_list.Cells [weaponID][5]); // TODO: create enums for the columns and use them to look things up
				type = weapon_type_lookup [typeID];
			} catch ( Exception e){
				type = WeaponType.EMPTY;
			}

			return type;
        }

#if UNITY_EDITOR


        /// <summary>
        /// Loop through all weapon types for each gender and create an animation asset and all associated clips
        /// The animations and clips are placed in GameData/Animation
        /// </summary>      
        public void GenerateAnimationAssets()
        {
            foreach(GenderType gender in Enum.GetValues(typeof(GenderType)))
            {  
                foreach(WeaponType weapon in Enum.GetValues(typeof(WeaponType)))
                {
                    GenerateAnimationAsset(gender, weapon);
                }
            }
        }

        private string[] getBoneNames(Transform[] transforms)
		{
			List<string> names = new List<string> ();
			foreach (Transform transform in transforms) 
			{
				names.Add(transform.name);
			}

			return names.ToArray ();
		}
        /// <summary>
        /// Generate an animation prefab for the given gender and weapon
        /// </summary>
        /// <param name="gender"></param>
        /// <param name="weapon"></param>
        public void GenerateAnimationAsset(GenderType gender, WeaponType weapon)
        {
            GameObject skeleton = new GameObject("skeleton");
            bool male = (gender == GenderType.MALE);
            ZMD zmd = new ZMD( male ? "Assets/3DData/Avatar/MALE.ZMD" : "Assets/3DData/Avatar/FEMALE.ZMD");
            zmd.buildSkeleton(skeleton);

			BindPoses poses = ScriptableObject.CreateInstance<BindPoses> ();
			poses.bindPoses = zmd.bindposes;
			poses.boneNames = getBoneNames (zmd.boneTransforms);
			poses.boneTransforms = zmd.boneTransforms;
            LoadClips(skeleton, zmd, weapon, gender);
            string path = "Assets/Resources/Animation/" + gender.ToString() + "/" + weapon.ToString() + "/skeleton.prefab";
			AssetDatabase.CreateAsset(poses, path.Replace("skeleton.prefab", "bindPoses.asset"));
			AssetDatabase.SaveAssets();
            PrefabUtility.CreatePrefab(path, skeleton);
        }

        public void GenerateAnimationAsset(GenderType gender, RigType rig, Dictionary<String,String> zmoPaths)
        {
            GameObject skeleton = new GameObject("skeleton");
            bool male = (gender == GenderType.MALE);
            ZMD zmd = new ZMD(male ? "Assets/3DData/Avatar/MALE.ZMD" : "Assets/3DData/Avatar/FEMALE.ZMD");
            zmd.buildSkeleton(skeleton);

            BindPoses poses = ScriptableObject.CreateInstance<BindPoses>();
            poses.bindPoses = zmd.bindposes;
            poses.boneNames = getBoneNames(zmd.boneTransforms);
            poses.boneTransforms = zmd.boneTransforms;
            LoadClips(skeleton, zmd, gender, rig, zmoPaths);
            string path = "Assets/Resources/Animation/" + gender.ToString() + "/" + rig.ToString() + "/skeleton.prefab";
            AssetDatabase.CreateAsset(poses, path.Replace("skeleton.prefab", "bindPoses.asset"));
            AssetDatabase.SaveAssets();
            PrefabUtility.CreatePrefab(path, skeleton);
        }


        /// <summary>
        /// Loads all animations for given weapon type and gender. The clips are saved to Animation/{gender}/{weapon}/clips/{action}.anim 
        /// Used only in editor to generate prefabs
        /// </summary>
        /// <param name="skeleton"></param>
        /// <param name="weapon"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        public void LoadClips(GameObject skeleton, ZMD zmd, WeaponType weapon, GenderType gender)
        {
            List<AnimationClip> clips = new List<AnimationClip>();

            foreach (ActionType action in Enum.GetValues(typeof(ActionType)))
            {
                string zmoPath = Utils.FixPath(ResourceManager.Instance.GetZMOPath(weapon, action, gender));  // Assets/3ddata path
                string unityPath = "Assets/Resources/Animation/" + gender.ToString() + "/" + weapon.ToString() + "/clips/" + action.ToString() + ".anim";

                AnimationClip clip =  new ZMO("Assets/" + zmoPath).buildAnimationClip(zmd);
                clip.name = action.ToString();
                clip.legacy = true;
                clip = (AnimationClip)Utils.SaveReloadAsset(clip, unityPath, ".anim");
                clips.Add(clip);
            }

			Animation animation = skeleton.AddComponent<Animation> ();
            AnimationUtility.SetAnimationClips(animation, clips.ToArray());
        }


        public void LoadClips(GameObject skeleton, ZMD zmd, GenderType gender, RigType rig, Dictionary<String, String> zmoPaths)
        {
            List<AnimationClip> clips = new List<AnimationClip>();

            foreach (KeyValuePair<String, String> motion in zmoPaths)
            {
                string unityPath = "Assets/Resources/Animation/" + gender.ToString() + "/" + rig.ToString() + "/clips/" + motion.Key + ".anim";

                AnimationClip clip = new ZMO("Assets/" + motion.Value).buildAnimationClip(zmd);
                clip.name = motion.Key;
                clip.legacy = true;
                clip = (AnimationClip)Utils.SaveReloadAsset(clip, unityPath, ".anim");
                clips.Add(clip);
            }

            Animation animation = skeleton.AddComponent<Animation>();
            AnimationUtility.SetAnimationClips(animation, clips.ToArray());
        }


        /// <summary>
        /// Loads all animations for equiped weapon type. Used only in editor to generate prefabs
        /// </summary>
        /// <param name="WeaponType"></param>
        public void LoadAnimations(GameObject player, ZMD skeleton, WeaponType weapon, GenderType gender)
        {
            List<AnimationClip> clips = new List<AnimationClip>();

            foreach (ActionType action in Enum.GetValues(typeof(ActionType)))
            {
                // Attempt to find animation asset, and if not found, load from ZMO
                string zmoPath = Utils.FixPath(ResourceManager.Instance.GetZMOPath(weapon, action, gender));
                AnimationClip clip = R2U.GetClip(zmoPath, skeleton, action.ToString());
                clip.legacy = true;
                clips.Add(clip);
            }

            Animation animation = player.GetComponent<Animation>();
            AnimationUtility.SetAnimationClips(animation, clips.ToArray());
        }



#endif

    }
}
