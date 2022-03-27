using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQMJoker3Pro
{
	internal class Number : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		private readonly uint mAddress;
		private readonly uint mSize;
		public Number(uint address, uint size)
		{
			mAddress = address;
			mSize = size;
		}

		public uint Value
		{
			get { return SaveData.Instance().ReadNumber(mAddress, mSize); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress, mSize, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
			}
		}
	}
}
