using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseTest.DatabaseTables
{
	public class Assignment
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("Client")]
		public int IdClient { get; set; }

		[ForeignKey("Config")]
		public int IdConfig { get; set; }

		public Config Config { get; set; }
		public Client Client { get; set; }
	}
}


