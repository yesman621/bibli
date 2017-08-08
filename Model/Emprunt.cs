namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public  class Emprunt
    {
        [Key]
        [StringLength(5)]
        public string idEmprunt { get; set; }

        [Required]
        [StringLength(5)]
        public string idEx { get; set; }

        [Required]
        [StringLength(5)]
        public string numCarte { get; set; }

        [Column(TypeName = "date")]
        public DateTime dateEmprnt { get; set; }

        [Column(TypeName = "date")]
        public DateTime dateMax { get; set; }

         [ForeignKey("numCarte")]
        public  Emprunteur Emprunteur { get; set; }

         [ForeignKey("idEx")]
        public  Exemplaire Exemplaire { get; set; }
    }
}
