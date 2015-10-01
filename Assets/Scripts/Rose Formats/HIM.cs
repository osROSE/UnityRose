// <copyright file="HIM.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine, Xadet, Bret19</author>
// <date>2/25/2015 8:37 AM </date>

using UnityEngine;
using System.Collections;
using UnityRose.File;

namespace UnityRose.Formats
{
	public class HIM
	{
		public int Length{ get; set; }
		public int Width{ get; set; }
		public float MinHeight{ get; set;}
		public float MaxHeight{ get; set;}
		public float[,] Heights{ get; set; }
		public int GridCount { get; set; }
		public float GridSize { get; set; }
		
		public HIM(string file)
		{
			FileReader fr = new FileReader ( file );
			
			Length = fr.Read<int> ();
			Width = fr.Read<int> ();
			GridCount = fr.Read<int> ();
			GridSize = fr.Read<float> ();
			
			//fr.BaseStream.Seek (8, System.IO.SeekOrigin.Current);
			
			Heights = new float[Length, Width];
			MinHeight = 10000000000000000000.0f;
			MaxHeight = 10000.0f;
			
			
			for (int y = 0; y < Length; ++y) 
			{
				for (int x = 0; x < Width; ++x) 
				{
					Heights [y, x] = fr.Read<float> ();
					if (Heights [y, x] < MinHeight)
						MinHeight = Heights [y, x];
					if (Heights [y, x] > MaxHeight)
						MaxHeight = Heights [y, x];
				}
			}
			
			fr.Close ();
		}
		
		/*
		public float[,] GetHeightsForExport(float minHeight)
		{
			for (int y = 0; y < Length; ++y) {
				for (int x = 0; x < Width; ++x) {
					heights [y, x] += -minHeight;
					heights [y, x] = heights [y, x] / maxHeight;
				}
			}
		}
		*/
		
			
	}
}

