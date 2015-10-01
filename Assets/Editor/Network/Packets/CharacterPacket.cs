// <copyright file="CharacterPacket.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine</author>
// <date>2/25/2015 8:37 AM </date>

using UnityEngine;
using System.Collections;
using JsonFx.Json;


namespace Network.Packets
{
	public enum CharacterOperation
	{
		DEFAULT,
		GROUNDCLICK,
		CHANGEDSTATE,
		INSTANTIATE,
		DESTROY,
		LOADCOMPLETE,
		numCharOperations
	}
	
	// All character packets sent are checked against clientID associated with socket connection
	[JsonOptIn]
	public class CharacterPacket: Packet
	{
		[JsonMember]
		public string clientID {get; set;}
		
		public CharacterPacket()
		{
			type = (int)PacketType.CHARACTER;
			operation = (int)CharacterOperation.DEFAULT;
		}
		
	}

	// Two way packet for sending/receiving ground click events
	[JsonOptIn]
	public class GroundClick: CharacterPacket
	{
		[JsonMember]
		public Vector3 pos {get; set;}
		
		public GroundClick()
		{
		}
		
		public GroundClick(string clientID, Vector3 position)
		{
			this.clientID = clientID;
			this.pos = position;
			
			operation = (int)CharacterOperation.GROUNDCLICK;
		}
		
	}
	
	// Client -> Server packet for player load completed
	[JsonOptIn]
	public class CharLoadCompleted: CharacterPacket
	{	
		public CharLoadCompleted()
		{
		}
		
		public CharLoadCompleted(string clientID)
		{
			operation = (int)CharacterOperation.LOADCOMPLETE;
			this.clientID = clientID;
		}
		
	}
	
	// Server -> Client packet for initializing a new character
	[JsonOptIn]
	public class InstantiateChar: CharacterPacket
	{
		[JsonMember]
		public bool isMain { get; set; }
		
		[JsonMember]
		public Vector3 pos {get; set;}
		
		[JsonMember]
		public Quaternion rot {get; set;}
		
		[JsonMember]
		public PlayerInfo pInfo { get; set; }
		
		public InstantiateChar()
		{
		}
		
		public InstantiateChar( bool isMain, Vector3 position, Quaternion rotation, PlayerInfo info)
		{
			operation = (int)CharacterOperation.INSTANTIATE;
			this.isMain = isMain;
			this.clientID = info.name;
			this.pos = position;
			this.rot = rotation;
			this.pInfo = info;
		}
		
	}
	
	[JsonOptIn]
	public class DestroyChar: CharacterPacket
	{		
		//Todo: add members for armor, speed, etc
		
		public DestroyChar()
		{
		}
		
		public DestroyChar(string clientID)
		{
			this.clientID = clientID;
			operation = (int)CharacterOperation.DESTROY;
		}
		
	}
	
	
	[JsonOptIn]
	public class ChangedState: CharacterPacket
	{
		[JsonMember]
		public string state {get; set;}
		
		public ChangedState(string clientID, string state)
		{
			this.clientID = clientID;
			this.state = state;
			operation = (int)CharacterOperation.CHANGEDSTATE;
		}
		
	}
}

