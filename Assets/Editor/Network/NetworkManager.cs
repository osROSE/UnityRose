// <copyright file="NetworkManager.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author> Wadii Bellamine, Wahid Bouakline</author>
// <date>2/25/2015 8:37 AM </date>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using Common.Cryptography;
using Network.Packets;
using Network.JsonConverters;
using JsonFx.Json;
using System.Runtime.CompilerServices;
using System.IO;

namespace Network
{
    
    public class State {

        public enum PacketState
        {
            HEADER,
            PAYLOAD
        };

        public PacketState packetState;

        public Socket socket;

        public const int BufferSize = 1024;

        public byte[] buffer = new byte[BufferSize];

        public int bytesReceived = 0; // bytes received so far
        public int bytesExpected = 0; // total bytes expected to be received

        public StringBuilder sb = new StringBuilder();
    }

    public class NetworkManager : MonoBehaviour
    {
        private const string ipAddress = "52.5.24.74";

        private const int port = 3000;

        private static Socket socket = null;

        private static AES crypto;

        private const int PrefixSize = 8;

        private static State state;


        void Awake()
        {
            state = new State();
            state.packetState = State.PacketState.HEADER;
            state.bytesReceived = 0;
            state.bytesExpected = PrefixSize;
            Connect();
        }


        private static void Connect()
        {
            try
            {
                crypto = new AES();
                IPAddress ip = IPAddress.Parse(ipAddress);

                IPEndPoint remoteEp = new IPEndPoint(ip, port);

                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket = client;

                client.BeginConnect(remoteEp, new AsyncCallback(ConnectCallback), client);


            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        private static void Disconnect()
        {
            if (socket != null)
                socket.Disconnect(false);
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                client.EndConnect(ar);

                Debug.Log("Socket connected to " + client.RemoteEndPoint.ToString());

                Recieve();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        public static void Recieve()
        {
            if (socket == null || !socket.Connected)
            {
                Connect();
                return;
            }

            try
            {
                state.socket = socket;
                if (state.packetState == State.PacketState.HEADER)
                {
                    socket.BeginReceive(state.buffer, state.bytesReceived, state.bytesExpected, 0, new AsyncCallback(RecievePrefixCallback), state);
                }
                else if (state.packetState == State.PacketState.PAYLOAD)
                    socket.BeginReceive(state.buffer, state.bytesReceived, state.bytesExpected, 0, new AsyncCallback(RecievePayloadCallback), state);
                
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        private static void RecievePrefixCallback(IAsyncResult ar)
        {
            try
            {
                state = (State)ar.AsyncState;
                Socket client = state.socket;

                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    if (bytesRead == state.bytesExpected)
                    {
                        string size = Encoding.ASCII.GetString(state.buffer, 0, PrefixSize);
                        Debug.Log("size: " + size);
                        int payloadSize = Int32.Parse(size, System.Globalization.NumberStyles.HexNumber);
                        state = new State();
                        state.packetState = State.PacketState.PAYLOAD;
                        state.bytesExpected = payloadSize;
                        state.bytesReceived = 0;
                    }
                    else
                    {
                        state.bytesExpected = PrefixSize - bytesRead;
                        state.bytesReceived += bytesRead;
                    }
                }

            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }

            Recieve();
        }


        private static void RecievePayloadCallback(IAsyncResult ar)
        {
            try
            {
                state = (State)ar.AsyncState;
                Socket client = state.socket;

                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, state.bytesReceived, bytesRead));

                    if (bytesRead == state.bytesExpected)
                    {
                        string data = crypto.Decrypt(state.sb.ToString());

                        // Switch to header mode
                        state = new State();
                        state.packetState = State.PacketState.HEADER;
                        state.bytesExpected = PrefixSize;
                        state.bytesReceived = 0;

                        Debug.Log(data);


                        JsonReader reader = new JsonReader(data);

                        Packet packet = reader.Deserialize<Packet>();

                        PacketType type = (PacketType)packet.type;

                        switch (type)
                        {
                            case PacketType.CHARACTER:
                                CharacterManager.Instance.handlePacket(packet.operation, data);
                                break;
                            case PacketType.USER:
                                UserManager.Instance.handlePacket(packet.operation, data);
                                break;
                            default:
                                break;

                        }
                    }
                    else
                    {
                        state.bytesExpected -= bytesRead;
                        state.bytesReceived += bytesRead;
                    }

                }

            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }

            Recieve();
        }
		
        public static void Send(Packets.Packet packet)
        {
			if (socket == null || !socket.Connected)
			{
				Connect();
				return;
			}

			string clearText = packet.toString();
			Debug.Log("Sending: " + clearText);
			byte[] byteData = Encoding.ASCII.GetBytes( crypto.Encrypt(clearText) );

            socket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), socket);
        }
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                int bytesSend = client.EndSend(ar);

				Debug.Log("Send " + bytesSend + " bytes to server.");
				
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
    }
}
