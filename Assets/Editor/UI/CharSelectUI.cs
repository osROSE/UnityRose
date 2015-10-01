using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using Network;
using Network.Packets;
using System.Linq;

namespace UnityRose
{
    public class CharSelectUI : NetworkMonoBehaviour
    {
        public Transform[] playerPositions;
        public GameObject newCharPanel;
        private Dictionary<RosePlayer, int> players;
        private RosePlayer currentPlayer;

        // Use this for initialization
        void Start() {
            base.Init();
            newCharPanel = GameObject.Find("NewCharPanel");
            newCharPanel.SetActive(false);

            players = new Dictionary<RosePlayer, int>();
			
			// Register for incoming createchar packets
			
			// Server tells us to load a character
            UserManager.Instance.registerCallback(UserOperation.CHARSELECT, (object obj) =>
            {
                funcQueue.Enqueue(() => {
                    CharSelectPacket packet = (CharSelectPacket)obj;
                    onLoad(packet.charModel);
                });
            });

            // Server tells us that we are clear to start playing with the chosen character
            UserManager.Instance.registerCallback(UserOperation.SELECTCHAR, (object obj) =>
            {
                funcQueue.Enqueue(() => {
                    CharSelectPacket packet = (CharSelectPacket)obj;
                    CharSelectResponse response = (CharSelectResponse)packet.status;
                    if (response == CharSelectResponse.SUCCESS)
                        Application.LoadLevel(packet.name);

                });
            });

            // Server tells us our new char request is valid
            UserManager.Instance.registerCallback(UserOperation.CREATECHAR, (object obj) =>
			{
				funcQueue.Enqueue(() => {
					CharSelectPacket packet = (CharSelectPacket)obj;
					
					CharSelectResponse response = (CharSelectResponse)packet.status;
					
					switch( response )
					{
						case CharSelectResponse.SUCCESS:
							onNetworkCreate();
							break;
						case CharSelectResponse.NAME_EXISTS:
							Debug.Log ("TODO: name already exists");
							break;
						case CharSelectResponse.INVALID_CHOICE:
							Debug.Log ("TODO: choices are not valid");
							break;
						case CharSelectResponse.ERROR:
							Debug.Log ("TODO: an internal error has occurred");
							break;
						default:
							Debug.Log ("TODO: a communication error has occurred");
							break;
					}
				});
			});

            // Tell the server we are ready to recieve char select packets
            NetworkManager.Send(new CharSelectPacket());

        }

        // Update is called once per frame
        void Update()
        {
            base.ProcessPackets();

            // Ignore clicks if the NewCharPanel is active
            if (newCharPanel.active)
                return;

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

            if (locate)
                LocatePlayer();
        }


        public void onLoad(CharModel charModel) {
            // Do nothing if we already reached the max number of chars
            if (players.Count >= 5)
                return;

            // Generate a default player
            charModel.rig = RigType.CHARSELECT;
            charModel.state = States.SITTING;
            RosePlayer player = new RosePlayer(charModel);
            int id = players.Count;
            Vector3 altarPos = playerPositions[id].position;
            player.player.transform.position = new Vector3(altarPos.x, altarPos.y + 2.1f, altarPos.z);
            player.player.transform.LookAt(Camera.main.transform);
            players.Add(player, id);
            currentPlayer = player;

        }

        public void onStart() {
            if (currentPlayer == null)
                return;

            NetworkManager.Send(new CharSelectPacket(UserOperation.SELECTCHAR, currentPlayer.charModel.name));

        }

        public void onNew() {
            // Do nothing if we already reached the max number of chars
            if (players.Count >= 5)
                return;

            // If a character has been selected already, make them sit the fuck down
            if (currentPlayer != null)
                currentPlayer.setAnimationState(States.SIT);

            // Bring up the new char panel
            newCharPanel.SetActive(true);

            // Generate a default player
            CharModel charModel = new CharModel();
            charModel.rig = RigType.CHARSELECT;
            charModel.state = States.HOVERING;
            RosePlayer player = new RosePlayer(charModel);
            

            // Put new player in front of the new player camera and facing it
            Transform newCharCam = GameObject.Find("NewCharCam").transform;
            player.player.transform.position = new Vector3(0.0f, -1.2f, 3.0f);
            player.player.transform.Rotate(new Vector3(0, 180, 0));
            //player.player.transform.LookAt(newCharCam);
            currentPlayer = player;

        }

        private int findFirstEmptySlot()
        {
            // Rely on exception to know which slot has no player
            for (int i = 0; i < 5; i++)
            { 
                if (!players.ContainsValue(i))
                    return i;
            }
            
            return 5;
        }

        public void onCreate()
        {
            // Tell the server that we have created a new char
            NetworkManager.Send(new CharSelectPacket(currentPlayer.charModel));

        }

		// Server response to onCreate()
        public void onNetworkCreate()
        {
			// Hide the new char panel
			newCharPanel.SetActive(false);
			
			// Put the player on the approriate altar
			Vector3 altarPos = playerPositions[findFirstEmptySlot()].position;
			
			int id = findFirstEmptySlot();
			players.Add(currentPlayer, id);
			
			currentPlayer.player.transform.position = new Vector3(altarPos.x, altarPos.y + 2.1f, altarPos.z);
			currentPlayer.player.transform.LookAt(Camera.main.transform);
			
			// Make them stand 
			currentPlayer.setAnimationState(States.STANDING);
        }
		
