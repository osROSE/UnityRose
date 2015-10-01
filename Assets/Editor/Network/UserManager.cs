using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network;
using Network.Packets;
using Network.JsonConverters;
using JsonFx.Json;

namespace Network
{
	public class UserManager : IPacketManager
	{
		private static UserManager instance;
		
		private Dictionary<UserOperation, Action<object>> callbacks;
		
		public UserManager()
		{
			callbacks = new Dictionary<UserOperation, Action<object>>();
		}
		
		public void registerCallback(UserOperation operation, Action<object> callback) {
			this.registerCallback((int)operation, callback);
		}
		
		public void registerCallback(int operation, Action<object> callback) {
			
			UserOperation userOperation = (UserOperation)operation;
			
			if(callbacks.ContainsKey(userOperation)) {
				callbacks[userOperation] = callback;
			} else {
				callbacks.Add(userOperation, callback);
			}
		}
		
		public void handlePacket(int operation, string packet) {
			UserOperation key = (UserOperation)operation;
			
			if(callbacks.ContainsKey(key)) {
				Action<object> callback = callbacks[key];
				object _packet = this.Convert(operation, packet);
				callback.Invoke(_packet);
			}
		}
		
		public object Convert(int operation, string packet) 
		{
			object _packet = null;
			UserOperation userOperation = (UserOperation)operation;
			
			JsonReader jsonReader = new JsonReader(packet);
			
			switch(userOperation) 
			{
				case UserOperation.LOGIN:
					_packet = jsonReader.Deserialize<LoginReply>();
					break;
				case UserOperation.REGISTER:
					_packet = jsonReader.Deserialize<RegisterReply>();
					break;
                case UserOperation.CHARSELECT:
                case UserOperation.CREATECHAR:
                case UserOperation.DELETECHAR:
                case UserOperation.SELECTCHAR:
                    _packet = jsonReader.Deserialize<CharSelectPacket>();
                    break;
                default:
					break;
			}
			
			return _packet;
		}
		
		public static UserManager Instance
		{
			get {

				if(instance == null) 
				{	
					instance = new UserManager();
				}
				
				return instance;
			}
		}
	}
}

