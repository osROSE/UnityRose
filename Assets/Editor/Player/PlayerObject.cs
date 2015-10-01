// <copyright file="PlayerObject.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine</author>
// <date>2/25/2015 8:37 AM </date>
using UnityEngine;
using System.Collections;
using Network;
using Network.Packets;

[System.Serializable]
public struct PlayerInfo
{
	public enum Job1Type {
		PEASANT,
		HAWKER,
		SOLDIER,
		MUSE,
		DEALER,
		numJobTypes
	}
	
	public enum Job2Type {
		SCOUT,
		RAIDER,
		KNIGHT,
		CHAMPION,
		MAGE,
		CLERIC,
		ARTISAN,
		BOURGEOIS,
		numJobTypes	
	}
	
	public new string name;
	
	public int level;
	public bool male;		// true = male. false = female
	public int job1;		
	public int job2;
	
	// Base Stats (names are kept short to minimize serialization overhead)
	public float bAtkS;  	// attack speed
	public float bMovS;		// movement speed
	
	public int bHP;
	public int bMP;
	public int bSP;
	
	public int bMaxHP;
	public int bMaxMP;
	public int bMaxSP;
	
	public int bDef;		// defence
	public int bDod;		// dodge
	public int bAtk;		// Attack
	public int bInt;		// Intelligence
	public float bCritC;	// Crit chance
	public int bCritD;		// Crit damage
	
	// Total Stats (after equipment and buff modifiers)
	public float tAtkS;  	// attack speed
	public float tMovS;		// movement speed
	
	public int tHP;
	public int tMP;
	public int tSP;
	
	public int tMaxHP;
	public int tMaxMP;
	public int tMaxSP;
	
	public int tDef;		// defence
	public int tDod;		// dodge
	public int tAtk;		// Attack
	public int tInt;		// Intelligence
	public float tCritC;	// Crit chance
	public int tCritD;		// Crit damage
	
	// Armor ID's
	public int chest;
	public int arms;
	public int foot;
	public int hair;
	public int face;
	public int cap;
	public int back;
	
	// Weapon ID's
	public int weap;		
	public int shield;
	
}