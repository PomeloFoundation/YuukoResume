using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pomelo.AspNetCore.Extensions.BlobStorage.Models;

namespace YuukoResume.Models
{
    public class Project
    {
        public long Id { get; set; }
        
        [Localized]
        [MaxLength(1024)]
        public string Title { get; set; }

        [Localized]
        public string Description { get; set; }

        [Localized]
        [MaxLength(1024)]
        public string Tags { get; set; }

        [MaxLength(256)]
        public string GitHub { get; set; }

        [MaxLength(256)]
        public string DemoUrl { get; set; }

        public DateTime From { get; set; }

        public DateTime? To { get; set; }

        [Localized]
        [MaxLength(1024)]
        public string Catalog { get; set; }

        [ForeignKey("Blob")]
        public Guid BlobId { get; set; }

        public virtual Blob Blob { get; set; }
    }
}
