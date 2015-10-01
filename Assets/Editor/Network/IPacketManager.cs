using System;
using Network.Packets;

namespace Network
{
	public interface IPacketManager
	{
		void registerCallback(int operation, Action<object> callback);
		void handlePacket(int operation, string packet);
		object Convert(int operation, string packet);
	}
}

