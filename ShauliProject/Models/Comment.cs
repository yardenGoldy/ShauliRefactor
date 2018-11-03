using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShauliProject.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [ForeignKey("Post")]
        public int PostId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Writer { get; set; }
        [Display(Name = "Writer's Website")]
        public string WriterWebSiteUrl { get; set; }
        [Required]
        [Display(Name = "Comment Content")]
        public string Content { get; set; }
        [Display(Name = "Post Title")]
        public Post Post { get; set; }
    }
}