using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CakesByRAndRCore.Models
{
    [Table("CakeCategory")]
    public partial class CakeCategory
    {
        [Key]
        public int Id { get; set; }
        [StringLength(256)]
        public string? TextBan { get; set; }
        [StringLength(256)]
        public string? TextEng { get; set; }
        [StringLength(256)]
        public string? URL { get; set; }
        public int? ParentId { get; set; }
        public bool? ForAll { get; set; }
        public int? Order { get; set; }
        [StringLength(100)]
        public string? Icon { get; set; }
        public int? Status { get; set; }
        [StringLength(50)]
        public string? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [StringLength(50)]
        public string? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateDate { get; set; }
    }
}
