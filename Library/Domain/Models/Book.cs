using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
	public class Book
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required(ErrorMessageResourceType =typeof(Resource.ResourceData),ErrorMessageResourceName ="BookName")]
        [MaxLength(20,ErrorMessageResourceType =typeof(Resource.ResourceData),ErrorMessageResourceName ="MaxLength")]
        [MinLength(3,ErrorMessageResourceType =typeof(Resource.ResourceData),ErrorMessageResourceName ="MinLength")]
		public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "AuthorName")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "MaxLength")]
        [MinLength(3, ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "MinLength")]
        public string Author { get; set; }
		public string ImageName { get; set; }
		public string FileName { get; set; }
		public string Description { get; set; }
		public bool Publish { get; set; }
        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        [ForeignKey("SubCategory")]
        public Guid SubCategoryId { get; set; }
       
        public SubCategory SubCategory { get; set; }

        public int CurrentState { get; set; }

    }
}
