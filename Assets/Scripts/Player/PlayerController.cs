// <copyright file="PlayerController.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <authors>Wadii Bellamine, Kiu</authors>
// <date>2/25/2015 8:37 AM </date>

using UnityEngine;
using System;
using System.Collections.Generic;
using Network.Packets;
using Network;
using UnityEngine.EventSystems;

namespace UnityRose
{

    public class PlayerController : NetworkMonoBehaviour
    {
    	public bool isMainPlayer = false;
		public PlayerInfo playerInfo;
		public GameObject cursor;
		public bool VR = false;
		
        private int floorMask;
        private float camRayLength = 500f;
        private Vector3 destinationPosition;
        private CharacterController controller;
		private State animationStateMachine;
        private bool isWalking = false;
        private States state = States.STANDING;
		public RosePlayer rosePlayer;
		
		void Awake()
		{
			Application.targetFrameRate = 60;
		}
		
        // Use this for initialization
        void Start()
        {	
			floorMask = LayerMask.GetMask("Floor")|LayerMask.GetMask("MapObjects");
            controller = this.gameObject.GetComponent<CharacterController>();
            destinationPosition = transform.position;
			playerInfo.name = this.name;

            base.Init();
           
           /*
            // Add definitions for all packet received delegates  
			CharacterManager.Instance.registerCallback(CharacterOperation.GROUNDCLICK, (object obj) => {
				
				funcQueue.Enqueue(() => {
					GroundClick packet = (GroundClick)obj;
					
					if(packet.clientID == playerInfo.name )//&& !isMainPlayer)
					{
						destinationPosition = packet.pos;
					}
				});
			});
			*/
					
            
            
			if( isMainPlayer )
			{
				// Tell server main player has finished loading
				NetworkManager.Send( new CharLoadCompleted( playerInfo.name ) ); 
			}

        }


        public void SetAnimationStateMachine(RigType rig, States initialState)
        {
            state = initialState;
            switch (rig)
            {
                case RigType.FOOT:
                    animationStateMachine = new PlayerState(initialState, "Player State Machine", this.gameObject);
                    break;
                case RigType.CHARSELECT:
                    animationStateMachine = new CharSelectState(initialState, "Char Select State Machine", this.gameObject);
                    break;
                default:
                    break;
            }

            animationStateMachine.Entry();
        }

        public void SetAnimationState(States state)
        {
            this.state = state;
			if (animationStateMachine != null)
				animationStateMachine.Evaluate(state);
        }

		public void OnSkeletonChange()
		{
            // Persist current rig and animation state
            SetAnimationStateMachine(rosePlayer.charModel.rig, animationStateMachine.state);
		}


		public void OnChangeEquip(BodyPartType bodyPart, int id)
		{
			rosePlayer.equip (bodyPart, id);
		}
		
		
        // Update is called once per frame
        void Update()
        {
            base.ProcessPackets();

            // Only handle walking movement if the correct rig is used
            if(rosePlayer.charModel.rig == RigType.FOOT)
            {

                // Only take input if this player is the main player
                if ( this.isMainPlayer )
			    {
				    // TODO: remove this after debugging is over
				    if( Input.GetKeyDown( KeyCode.J ) )
				    {
					    // this packet is reflected to all clients by server (for debugging only)
					    NetworkManager.Send( new InstantiateChar(  false, gameObject.transform.position, gameObject.transform.rotation, playerInfo ) ); //gameObject.name, gameObject.transform.position, gameObject.transform.rotation ));
				    }
				
				
				    bool locate = false;
				    switch (Application.platform)
				    {
					    case RuntimePlatform.IPhonePlayer:
					    case RuntimePlatform.Android:
					    case RuntimePlatform.WP8Player:
						    locate = Input.touchCount > 0;
					    break;
					    default:
						    locate = Input.GetMouseButton(0);
					    break;
					
				    }
				
				    if ( locate )
	            	    LocatePosition();
        	    }	
            

                MoveToPosition();

            
                // TODO: use character state packets to control state
                if (isWalking)
                    state = States.RUN;
                else
                    state = States.STANDING;
            }

            // Evaluate the animation state machine
            if (animationStateMachine != null)
                animationStateMachine.Evaluate(state);

        }


        void LocatePosition()
        {
			Vector2 screenPoint;
			bool fire = false;
			switch (Application.platform)
        	{
        		case RuntimePlatform.IPhonePlayer:
        		case RuntimePlatform.Android:
        		case RuntimePlatform.WP8Player:
					screenPoint = Input.GetTouch(0).position;
					fire = (Input.GetTouch(0).tapCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Ended);
					break;
				default:
					screenPoint = Input.mousePosition;
					fire = Input.GetMouseButtonDown(0);
					break;
			
			}
			
             Ray camRay = Camera.main.ScreenPointToRay( screenPoint );
             RaycastHit floorHit;

			if( fire )
			{
				if(VR)
					destinationPosition = cursor.transform.position;
				// Perform the raycast and if it hits something on the floor layer...
				else if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
				 	destinationPosition = floorHit.point;
				 	
				// Send a clicked on ground packet
				NetworkManager.Send( new GroundClick( gameObject.name, destinationPosition ));
				//destinationPosition = transform.position; // TODO: restore this
			
            }
        }
		
        void MoveToPosition() 
        {
            if ( Vector3.Distance( transform.position , destinationPosition ) > 0.5f )
            {
                Vector3 playerToMouse = destinationPosition - transform.position;
                playerToMouse.y = 0;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
                transform.rotation = newRotation;
				
				if(VR)
				{
					// TODO: use a smooth rotation
					GameObject cameraObject = GameObject.Find("Main Camera");
					Vector3 camRotationEuler = cameraObject.transform.rotation.eulerAngles;
					Vector3 newRotationEuler = newRotation.eulerAngles;
					
					Quaternion newCamRotation = Quaternion.Euler( camRotationEuler.x, newRotationEuler.y, newRotationEuler.z);
					cameraObject.transform.rotation = newRotation;
					
					if( isWalking == false )
						Cardboard.SDK.Recenter();
				}
				
                //check hangout
                controller.SimpleMove(transform.forward * playerInfo.tMovS);
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }         
        }

    }
}

