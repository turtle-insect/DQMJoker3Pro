using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQMJoker3Pro
{
	internal class Monster : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;


		public Monster(uint address)
		{
			mAddress = address;
		}

		public String Name
		{
			get { return SaveData.Instance().ReadText(mAddress, 14); }
			set
			{
				SaveData.Instance().WriteText(mAddress, 14, value);
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

		public uint Type
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 24, 2); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress + 24, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Type)));
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

		public uint Property1
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 64, 2); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress + 64, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Property1)));
			}
		}

		public uint Property2
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 66, 2); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress + 66, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Property2)));
			}
		}

		public uint Property3
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 68, 2); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress + 68, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Property3)));
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

		public uint Skill1
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 82, 4); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress + 82, 4, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Skill1)));
			}
		}

		public uint Skill2
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 86, 4); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress + 86, 4, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Skill2)));
			}
		}

		public uint Skill3
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 90, 4); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress + 90, 4, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Skill3)));
			}
		}

		private readonly uint mAddress;
	}
}
