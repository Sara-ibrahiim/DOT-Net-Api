using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Sepecifcations
{
    public class ProductsWithFiltersForCountSepecifcations : BaseSepecifcations<Product>
    {

        public ProductsWithFiltersForCountSepecifcations(ProductSepecifcations sepecifcations)
         : base(x =>
          (string.IsNullOrEmpty(sepecifcations.Search) || x.Name.Trim().ToLower().Contains(sepecifcations.Search)) &&
         (!sepecifcations.BrandId.HasValue || x.ProductBrandId == sepecifcations.BrandId)
         && (sepecifcations.TypeId.HasValue || x.ProductBrandId == sepecifcations.TypeId)
         )
        {

        }
    }
}
