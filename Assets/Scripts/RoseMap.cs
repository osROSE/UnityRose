// <copyright file="RoseMap.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine</author>
// <date>2/25/2015 8:37 AM </date>

// ------------------------------------------------------------------------------
//  <authors>
//      Wadii Bellamine
//      3/14/2014
//  </authors>
// ------------------------------------------------------------------------------
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityRose.Game 
{
	public class RoseMap
	{
		public List<RosePatch> m_patches { get; set; }
		public DirectoryInfo m_assetDir { get; set; }
		public string m_name { get; set; }
		// Default contsructor
		public RoseMap()
		{
		}
		
		// Functional constructor 1
		public RoseMap (DirectoryInfo assetDir)
		{
			this.m_assetDir = assetDir;
			m_name = assetDir.Name;
			foreach(DirectoryInfo dir in assetDir.GetDirectories())
			{
				RosePatch patch = new RosePatch(dir);
				if(patch.m_isValid)
					m_patches.Add (patch);
			}
		}
	}
}

#endif
