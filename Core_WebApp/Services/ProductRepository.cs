using Core_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_WebApp.Services
{
    /// <summary>
    /// Inject the DbContext as constructor injectation
    /// </summary>
    public class ProductRepository : IRepository<Product, int>
    {
        private readonly AppJune2020DbContext ctx;
        public ProductRepository(AppJune2020DbContext ctx)
        {
            this.ctx = ctx;
        }
        /// <summary>
        /// async: The method will be containing the Asynchronous call
        /// and this method returns Task Object
        /// await: The statement performs Async operation on separate thread
        /// other than calling thread and the async method will returns response
        /// to the calling thread
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Product> CraeteAsync(Product entity)
        {
            // new Category will be added in DbSet
            var result = await ctx.Products.AddAsync(entity);
            // the transaction will be commited by adding new category in
            // Categories table
            await ctx.SaveChangesAsync();
            // newly created Category will be returned
            return result.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // seacrh record based on Primary key
            var cat = await ctx.Products.FindAsync();
            if (cat != null)
            {
                // remove the object
                ctx.Products.Remove(cat);
                await ctx.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Product>> GetAsync()
        {
            return await ctx.Products.ToListAsync();
        }

        public async Task<Product> GetAsync(int id)
        {
            return await ctx.Products.FindAsync(id);
        }

        public async Task<Product> UpdateAsync(int id, Product entity)
        {
            // seacrh record based on Primary key
            // C# 7.0+ new declaratrion knonw as 'Discard'
            var _ = await ctx.Products.FindAsync(id);

            if (_ != null)
            {
                _.ProductId = entity.ProductId;
                _.ProductName = entity.ProductName;
                _.Description = entity.Description;
                _.Price = entity.Price;
                _.CategoryRowId = entity.CategoryRowId;
                await ctx.SaveChangesAsync();
                return _;
            }

            return entity;
        }
    }
}
