using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Network;
using Network.Packets;

public class registerGUI : MonoBehaviour {

	public Button registerBtn;
	
	public InputField usernameInput;
	
	public InputField passwordInput;
	
	public InputField emailInput;
	
	public const string MatchEmailPattern =
		@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
			+ @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
			+ @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
			+ @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
			
	public static bool IsEmail(string email)
	{
		if (email != null) 
			return Regex.IsMatch(email, MatchEmailPattern);
		else 
			return false;
	}
	
	private bool validateRegister(string username, string email, string password) 
	{
		return (username != string.Empty && username.Length >= 5) &&
				(email != string.Empty && IsEmail(email)) &&
				(password != string.Empty && password.Length >= 6);
	}
	
	// Use this for initialization
	void Start () 
	{
	
		registerBtn.onClick.AddListener(delegate {
			string username = usernameInput.text;
			string email = emailInput.text;
			string password = passwordInput.text;
			
			if(validateRegister(username, email, password)) 
			{
				NetworkManager.Send(new RegisterPacket(username, email, password));
			}
		});
		
		// Add packet received delegates
		// Registration response
		/*NetworkManager.registerReplyDelegate += (RegisterReply packet) => 
		{
			RegisterResponse response = (RegisterResponse) packet.response;
			
			switch( response )
			{
				case RegisterResponse.SUCCESS:
					Debug.Log ("Registration successful!");
					break;
				case RegisterResponse.USEREXISTS:
					Debug.Log("Username already taken. Try a different one.");
					break;
				case RegisterResponse.ERROR:
					Debug.Log("Error while registrating");
					break;
				case RegisterResponse.EMAIL_USED:
					Debug.Log ("Email already USED.");
					break;
;
				case RegisterResponse.USERINVALID_TOOSHORT:
					Debug.Log ("Username need to be at least 5 characters");
					break;
				case RegisterResponse.USERINVALID_BADCHARS:
					Debug.Log ("Username contains invalid characters.");
					break;
				case RegisterResponse.PASSWORD_TOOSHORT:
					Debug.Log ("Password is too short");
					break;
				default:
					Debug.Log ("Unknown registration response");
					break;
			}
			
		};*/
	}
}
