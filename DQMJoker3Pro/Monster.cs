using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQMJoker3Pro
{
	internal class Monster : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		public Number Type { get; private set; }
		public ObservableCollection<Number> Propertys { get; private set; } = new ObservableCollection<Number>();
		public ObservableCollection<Number> Skills { get; private set; } = new ObservableCollection<Number>();

		public Monster(uint address)
		{
			mAddress = address;

			Type = new Number(address + 24, 2);
			for (uint i = 0; i < 7; i++)
			{
				Propertys.Add(new Number(address + 64 + i * 2, 2));
			}
			for (uint i = 0; i < 3; i++)
			{
				Skills.Add(new Number(address + 82 + i * 4, 4));
			}
		}

		public String Name
		{
			get { return SaveData.Instance().ReadText(mAddress, 16); }
			set
			{
				SaveData.Instance().WriteText(mAddress, 16, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
			}
		}

		public uint Lv
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 42, 1); }
			set
			{
				Util.WriteNumber(mAddress + 42, 1, value, 1, 99);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Lv)));
			}
		}

		public uint MaxHP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 26, 2); }
			set
			{
				Util.WriteNumber(mAddress + 26, 2, value, 1, 9999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxHP)));
			}
		}

		public uint MaxMP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 28, 2); }
			set
			{
				Util.WriteNumber(mAddress + 28, 2, value, 0, 9999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxMP)));
			}
		}

		public uint HP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 30, 2); }
			set
			{
				Util.WriteNumber(mAddress + 30, 2, value, 0, 9999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HP)));
			}
		}

		public uint MP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 32, 2); }
			set
			{
				Util.WriteNumber(mAddress + 32, 2, value, 0, 9999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MP)));
			}
		}

		public uint Offense
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 34, 2); }
			set
			{
				Util.WriteNumber(mAddress + 34, 2, value, 0, 9999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Offense)));
			}
		}

		public uint Defense
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 36, 2); }
			set
			{
				Util.WriteNumber(mAddress + 36, 2, value, 0, 9999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Defense)));
			}
		}

		public uint Speed
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 38, 2); }
			set
			{
				Util.WriteNumber(mAddress + 38, 2, value, 0, 9999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Speed)));
			}
		}

		public uint Wise
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 40, 2); }
			set
			{
				Util.WriteNumber(mAddress + 40, 2, value, 0, 9999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Wise)));
			}
		}

		public uint Exp
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 44, 4); }
			set
			{
				Util.WriteNumber(mAddress + 44, 4, value, 0, 9999999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Exp)));
			}
		}

		public uint SkillPoint
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 120, 2); }
			set
			{
				Util.WriteNumber(mAddress + 120, 2, value, 0, 999);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SkillPoint)));
			}
		}

		private readonly uint mAddress;
	}
}
