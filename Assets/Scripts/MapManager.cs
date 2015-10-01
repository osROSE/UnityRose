// <copyright file="MapManager.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine</author>
// <date>2/25/2015 8:37 AM </date>


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using Network.Packets;
using UnityRose;

// This class handles the loading and desctruction of other avatars on the map
// One instance of this class is attached to each map (each map lives in a difference scene)
public class MapManager : MonoBehaviour {

	public string mapID;
	public string mainChar;
	public GameObject playerFab;
	
	private Dictionary<string, GameObject> players;
	private int numPlayers;
	
	private Queue<Packet> packetQueue;
	
	// Use this for initialization
	void Start () {
	
		players = new Dictionary< string ,GameObject>();
		packetQueue = new Queue<Packet>();
		numPlayers = 0;
		
		// Add definitions for all packet received delegates
		/*NetworkManager.instantiateCharDelegate += (InstantiateChar packet) => 
		{
			packetQueue.Enqueue( packet );
		};
		
		NetworkManager.destroyCharDelegate += (DestroyChar packet) => 
		{
			packetQueue.Enqueue( packet );
		};*/
		
		
	}
	
	void OnDestroy()
	{
		foreach( GameObject player in players.Values)
		{
			Destroy( player );
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if( packetQueue.Count > 0)
		{
			Packet packet = packetQueue.Dequeue();
			
			switch( (PacketType)packet.type )
			{
				case PacketType.CHARACTER:
					switch( (CharacterOperation)packet.operation )
					{
						case CharacterOperation.INSTANTIATE:
							InstantiateChar ic = (InstantiateChar) packet;
							if( !ic.isMain )
							{
								GameObject newPlayer = (GameObject)Instantiate( playerFab, ic.pos, ic.rot);
								
								try{
									players.Add( ic.clientID, newPlayer );
								}catch(Exception e){
									Debug.LogWarning ("Player already exists in map. Destroying it...");
									Destroy( newPlayer );
									break;
								}
								
								// TODO: add all other player initialization specifications here based on packet
								newPlayer.name = ic.clientID; // + " " + numPlayers;
								
								PlayerController playerController = newPlayer.GetComponent<PlayerController>();
								playerController.isMainPlayer = false;
								playerController.playerInfo = ic.pInfo;
								players[ic.clientID] = newPlayer;
							}
							break;
						case CharacterOperation.DESTROY:
							DestroyChar dc = (DestroyChar) packet;
							try{
								Destroy(players[dc.clientID]);
								players.Remove(dc.clientID);
							}catch(Exception e){
								Debug.LogError("Failed to remove player" + dc.clientID + " from map.");
								Debug.LogException(e);
							}
							break;
						default:
							break;
					}
					break;
				default:
					break;
			}
		}
	}
}
