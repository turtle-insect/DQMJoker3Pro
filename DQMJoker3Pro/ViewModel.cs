using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQMJoker3Pro
{
	internal class ViewModel
	{
		private Info mInfo = Info.Instance();
		public ObservableCollection<Monster> Monsters { get; private set; } = new ObservableCollection<Monster>();

		public ViewModel()
		{
			for (uint index = 0; index < 500; index++)
			{
				Monster monster = new Monster(0x3EC + index * 240);
				if (monster.Type == 0) continue;

				Monsters.Add(monster);
			}
		}

		public uint Money
		{
			get { return SaveData.Instance().ReadNumber(0x1C8, 4); }
			set { Util.WriteNumber(0x1C8, 4, value, 0, 999999); }
		}

		public uint Bank
		{
			get { return SaveData.Instance().ReadNumber(0x1CC, 4); }
			set { Util.WriteNumber(0x1CC, 4, value, 0, 99999999); }
		}

		public String Name
		{
			get { return SaveData.Instance().ReadText(0xFC, 14); }
			set { SaveData.Instance().WriteText(0xFC, 14, value); }
		}
	}
}
