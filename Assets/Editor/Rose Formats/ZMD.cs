// <copyright file="ZMD.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine, Xadet, Brett19</author>
// <date>2/25/2015 8:37 AM </date>

using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityRose.File;

namespace UnityRose.Formats
{
	/// <summary>
	/// ZMD class.
	/// </summary>
	public class ZMD 
	{
		public Transform[] boneTransforms {get; set;}
		public Matrix4x4[] bindposes {get; set;}
		private Matrix4x4[] matrices;
		public List<BoneNode> bones {get; set;}
		public List<BoneNode> dummies {get; set;}
		public int nBones { get; set; }
		public int nDummies { get; set; }
		private const float zz_scale = 0.01f;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ZMO"/> class.
		/// </summary>
		public ZMD()
		{
			
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ZMD"/> class.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		public ZMD(string filePath)
		{
			Load(filePath);
		}
		
		
		public void buildSkeleton(GameObject parent, bool renderSkeleton = false)
		{
			boneTransforms = new Transform[nBones + nDummies];
			matrices = new Matrix4x4[nBones + nDummies];
			bindposes = new Matrix4x4[nBones + nDummies];
			
			for( int i = 0; i < bones.Count; i++ )
			{
				Matrix4x4 myMat = Matrix4x4.TRS(
					bones[i].Position,
					bones[i].Rotation,
					Vector3.one);
					
				if( i == 0 )
				{
					bones[i].boneObject.transform.parent = parent.transform;
					matrices[i] = myMat;
					bones[i].Path = bones[i].Name;
				}
				else
				{
					bones[i].boneObject.transform.parent = bones[bones[i].ParentID].boneObject.transform;
					matrices[i] = matrices[bones[i].ParentID] * myMat;
					bones[i].Path = bones[bones[i].ParentID].Path + "/" + bones[i].Name;
				}
				
				bindposes[i] = matrices[i].inverse;
				bones[i].boneObject.transform.localPosition = bones[i].Position;
				bones[i].boneObject.transform.localRotation = bones[i].Rotation;
				boneTransforms[i] = bones[i].boneObject.transform;
				
				
				if( renderSkeleton && i != 0)
				{
					LineRenderer line = bones[i].boneObject.AddComponent<LineRenderer>();
					//line.material = Resources.LoadAssetAtPath<Material>("Assets/Materials/LineRenderer.mat");
					
					DrawLine script = bones[i].boneObject.AddComponent<DrawLine>();
					script.p0 = bones[bones[i].ParentID].boneObject.transform;
					script.p1 = bones[i].boneObject.transform; 
				}
				
			}	
			
			
			for(int i = 0; i < nDummies; i++)
			{
				Matrix4x4 myMat = Matrix4x4.TRS(
					dummies[i].Position,
					dummies[i].Rotation,
					Vector3.one);

				dummies[i].boneObject.transform.parent = bones[dummies[i].ParentID].boneObject.transform;
				matrices[nBones + i] = matrices[dummies[i].ParentID] * myMat;
				dummies[i].Path = bones[dummies[i].ParentID].Path + "/" + dummies[i].Name;
				
				
				bindposes[nBones + i] = matrices[nBones + i].inverse;
				dummies[i].boneObject.transform.localPosition = dummies[i].Position;
				dummies[i].boneObject.transform.localRotation = dummies[i].Rotation;
				boneTransforms[nBones + i] = dummies[i].boneObject.transform;
				
				
				if( renderSkeleton)
				{
					LineRenderer line = dummies[i].boneObject.AddComponent<LineRenderer>();
					//line.material = Resources.LoadAssetAtPath<Material>("Assets/Materials/LineRenderer.mat");
					
					DrawLine script = dummies[i].boneObject.AddComponent<DrawLine>();
					script.p0 = bones[dummies[i].ParentID].boneObject.transform;
					script.p1 = dummies[i].boneObject.transform; 
				}
			}
			
			
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
                fh = new FileHandler(asset, null);
            else
                fh = new FileHandler(filePath, FileHandler.FileOpenMode.Reading, null);

            Load();
        }

        /// <summary>
        /// Loads the specified file.
        /// </summary>
        private void Load()
        {
			// header
			int version = 2;
			string magic_number = fh.Read<ZString>();
			magic_number = magic_number.Substring(0,7);
			
			if(magic_number == "ZMD0002")
				version = 2;
			else if(magic_number == "ZMD0003")
				version = 3;
			
			
			fh.Seek(7, SeekOrigin.Begin);
			nBones = fh.Read<int>();
			bones = new List<BoneNode>();
			
			for(int i=0; i<nBones; i++)
			{
				BoneNode node = new BoneNode();
				node.BoneID = i;
				node.ParentID = fh.Read<int>();
				node.Name = fh.Read<ZString>();
				node.boneObject = new GameObject(node.Name);
				
				node.Position = new Vector3()
				{
					x = fh.Read<float>() * zz_scale,
					y = fh.Read<float>() * zz_scale,
					z = fh.Read<float>() * zz_scale
				};

                node.Position = Utils.r2uPosition(node.Position);
                node.Rotation = Utils.r2uRotation(new Quaternion()
                {
                    w = fh.Read<float>(),
                    x = fh.Read<float>(),
                    y = fh.Read<float>(),
                    z = fh.Read<float>()
                });
				
				bones.Add (node);
			}
			
			// Load Dummy bones
			dummies = new List<BoneNode>();
			nDummies  = fh.Read<int>();
			for(int i = 0; i<nDummies; i++)
			{
				BoneNode node = new BoneNode();
				node = new BoneNode();
				node.BoneID = nBones + i;
				node.Name = fh.Read<ZString>();
				node.boneObject = new GameObject(node.Name);
				node.ParentID = fh.Read<int>();
				
				node.Position = new Vector3()
				{
					x = fh.Read<float>() * zz_scale,
					y = fh.Read<float>() * zz_scale,
					z = fh.Read<float>() * zz_scale
				};

                node.Position = Utils.r2uPosition(node.Position);
				
				if(version == 3)
				{
                    node.Rotation = Utils.r2uRotation(new Quaternion()
                    {
                        w = fh.Read<float>(),
                        x = fh.Read<float>(),
                        y = fh.Read<float>(),
                        z = fh.Read<float>()
                    });
				}
				dummies.Add(node);
			}
			
			
			// add extra root bone dummy bone
			BoneNode extraDummy = new BoneNode();
			extraDummy.Name = "_p1";
			extraDummy.ParentID = 0;
			extraDummy.Position = new Vector3();
			extraDummy.Rotation = Quaternion.identity;
			extraDummy.BoneID = nBones + nDummies;
			extraDummy.boneObject = new GameObject(extraDummy.Name);
			
			dummies.Add (extraDummy);
			
			nDummies = dummies.Count;
			
			fh.Close();

            if( asset != null )
                Resources.UnloadAsset(asset);
        }
		
		public BoneNode findDummy(string name)
		{
			foreach(BoneNode dummy in dummies)
			{
				if(dummy.Name == name)
					return dummy;
			}
			
			return null;
		}
		
		public BoneNode findBone(string name)
		{
			foreach(BoneNode bone in bones)
			{
				if(bone.Name == name)
					return bone;
			}
			
			return null;
		}
	}	
}