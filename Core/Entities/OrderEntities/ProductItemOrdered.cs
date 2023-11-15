using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderEntities
{
    public class ProductItemOrdered
    {
        public ProductItemOrdered() { } 

        public ProductItemOrdered(int productItemId, string productName, string picturUrl)
        {
            ProductItemId = productItemId;
            ProductName = productName;
            PicturUrl = picturUrl;
        }

        public int ProductItemId { get; set; }

        public string ProductName { get; set; }
        public string PicturUrl { get; set; }


    }
}
