using System;
namespace DataBasHunters.Shared
{
	public class FinishGame
	{
		public User winner { get; set; }
		public User loser { get; set; }
        public Cointoss game { get; set; }
		public User joinperson { get; set; }
    }
}

