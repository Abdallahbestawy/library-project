using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
	public class LogBook
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
		public string Action { get; set; }
		public DateTime Date { get; set; }
		[ForeignKey("Book")]
		public Guid BookId { get; set; }
		public Book Book { get; set; }
	}
}
