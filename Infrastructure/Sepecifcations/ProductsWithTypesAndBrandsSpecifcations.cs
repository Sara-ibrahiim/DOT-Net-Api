using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Sepecifcations
{
    public class ProductsWithTypesAndBrandsSpecifcations : BaseSepecifcations<Product>
    {
        public ProductsWithTypesAndBrandsSpecifcations(ProductSepecifcations sepecifcations) 
            : base( x=>
            (string.IsNullOrEmpty(sepecifcations.Search)|| x.Name.Trim().ToLower().Contains(sepecifcations.Search))&&
            (!sepecifcations.BrandId.HasValue || x.ProductBrandId == sepecifcations.BrandId) 
            && (sepecifcations.TypeId.HasValue || x.ProductBrandId == sepecifcations.TypeId)
            )
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            AddOrderBy(p => p.Name);
            ApplyPagination(sepecifcations.PageSize * (sepecifcations.PageIndex - 1), sepecifcations.PageSize);

            if(!string.IsNullOrEmpty(sepecifcations.Sort))
            {
                switch (sepecifcations.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;

                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;

                }
            }


        }
        public ProductsWithTypesAndBrandsSpecifcations(int? id)
           : base(x => x.Id == id )
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);

        }
    }
}
