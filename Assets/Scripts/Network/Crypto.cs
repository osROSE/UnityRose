// <copyright file="Crypto.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <authors>Wadii Bellamine, Wahid Bouakline</authors>
// <date>2/25/2015 8:37 AM </date>

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;


namespace Common.Cryptography
{
	/// <summary>
	/// AES is a symmetric 256-bit encryption algorthm.
	/// Read more: http://en.wikipedia.org/wiki/Advanced_Encryption_Standard
	/// </summary>
	public class AES
	{
        const bool encrypt = false;  // set to true to allow encryption

        private const string _SALT = "g46dzQ80";
		private const string _INITVECTOR = "5TGB&YHN7UJM(IK<";
		private const string _KEY = "!QAZ2WSX#EDC4RFV";
		
		private byte[] _saltBytes;
		private byte[] _initVectorBytes;
		private byte[] _keyBytes;
		
		public AES()
		{
			_saltBytes = Encoding.UTF8.GetBytes(_SALT);
			_initVectorBytes = Encoding.UTF8.GetBytes(_INITVECTOR);
			_keyBytes = Encoding.UTF8.GetBytes(_KEY);
		}
		
		
		/// <summary>
		/// Encrypts a string with AES
		/// </summary>
		/// <param name="plainText">Text to be encrypted</param>
		/// <param name="password">Password to encrypt with</param>   
		/// <param name="salt">Salt to encrypt with</param>    
		/// <param name="initialVector">Needs to be 16 ASCII characters long</param>    
		/// <returns>An encrypted string</returns>        
		public string Encrypt(string plainText, string password = null, string salt = null, string initialVector = null)
		{
            if (encrypt)
                return Convert.ToBase64String(EncryptToBytes(plainText, password, salt, initialVector));
            else
                return plainText;
		}
		
		/// <summary>
		/// Encrypts a string with AES
		/// </summary>
		/// <param name="plainText">Text to be encrypted</param>
		/// <param name="password">Password to encrypt with</param>   
		/// <param name="salt">Salt to encrypt with</param>    
		/// <param name="initialVector">Needs to be 16 ASCII characters long</param>    
		/// <returns>An encrypted string</returns>        
		public byte[] EncryptToBytes(string plainText, string password = null, string salt = null, string initialVector = null)
		{
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			return EncryptToBytes(plainTextBytes, password, salt, initialVector);
		}
		
		/// <summary>
		/// Encrypts a string with AES
		/// </summary>
		/// <param name="plainTextBytes">Bytes to be encrypted</param>
		/// <param name="password">Password to encrypt with</param>   
		/// <param name="salt">Salt to encrypt with</param>    
		/// <param name="initialVector">Needs to be 16 ASCII characters long</param>    
		/// <returns>An encrypted string</returns>        
		public byte[] EncryptToBytes(byte[] plainTextBytes, string password = null, string salt = null, string initialVector = null)
		{
			int keySize = 256;
			
			byte[] initialVectorBytes = string.IsNullOrEmpty(initialVector) ? _initVectorBytes : Encoding.UTF8.GetBytes(initialVector);
			byte[] saltValueBytes = string.IsNullOrEmpty(salt) ? _saltBytes : Encoding.UTF8.GetBytes(salt);
			byte[] keyBytes = string.IsNullOrEmpty(password) ? _keyBytes : new Rfc2898DeriveBytes(password, saltValueBytes).GetBytes(keySize / 8);
			
			using (RijndaelManaged symmetricKey = new RijndaelManaged())
			{
				symmetricKey.Mode = CipherMode.CBC;
				symmetricKey.Padding = PaddingMode.PKCS7;
				
				using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initialVectorBytes))
				{
					using (MemoryStream memStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
						{
							cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
							cryptoStream.FlushFinalBlock();
							
							return memStream.ToArray();
						}
					}
				}
			}
		}
		
		/// <summary>  
		/// Decrypts an AES-encrypted string. 
		/// </summary>  
		/// <param name="cipherText">Text to be decrypted</param> 
		/// <param name="password">Password to decrypt with</param> 
		/// <param name="salt">Salt to decrypt with</param> 
		/// <param name="initialVector">Needs to be 16 ASCII characters long</param> 
		/// <returns>A decrypted string</returns>
		public string Decrypt(string cipherText, string password = null, string salt = null, string initialVector = null)
		{
            if (encrypt)
            {
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText.Replace(' ', '+'));
                return Decrypt(cipherTextBytes, password, salt, initialVector).TrimEnd('\0');
            }
            else
                return cipherText;
		}
		
		/// <summary>  
		/// Decrypts an AES-encrypted string. 
		/// </summary>  
		/// <param name="cipherText">Text to be decrypted</param> 
		/// <param name="password">Password to decrypt with</param> 
		/// <param name="salt">Salt to decrypt with</param> 
		/// <param name="initialVector">Needs to be 16 ASCII characters long</param> 
		/// <returns>A decrypted string</returns>
		public string Decrypt(byte[] cipherTextBytes, string password = null, string salt = null, string initialVector = null)
		{
			int keySize = 256;
			
			byte[] initialVectorBytes = string.IsNullOrEmpty(initialVector) ? _initVectorBytes : Encoding.UTF8.GetBytes(initialVector);
			byte[] saltValueBytes = string.IsNullOrEmpty(salt) ? _saltBytes : Encoding.UTF8.GetBytes(salt);
			byte[] keyBytes = string.IsNullOrEmpty(password) ? _keyBytes : new Rfc2898DeriveBytes(password, saltValueBytes).GetBytes(keySize / 8);
			byte[] plainTextBytes = new byte[cipherTextBytes.Length];
			
			using (RijndaelManaged symmetricKey = new RijndaelManaged())
			{
				symmetricKey.Mode = CipherMode.CBC;
				symmetricKey.Padding = PaddingMode.PKCS7;
				
				using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initialVectorBytes))
				{
					using (MemoryStream memStream = new MemoryStream(cipherTextBytes))
					{
						using (CryptoStream cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read))
						{
							int byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
							
							return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
						}
					}
				}
			}
		}
	}
}