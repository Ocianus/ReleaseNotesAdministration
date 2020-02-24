﻿using AutoMapper;
using Dapper;
using Microsoft.Extensions.Options;
using Services.Repository.Config;
using Services.Repository.Interfaces;
using Services.Repository.Models.DatabaseModels;
using Services.Repository.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Services.Repository
{
    public class ProductsRepository : IProductsRepository
    {
        public readonly string _connectionString;
        private readonly IMapper _mapper;

        public ProductsRepository(IOptions<SqlDbConnection> sqlDbConnection, IMapper mapper)
        {
            _connectionString = sqlDbConnection.Value.ConnectionString;
            _mapper = mapper;
        }

        public async Task CreateProduct(ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);

                using (var connection = new SqlConnection(_connectionString))
                {
                    var insert = @"INSERT INTO [ProductsDb]
                                (
                                    [ProductId],
                                    [ProductName],
                                    [ProductImage],
                                    [ProductDescription]
                                )
                                VALUES
                                (
                                    @ProductId,
                                    @ProductName
                                    @ProductImage
                                    @ProductDescription
                                )
                                SELECT [Id] FROM [ReleaseNotesDb] WHERE [Id] = @Id AND [ProductId] = @ProductId";
                    await connection.ExecuteAsync(insert, product);
                }
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message);
            }
        }

        public async Task<ProductDto> GetProductById(int? productId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT *
                FROM [ProductsDb]
                WHERE [ProductId] = @ProductId";

                var product = await connection.QueryFirstOrDefaultAsync<Product>(query, new { @ProductId = productId });
                var mappedProduct = _mapper.Map<ProductDto>(product);
                return mappedProduct;
            }
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT *
                FROM [ProductsDb]";

                var product = await connection.QueryAsync<Product>(query);
                var productMapped = _mapper.Map<List<ProductDto>>(product);
                return productMapped;
            }
        }

        public async Task<ProductDto> UpdateProduct(int? ProductId, ProductDto product)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var updateDb = @"UPDATE [ProductsDb]
                    SET
                        [ProductId] = @ProductId, 
                        [ProductName] = @ProductName,
                        [ProductImage] = @ProductImage,
                        [ProductDescription] = @ProductDescription
                    WHERE [ProductId] = @ProductId";
                    var productMapped = _mapper.Map<Product>(product);
                    productMapped.AddProductId(ProductId);

                    var result = await connection.ExecuteAsync(updateDb, productMapped);
                    return product;
                }
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message);
            }
        }

        public async Task<bool> DeleteProduct(int? productId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var Delete = "DELETE FROM ProductDb WHERE ProductId = @ProductId";
                    var returnedProduct = await connection.ExecuteAsync(Delete, new { @ProductId = productId });
                    bool success = returnedProduct > 0;
                    return success;
                }
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message);
            }
        }
    }
}