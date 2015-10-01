// <copyright file="RegisterUI.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine, Wahid Bouakline</author>
// <date>2/25/2015 8:37 AM </date>

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Network;
using Network.Packets;

public class RegisterUI : NetworkMonoBehaviour {

	EventSystem system;
	
	GameObject messageBox;
	
	public Button registerButton;
    public Text errorText;
	public Color errorColor;
	public int minPwLength;
	public char[] invalidUserNameChars;
	public char[] invalidPasswordChars;
	public char[] invalidEmailChars;
	
	private string username;
	private string email;
	private string password;
	
	private bool userNameValid;
	private bool passwordValid;
	private bool passwordMatchValid;
	private bool emailValid;
    private bool success;

	// Use this for initialization
	void Start () {
        base.Init();
		system = EventSystem.current;
		registerButton.interactable = false;
		userNameValid = false;
		passwordValid = false;
		passwordMatchValid = false;
		emailValid = false;
		
		UserManager.Instance.registerCallback(UserOperation.REGISTER, (object obj) =>
		{
            funcQueue.Enqueue(() => {
            
                RegisterReply packet = (RegisterReply)obj;
			
			    RegisterResponse registrationResponse = (RegisterResponse)packet.response;
			
			    switch(registrationResponse)
			    {
				    case RegisterResponse.VALID:
                        // TODO: show a message box telling them the registration was successful
                        errorText.color = Color.green;
                        errorText.text = "Registration successful!";
                        Application.LoadLevel("loginScene");
                        break;
				
				    case RegisterResponse.EMAIL_INVALID:
                        errorText.text = "Invalid email";
                        break;
                    case RegisterResponse.EMAIL_USED:
                        errorText.text = "Email already used";
                        break;
                    case RegisterResponse.PASSWORD_TOO_SHORT:
                        errorText.text = "Password too short";
                        break;
                    case RegisterResponse.USERNAME_BAD_CHARS:
                        errorText.text = "Username contains invalid characters";
                        break;
                    case RegisterResponse.USERNAME_EXISTS:
                        errorText.text = "Username already taken";
                        break;
                    case RegisterResponse.USERNAME_TOO_SHORT:
                        errorText.text = "Username is too short. Must be at least 6 characters.";
                        break;
                    default:
                        errorText.text = "An unknown error occurred: " + (int)registrationResponse;
					    break;
			    }

            }); // End Enqueue

        }); // End registerCallback
	}
	
	// Update is called once per frame
	void Update () {
		Utils.handleTab( system );

        base.ProcessPackets();

        if ( userNameValid &&  passwordValid && passwordMatchValid && emailValid )
			registerButton.interactable = true;
		else
			registerButton.interactable = false;
			
		if (Input.GetKeyDown(KeyCode.Return) && registerButton.interactable)
			registerBtn();

	}
	
    public void onClearError()
    {
        errorText.color = Color.red;
        errorText.text = "";
    }

	public void backBtn() {
		Application.LoadLevel("loginScene");
	}
	
	public void registerBtn(){
		NetworkManager.Send(new RegisterPacket(username, email, password));
	}
	
	public void validateField(ref bool fieldValid, bool valid, InputField input) 
	{
		ColorBlock colors = input.colors;
		
		if( valid ) {
			colors.highlightedColor = colors.normalColor;
		}
		else {
			colors.highlightedColor = errorColor;
		}
		
		fieldValid = valid;
		
		input.colors = colors;
	}
	
	public void validateUsername()
	{
		InputField input = system.currentSelectedGameObject.GetComponent<InputField>();
		username = input.text;
		validateField(ref userNameValid, username.IndexOfAny( invalidUserNameChars ) == -1, input);
	}
	
	public void validatePassword(){
		InputField input = system.currentSelectedGameObject.GetComponent<InputField>();
		password = input.text;
		validateField(ref passwordValid, password.IndexOfAny( invalidPasswordChars ) == -1, input);
	}
	
	public void validatePasswordFinish(){
		InputField input = system.currentSelectedGameObject.GetComponent<InputField>();
		password = input.text;
		validateField(ref passwordValid, password.Length >= minPwLength, input);
	}
	
	public void validatePasswordMatch(){
		InputField input = system.currentSelectedGameObject.GetComponent<InputField>();
		string text = input.text;
		validateField(ref passwordMatchValid, text == password, input);
	}
	
	public void validateEmail(){
		InputField input = system.currentSelectedGameObject.GetComponent<InputField>();
		email = input.text;
		validateField(ref emailValid, Utils.isEmail(email), input);
	}
}
