// <copyright file="BoneNode.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine, Wahid Bouakline</author>
// <date>2/25/2015 8:37 AM </date>

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Network;
using Network.Packets;

public class LoginUI : NetworkMonoBehaviour {

    
	EventSystem system;
	private string username;
	private string password;

    private Text errorText;
    private InputField userField;
    private InputField passField;

    // Use this for initialization
    void Start () {
        base.Init();

        system = EventSystem.current;

        errorText = GameObject.Find("ErrorText").GetComponent<Text>();
        userField = GameObject.Find("usernameEdit").GetComponent<InputField>();
        passField = GameObject.Find("passwordEdit").GetComponent<InputField>();

        UserManager.Instance.registerCallback(UserOperation.LOGIN, (object obj) =>
		{
			funcQueue.Enqueue(() => {
				LoginReply packet = (LoginReply)obj;
				
				LoginStatus loginStatus = (LoginStatus)packet.response;
				
				string loginmessage = string.Empty;

				
				switch(loginStatus)
				{
					case LoginStatus.VALID:
                        errorText.text = "";
                        Application.LoadLevel("charSelect");
						break;
					case LoginStatus.NOT_EXIST:
                        userField.text = "";
                        passField.text = "";
                        errorText.text =  "Username or password invalid";
						break;
					case LoginStatus.ERROR:
                        userField.text = "";
                        passField.text = "";
                        errorText.text = "There was an error";
						break;
					default:
					break;
				}
				
			});
		});
	}
	
	// Update is called once per frame
	void Update () 
	{
		Utils.handleTab( system );
        if (Input.GetKeyDown(KeyCode.Return))
            onLogin();
        
        base.ProcessPackets();

	}
	
	public void saveUsername() {
		InputField input = system.currentSelectedGameObject.GetComponent<InputField>();
		username = input.text;
        onClearError();
    }
	
	public void savePassword() {
		InputField input = system.currentSelectedGameObject.GetComponent<InputField>();
		password = input.text;
        onClearError();
    }
	
	public void onLogin(){
		NetworkManager.Send(new LoginPacket(username, password));
	}
	
	public void onRegister(){
		Application.LoadLevel("registrationScene");
	}

    public void onClearError()
    {
        errorText.text = "";
    }
}
