// <copyright file="BoneNode.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine</author>
// <date>2/25/2015 8:37 AM </date>

using UnityEngine;
using System.Collections.Generic;

	
public class BoneNode
{
	public string Name {get; set;}
	public string Path {get; set;}
	public int BoneID {get; set;}
	
	public Vector3 Position {get; set;}
	public Quaternion Rotation {get; set;}
	public int ParentID {get; set;}
	public BoneNode parent {get; set;}
	public List<BoneNode> children {get; set;}
	public GameObject boneObject {get; set;}
	/*public BoneNode()
	{
		Rotation = Quaternion.identity;
		parent = null;
	} */
	
	public void RenderSkeleton()
	{
		renderSkeleton(this);
	}
	
	private void renderSkeleton(BoneNode parent)
	{
		if(parent.children != null)
		{
			foreach(BoneNode child in parent.children)
			{
				Debug.DrawLine(parent.boneObject.transform.localPosition, child.boneObject.transform.localPosition, Color.green, 2f, false);	
				renderSkeleton (child);
			}
		}
	}
		
}

