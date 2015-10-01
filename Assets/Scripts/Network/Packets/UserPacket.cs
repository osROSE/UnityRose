using UnityEngine;
using System.Collections;
using JsonFx.Json;
using UnityRose;


namespace Network.Packets
{
	public enum UserOperation
	{
		REGISTER = 0,
		LOGIN,
		CHARSELECT, // two-way: tells server char select finished loading. Tells us to spawn a char
        SELECTCHAR, // tells server which char has been selected for entering game
        CREATECHAR, // two-way: tells server we created a char. Tells client to accept that char.
        DELETECHAR, // tells server we have deleted a char
	}

    public enum CharSelectOperation
    {
        CREATE = 0,
        DELETE = 1,
        SELECT = 2
    }
	
	public enum LoginStatus
	{
		ERROR = 0,
		NOT_EXIST = 1,
		VALID = 2,
		INPUT_EMPTY = 3
	}
	
	public enum RegisterResponse
	{
		ERROR = 0,
		VALID,
		USERNAME_TOO_SHORT,
		USERNAME_BAD_CHARS,
		USERNAME_EXISTS,
        PASSWORD_TOO_SHORT,
		EMAIL_USED,
		EMAIL_INVALID,
	}

    public enum CharSelectResponse
    {
        ERROR = 0,
        SUCCESS,
        NAME_EXISTS,
        INVALID_CHOICE
    }

    // All user packets must derive from this one
    public class UserPacket : Packet
    {
        public UserPacket()
        {
            type = (int)PacketType.USER;
        }
    }

    ///////////////// Client -> Server packets //////////////////////
    public class RegisterPacket: UserPacket 
	{
        [JsonMember]
        public string username { get; set; }
        [JsonMember]
        public string password { get; set; }
        [JsonMember]
        public string email { get; set; }
		
		public RegisterPacket()
		{
            
		}
		
		public RegisterPacket(string username, string email, string password)
		{
			this.username = username;
			this.password = password;
			this.email = email;
			operation = (int)UserOperation.REGISTER;
		}
	}
	
	
	// This packet is used by the user to login to the char select scene
	public class LoginPacket: UserPacket
	{
        [JsonMember]
        public string username { get; set; }
        [JsonMember]
        public string password {get; set; }
		
		public LoginPacket()
		{
		
		}
		
		public LoginPacket(string username, string password)
		{
			this.username = username;
			this.password = password;
			operation = (int)UserOperation.LOGIN;
		}
		
	}

    ///////////////// Server -> Client packets //////////////////////
    public class LoginReply: Packet
	{
		[JsonMember]
		public int response {get; set;}
		
		public LoginReply()
		{
			type = (int)PacketType.USER;
			operation = (int)UserOperation.LOGIN;
		}

	}
	
	
	public class RegisterReply: Packet
	{
		[JsonMember]
		public int response { get; set; }
		
		
		public RegisterReply()
		{
			type = (int)PacketType.USER;
			operation = (int)UserOperation.REGISTER;
		}
	}


    ///////////////// Two-way packets //////////////////////

    // This packet is sent by the client after creating, deleting, or choosing a character in the char select scene
    public class CharSelectPacket : UserPacket
    {
        [JsonMember]
        public int status { get; set; }

        [JsonMember]
        public CharModel charModel { get; set; }

        [JsonMember]
        public string name { get; set; }

        public CharSelectPacket()
        {
        	operation = (int)UserOperation.CHARSELECT;
        }

        public CharSelectPacket(UserOperation op, string name)
        {
            operation = (int)op;
            this.name = name;
        }

        public CharSelectPacket(CharModel charModel)
        {
            operation = (int)UserOperation.CREATECHAR;
            this.charModel = charModel;
        }
    }
}

