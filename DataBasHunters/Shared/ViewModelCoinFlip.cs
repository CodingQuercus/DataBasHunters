using System;
namespace DataBasHunters.Shared
{
	public class ViewModelCoinFlip
	{
		public User creator { get; set; }
		public User opponent { get; set; }
        public Cointoss game { get; set; }
    }
}

