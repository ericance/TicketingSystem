using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.Models
{
	public class Ticket
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Enter a sprint Name.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Enter a sprint description.")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Select the sprint number.")]
		public string SprintId { get; set; }
		public Sprint Sprint { get; set; }

		[Required(ErrorMessage = "Select a point value.")]
		public string PointId { get; set; }
		public Point Point { get; set; }

		[Required(ErrorMessage = "Select a status.")]
		public string StatusId { get; set; }
		public Status Status { get; set; }
	}
}
