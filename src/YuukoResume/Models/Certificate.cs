using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pomelo.AspNetCore.Extensions.BlobStorage.Models;

namespace YuukoResume.Models
{
    public class Certificate
    {
        public long Id { get; set; }

        [Localized]
        [MaxLength(512)]
        public string Title { get; set; }

        [ForeignKey("Blob")]
        public Guid BlobId { get; set; }

        public virtual Blob Blob { get; set; }
        
        public long PRI { get; set; }
    }
}
