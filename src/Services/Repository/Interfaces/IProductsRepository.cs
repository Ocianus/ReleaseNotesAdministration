﻿using Services.Repository.Models.DataTransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repository.Interfaces
{
    public interface IProductsRepository
    {
        Task<List<ProductDto>> GetAllProducts();
        Task<ProductDto> GetProductById(int productId);
        Task<int> CreateProduct(ProductDto productDto);
        Task<ProductDto> UpdateProduct(int ProductId, ProductDto product);
        Task<bool> DeleteProduct(int productId);
    }
}