		// Cancel char creation
        public void onCancel()
        {
            // Destroy the temporary char
            players.Remove(currentPlayer);
            currentPlayer.Destroy();
            currentPlayer = players.Keys.ElementAt(players.Count - 1);

            // Hide the new char panel
            newCharPanel.SetActive(false);
        }

		// Delete the current player
        public void onDelete()
        {
            if (currentPlayer != null)
            {
            	// Tell the server
				NetworkManager.Send(new CharSelectPacket(UserOperation.DELETECHAR, currentPlayer.charModel.name));
                // Destroy the current player
                players.Remove(currentPlayer);
                currentPlayer.Destroy();
                currentPlayer = (players.Count > 0) ? players.Keys.ElementAt(players.Count - 1) : null;
            }
        }   

        public void onNameChange()
        {
            currentPlayer.changeName(GameObject.Find("CharNameSection").transform.FindChild("TextBox").transform.FindChild("Text").GetComponent<Text>().text);
        }
        private static GenderType[] genderSelections = { GenderType.MALE, GenderType.FEMALE };
        private int currentGenderSelection = 0;

        public void onNextGender()
        {
            currentGenderSelection = (currentGenderSelection + 1) % genderSelections.Length;
            currentPlayer.changeGender(genderSelections[currentGenderSelection]);
            GameObject.Find("GenderSection").transform.FindChild("Value").GetComponent<Text>().text = genderSelections[currentGenderSelection].ToString().ToLowerInvariant();
        }

        public void onPreviousGender()
        {
            currentGenderSelection--;
            if (currentGenderSelection == -1)
                currentGenderSelection = genderSelections.Length - 1;
            currentGenderSelection = currentGenderSelection % genderSelections.Length;
            currentPlayer.changeGender(genderSelections[currentGenderSelection]);
            GameObject.Find("GenderSection").transform.FindChild("Value").GetComponent<Text>().text = genderSelections[currentGenderSelection].ToString().ToLowerInvariant();
        }

        private static int[] hairSelections = { 0, 5, 10, 15, 20, 25, 30 };
        private int currentHairSelection = 0;

        public void onNextHair()
        {
            currentHairSelection = (currentHairSelection + 1) % hairSelections.Length;
            currentPlayer.equip(BodyPartType.HAIR, hairSelections[currentHairSelection]);
            GameObject.Find("HairSection").transform.FindChild("Value").GetComponent<Text>().text = (currentHairSelection+1).ToString();
        }

        public void onPreviousHair()
        {
            currentHairSelection--;
            if (currentHairSelection == -1)
                currentHairSelection = hairSelections.Length-1;
            currentHairSelection = currentHairSelection % hairSelections.Length;
            currentPlayer.equip(BodyPartType.HAIR, hairSelections[currentHairSelection]);
            GameObject.Find("HairSection").transform.FindChild("Value").GetComponent<Text>().text = (currentHairSelection+1).ToString();
        }

        private static int[] faceSelections = { 1, 8, 15, 22, 29, 36, 43, 50, 57, 64, 71, 78, 85, 92 };
        private int currentFaceSelection = 0;

        public void onNextFace()
        {
            currentFaceSelection = (currentFaceSelection + 1) % faceSelections.Length;
            currentPlayer.equip(BodyPartType.FACE, faceSelections[currentFaceSelection]);
            GameObject.Find("FaceSection").transform.FindChild("Value").GetComponent<Text>().text = (currentFaceSelection+1).ToString();
        }

        public void onPreviousFace()
        {
            currentFaceSelection--;
            if (currentFaceSelection == -1)
                currentFaceSelection = faceSelections.Length - 1;
            currentFaceSelection = currentFaceSelection % faceSelections.Length;
            currentPlayer.equip(BodyPartType.FACE, faceSelections[currentFaceSelection]);
            GameObject.Find("FaceSection").transform.FindChild("Value").GetComponent<Text>().text = (currentFaceSelection + 1).ToString();
        }

        
        void LocatePlayer()
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

            Ray camRay = Camera.main.ScreenPointToRay(screenPoint);
            RaycastHit playerHit;

            if (fire)
            {
                // Perform the raycast and if it hits something on the floor layer...
                if (Physics.Raycast(camRay, out playerHit, 500.0f))//, LayerMask.NameToLayer("Players")))
                    OnPointerClick(playerHit.transform);
            }
        }


        void OnPointerClick(Transform clickedTransform)
        {
            PlayerController controller = clickedTransform.gameObject.GetComponent<PlayerController>();
            if (controller != null)
            {
                RosePlayer clickedPlayer = controller.rosePlayer;
                clickedPlayer.setAnimationState(States.STANDUP);
                if (clickedPlayer != currentPlayer)
                {
                    if(currentPlayer != null)
                        currentPlayer.setAnimationState(States.SIT);
                    currentPlayer = clickedPlayer;
                }

            }
        }

        void OnPointerDoubleClick(Transform clickedTransform)
        {
            PlayerController controller = clickedTransform.gameObject.GetComponent<PlayerController>();
            if (controller != null)
            {
                RosePlayer clickedPlayer = controller.rosePlayer;
                if (clickedPlayer != currentPlayer)
                {
                    clickedPlayer.setAnimationState(States.SELECT);
                    currentPlayer.setAnimationState(States.SIT);
                    currentPlayer = clickedPlayer;
                }
            }
        }
    }
}
