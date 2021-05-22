namespace BookStore.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderDetail")]
    public partial class OrderDetail
    {
        public int ID { get; set; }

        public int idOrder { get; set; }

        public int idBook { get; set; }

        public int Quantity { get; set; }

        public virtual Book Book { get; set; }

        public virtual Order Order { get; set; }
    }
}
