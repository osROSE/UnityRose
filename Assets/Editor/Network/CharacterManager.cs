using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network.Packets;
using Network.JsonConverters;
using JsonFx;
using JsonFx.Json;

namespace Network
{
	public class CharacterManager
	{	
		private static CharacterManager instance;
		
		private Dictionary<CharacterOperation, Action<object>> callbacks;
		
		public CharacterManager ()
		{
			callbacks = new Dictionary<CharacterOperation, Action<object>>();
		}
		
		public void registerCallback(CharacterOperation operation, Action<object> callback) {
			this.registerCallback((int)operation, callback);
		}
		
		public void registerCallback(int operation, Action<object> callback) {
			CharacterOperation characterOperation = (CharacterOperation)operation;
			
			if(callbacks.ContainsKey(characterOperation)) {
				callbacks[characterOperation] = callback;
			} else {
				callbacks.Add(characterOperation, callback);
			}
		}
		
		public void handlePacket(int operation, string packet) {
			CharacterOperation key = (CharacterOperation)operation;
			
			if(callbacks.ContainsKey(key)) {
				Action<object> callback = callbacks[key];
				object _packet = this.Convert(operation, packet);
				callback.Invoke(_packet);
			}
		}
		
		public object Convert(int operation, string packet) 
		{
			object _packet = null;
			
			CharacterOperation key = (CharacterOperation)operation;
			
			JsonReaderSettings settings = new JsonReaderSettings();
			settings.AddTypeConverter (new VectorConverter());
			settings.AddTypeConverter (new QuaternionConverter());
			
			JsonReader jsonReader = new JsonReader(packet, settings);
			
			switch(key) 
			{
				case CharacterOperation.GROUNDCLICK:
					_packet = jsonReader.Deserialize<GroundClick>();
					break;
				case CharacterOperation.INSTANTIATE:
					_packet = jsonReader.Deserialize<InstantiateChar>();
					break;
				case CharacterOperation.DESTROY:
					_packet = jsonReader.Deserialize<DestroyChar>();
					break;
				default:
					break;
			}
			
			return _packet;
		}
		
		public static CharacterManager Instance
		{
			get {
				
				if(instance == null) 
				{	
					instance = new CharacterManager();
				}
				
				return instance;
			}
		}
	}
}

