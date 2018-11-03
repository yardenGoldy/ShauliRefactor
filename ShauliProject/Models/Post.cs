using ShauliProject.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace ShauliProject.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Writer Name")]
        public string Writer { get; set; }
        [Display(Name = "Writer's Website")]
        public string WriterWebSiteUrl { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PublishDate { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public string Image { get; set; }
        public string Video { get; set; }
        public List<Comment> Comments { get; set; }

        public void ConvertImageFileToPath(string ImagePath)
        {
            this.Image = ImagePath;
        }

        public void ConvertVideoFileToPath(string VideoPath)
        {
            this.Video = VideoPath;
        }

        public string GetImageHtml
        {
            get
            {
                if (this.Image != null && this.Image != string.Empty)
                {
                    byte[] imageData = null;
                    var fileInfo = new FileInfo(this.Image);
                    var imageFileLength = fileInfo.Length;
                    using (var fs = new FileStream(this.Image, FileMode.Open, FileAccess.Read))
                    {
                        using (var br = new BinaryReader(fs))
                        {
                            imageData = br.ReadBytes((int)imageFileLength);
                            var base64Image = Convert.ToBase64String(imageData);
                            return string.Format("data:image/png;base64,{0}", base64Image);
                        }
                    }
                }

                return this.Image;
            }
        }

        public string GetVideoHtml
        {
            get
            {
                if (this.Video != null && this.Video != string.Empty)
                {
                    byte[] videoData = null;
                    var fileInfo = new FileInfo(this.Video);
                    var videoFileLength = fileInfo.Length;
                    using (var fs = new FileStream(this.Video, FileMode.Open, FileAccess.Read))
                    {
                        using (var br = new BinaryReader(fs))
                        {
                            videoData = br.ReadBytes((int)videoFileLength);
                            var base64Video = Convert.ToBase64String(videoData);
                            return string.Format("data:video/mp4;base64,{0}", base64Video);
                        }
                    }
                }

                return this.Video;
            }
        }
    }
}