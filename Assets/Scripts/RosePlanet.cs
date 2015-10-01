// <copyright file="RosePlanet.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine</author>
// <date>2/25/2015 8:37 AM </date>

// ------------------------------------------------------------------------------
//  <authors>
//      Wadii Bellamine
//      3/13/2014
//  </authors>
// ------------------------------------------------------------------------------
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityRose.Game 
{
	public class RosePlanet
	{
		public List<RoseMap> m_maps { get; set; }
		public DirectoryInfo m_assetDir { get; set; }
		
		// Default contsructor
		public RosePlanet()
		{
		}
		
		// Functional constructor 1
		public RosePlanet (DirectoryInfo assetDir)
		{
			this.m_assetDir = assetDir;
		}
	}
}

#endif

