using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQMJoker3Pro
{
	internal class Skill : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		private readonly uint mAddress;

		public Skill(uint address)
		{
			mAddress = address;
		}

		public uint ID
		{
			get => SaveData.Instance().ReadNumber(mAddress, 2);
			set
			{
				SaveData.Instance().WriteNumber(mAddress, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
			}
		}

		public uint Point
		{
			get => SaveData.Instance().ReadNumber(mAddress + 2, 2);
			set
			{
				SaveData.Instance().WriteNumber(mAddress + 2, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Point)));
			}
		}
	}
}
