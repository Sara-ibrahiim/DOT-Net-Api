using AutoMapper;
using Core.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Sepecifcations;
using Microsoft.AspNetCore.Mvc;

using Services.Helper;
using Services.Services.ProductSerives.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.ProductSerives
{
    public class ProductService : IProductService
    {
        private readonly IUntiOfWork _untiOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUntiOfWork untiOfWork , IMapper mapper)
        {
            _untiOfWork = untiOfWork;
            _mapper = mapper;
        }
        // private readonly IUntiOfWork
        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
            => await _untiOfWork.Repository<ProductBrand>().GetAllAsync();

        public async Task<ProductResultDto> GetProductByIdAsync(int? id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecifcations(id);
           
            var product = await _untiOfWork.Repository<Product>().GetEntityWithSpecificationsAsync(spec);
            //if (product is null)
            //    return NotFound(new ApiResponse());
            
            var mappedProduct = _mapper.Map<ProductResultDto>(product);
            return mappedProduct;
        }
       




        public async Task<Pagination<ProductResultDto>> GetProductsAsync(ProductSepecifcations sepecifcations)
          
        {
            var spec = new ProductsWithTypesAndBrandsSpecifcations(sepecifcations);
            var products = await _untiOfWork.Repository<Product>().GetAllWithSpecificationsAsync(spec);
            var totleItems = await _untiOfWork.Repository<Product>().CountAsync(spec);
            var mappedProducts = _mapper.Map<IReadOnlyList<ProductResultDto>>(products);
            return new Pagination<ProductResultDto>(sepecifcations.PageIndex, sepecifcations.PageSize,totleItems,mappedProducts);
        }




        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
          => await _untiOfWork.Repository<ProductType>().GetAllAsync();
    }

}
