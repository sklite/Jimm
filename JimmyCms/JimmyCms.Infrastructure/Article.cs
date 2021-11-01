using System;
using System.ComponentModel.DataAnnotations;

namespace JimmyCms.Infrastructure
{
    public class Article
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [Required]
        [StringLength(1000)]
        public string Body { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}