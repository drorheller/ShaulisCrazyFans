using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace ShaulisCrazyFans.Models
{
    public class Post
    {
        public Post()
        {
            ReleaseDate = DateTime.Now;
            Comments = new List<Comment>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }
        
        [DisplayName("Author Site")]
        public string AuthorSite { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; }

        public string Content { get; set; }

        public List<Comment> Comments { get; set; }
    }
}