using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQMJoker3Pro
{
	internal class SaveData
	{
		private static SaveData mThis;
		private String mFileName = null;
		private Byte[] mBuffer = null;
		Byte[] mKey = { 0x2C, 0x29, 0x9C, 0x25, 0x3E, 0xC5, 0xDF, 0x0D, 0x0C, 0x06, 0x56, 0xC7, 0xEE, 0x64, 0xCE, 0x34, };
		Byte[] mIV = { 0x3C, 0x21, 0xB3, 0x3E, 0x28, 0x6D, 0x99, 0xF0, 0x96, 0xD2, 0x0F, 0x8B, 0xE9, 0x1E, 0xFE, 0x47, };
		private readonly System.Text.Encoding mEncode = System.Text.Encoding.UTF8;
		public uint Adventure { private get; set; } = 0;

		private SaveData()
		{ }

		public static SaveData Instance()
		{
			if (mThis == null) mThis = new SaveData();
			return mThis;
		}

		public bool Open(String filename, bool force)
		{
			if (System.IO.File.Exists(filename) == false) return false;

			Byte[] buffer = System.IO.File.ReadAllBytes(filename);
			mBuffer = new Byte[buffer.Length - 0x20];
			int index = 0;

			Byte[] nonce = new Byte[12];
			Array.Copy(buffer, 0, nonce, 0, nonce.Length);
			Array.Copy(nonce, 0, mBuffer, index, nonce.Length);
			index += 16;
			Byte[] tag = new Byte[16];
			Array.Copy(buffer, 16, tag, 0, tag.Length);
			Byte[] ciphertext = new Byte[0xD4];
			Array.Copy(buffer, 32, ciphertext, 0, ciphertext.Length);
			Byte[] headerext = new Byte[ciphertext.Length];

			var aesccm = new System.Security.Cryptography.AesCcm(mKey);
			aesccm.Decrypt(nonce, ciphertext, tag, headerext);
			Array.Copy(headerext, 0, mBuffer, index, headerext.Length);
			index += headerext.Length;

			Byte init = (Byte)(headerext[0xD0] ^ 185);
			Byte[] bodyKey = new Byte[16];
			for (int i = 0; i < bodyKey.Length; i++)
			{
				bodyKey[i] = (Byte)(init ^ mIV[i]);
				init += bodyKey[i];
			}

			Array.Copy(buffer, 0xF4, nonce, 0, nonce.Length);
			Array.Copy(nonce, 0, mBuffer, index, nonce.Length);
			index += 16;
			Array.Copy(buffer, 0x104, tag, 0, tag.Length);
			ciphertext = new Byte[0x37AF0];
			Array.Copy(buffer, 0x114, ciphertext, 0, ciphertext.Length);
			Byte[] bodytext = new Byte[ciphertext.Length];

			aesccm = new System.Security.Cryptography.AesCcm(bodyKey);
			aesccm.Decrypt(nonce, ciphertext, tag, bodytext);
			Array.Copy(bodytext, 0, mBuffer, index, bodytext.Length);
			mFileName = filename;
			Backup();
			return true;
		}

		public bool Save()
		{
			if (mFileName == null || mBuffer == null) return false;

			Byte[] bodytext = new Byte[0x37AF0];
			Array.Copy(mBuffer, 0xF4, bodytext, 0, bodytext.Length);

			var sha256 = System.Security.Cryptography.SHA256.Create();
			Byte[] hash = sha256.ComputeHash(bodytext);

			mBuffer[0xE0] = hash[0];
			Byte init = (Byte)(hash[0] ^ 185);
			Byte[] bodyKey = new Byte[16];
			for (int i = 0; i < bodyKey.Length; i++)
			{
				bodyKey[i] = (Byte)(init ^ mIV[i]);
				init += bodyKey[i];
			}

			Byte[] nonce = new Byte[12];
			Array.Copy(mBuffer, 0xE4, nonce, 0, nonce.Length);
			Byte[] tag = new Byte[16];
			Byte[] ciphertext = new Byte[bodytext.Length];

			var aesccm = new System.Security.Cryptography.AesCcm(bodyKey);
			aesccm.Encrypt(nonce, bodytext, ciphertext, tag);

			Byte[] buffer = new Byte[mBuffer.Length + 0x20];
			Array.Copy(nonce, 0, buffer, 0xF4, nonce.Length);
			Array.Copy(tag, 0, buffer, 0x104, tag.Length);
			Array.Copy(ciphertext, 0, buffer, 0x114, ciphertext.Length);

			Array.Copy(mBuffer, 0, nonce, 0, nonce.Length);
			Byte[] headertext = new Byte[0xD4];
			Array.Copy(mBuffer, 0x10, headertext, 0, headertext.Length);
			tag = new Byte[16];
			ciphertext = new Byte[headertext.Length];

			aesccm = new System.Security.Cryptography.AesCcm(mKey);
			aesccm.Encrypt(nonce, headertext, ciphertext, tag);

			Array.Copy(nonce, 0, buffer, 0, nonce.Length);
			Array.Copy(tag, 0, buffer, 0x10, tag.Length);
			Array.Copy(ciphertext, 0, buffer, 0x20, ciphertext.Length);

			System.IO.File.WriteAllBytes(mFileName, buffer);
			return true;
		}

		public bool SaveAs(String filename)
		{
			if (mFileName == null || mBuffer == null) return false;
			mFileName = filename;
			return Save();
		}

		public void Import(String filename)
		{
			if (mFileName == null) return;

			mBuffer = System.IO.File.ReadAllBytes(filename);
		}

		public void Export(String filename)
		{
			System.IO.File.WriteAllBytes(filename, mBuffer);
		}

		public uint ReadNumber(uint address, uint size)
		{
			if (mBuffer == null) return 0;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return 0;
			uint result = 0;
			for (int i = 0; i < size; i++)
			{
				result += (uint)(mBuffer[address + i]) << (i * 8);
			}
			return result;
		}

		public Byte[] ReadValue(uint address, uint size)
		{
			Byte[] result = new Byte[size];
			if (mBuffer == null) return result;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return result;
			for (int i = 0; i < size; i++)
			{
				result[i] = mBuffer[address + i];
			}
			return result;
		}

		public Byte ReadByte(uint address, bool isLow)
		{
			Byte result = 0;
			if (mBuffer == null) return result;
			address = CalcAddress(address);
			if (address > mBuffer.Length) return result;
			result = mBuffer[address];
			if (isLow == false)
			{
				result = (Byte)(result >> 4);
			}
			result &= 0x0F;

			return result;
		}

		// 0 to 7.
		public bool ReadBit(uint address, uint bit)
		{
			if (bit < 0) return false;
			if (bit > 7) return false;
			if (mBuffer == null) return false;
			address = CalcAddress(address);
			if (address > mBuffer.Length) return false;
			Byte mask = (Byte)(1 << (int)bit);
			Byte result = (Byte)(mBuffer[address] & mask);
			return result != 0;
		}

		public String ReadText(uint address, uint size)
		{
			if (mBuffer == null) return "";
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return "";

			Byte[] tmp = new Byte[size];
			for (uint i = 0; i < size; i++)
			{
				if (mBuffer[address + i] == 0) break;
				tmp[i] = mBuffer[address + i];
			}
			return mEncode.GetString(tmp).Trim('\0');
		}

		public void WriteNumber(uint address, uint size, uint value)
		{
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return;
			for (uint i = 0; i < size; i++)
			{
				mBuffer[address + i] = (Byte)(value & 0xFF);
				value >>= 8;
			}
		}

		public void WriteByte(uint address, bool isLow, Byte value)
		{
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address > mBuffer.Length) return;
			Byte tmp = mBuffer[address];
			if (isLow == false)
			{
				tmp &= 0x0F;
				value = (Byte)(value << 4);
			}
			else
			{
				tmp &= 0xF0;
			}
			mBuffer[address] = (Byte)(tmp | value);
		}

		// 0 to 7.
		public void WriteBit(uint address, uint bit, bool value)
		{
			if (bit < 0) return;
			if (bit > 7) return;
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address > mBuffer.Length) return;
			Byte mask = (Byte)(1 << (int)bit);
			if (value) mBuffer[address] = (Byte)(mBuffer[address] | mask);
			else mBuffer[address] = (Byte)(mBuffer[address] & ~mask);
		}

		public void WriteText(uint address, uint size, String value)
		{
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return;
			Byte[] tmp = mEncode.GetBytes(value);
			Array.Resize(ref tmp, (int)size);
			for (uint i = 0; i < size; i++)
			{
				mBuffer[address + i] = tmp[i];
			}
		}

		public void WriteValue(uint address, Byte[] buffer)
		{
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address + buffer.Length > mBuffer.Length) return;

			for (uint i = 0; i < buffer.Length; i++)
			{
				mBuffer[address + i] = buffer[i];
			}
		}

		public void Fill(uint address, uint size, Byte number)
		{
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return;
			for (uint i = 0; i < size; i++)
			{
				mBuffer[address + i] = number;
			}
		}

		public void Copy(uint from, uint to, uint size)
		{
			if (mBuffer == null) return;
			from = CalcAddress(from);
			to = CalcAddress(to);
			if (from + size > mBuffer.Length) return;
			if (to + size > mBuffer.Length) return;
			for (uint i = 0; i < size; i++)
			{
				mBuffer[to + i] = mBuffer[from + i];
			}
		}

		public void Swap(uint from, uint to, uint size)
		{
			if (mBuffer == null) return;
			from = CalcAddress(from);
			to = CalcAddress(to);
			if (from + size > mBuffer.Length) return;
			if (to + size > mBuffer.Length) return;
			for (uint i = 0; i < size; i++)
			{
				Byte tmp = mBuffer[to + i];
				mBuffer[to + i] = mBuffer[from + i];
				mBuffer[from + i] = tmp;
			}
		}

		public List<uint> FindAddress(String name, uint index)
		{
			List<uint> result = new List<uint>();
			if (mBuffer == null) return result;

			for (; index < mBuffer.Length; index++)
			{
				if (mBuffer[index] != name[0]) continue;

				int len = 1;
				for (; len < name.Length; len++)
				{
					if (mBuffer[index + len] != name[len]) break;
				}
				if (len >= name.Length) result.Add(index);
				index += (uint)len;
			}
			return result;
		}

		private uint CalcAddress(uint address)
		{
			return address + Adventure;
		}

		private void Backup()
		{
			DateTime now = DateTime.Now;
			String path = "backup";
			if (!System.IO.Directory.Exists(path))
			{
				System.IO.Directory.CreateDirectory(path);
			}
			path = System.IO.Path.Combine(path, $"{now:yyyy-MM-dd HH-mm-ss} {System.IO.Path.GetFileName(mFileName)}");
			System.IO.File.Copy(mFileName, path, true);
		}
	}
}
