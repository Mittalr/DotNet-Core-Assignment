using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core_WebApp.Models;
using Core_WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace Core_WebApp.Controllers
{
    /// <summary>
    /// Contains action methods (methods those will be executed over HttpRequest)
    /// ActionMethos can be either HttpGet (Default) or HttpPost(HttpPut/HttpDelete)
    /// ActionMethod Retuens IActionResult interface
    /// </summary>
    public class ProductController : Controller
    {
        private readonly IRepository<Product, int> ProdRepository;
        /// <summary>
        /// Inject the ProductRepository as constructor injection
        /// </summary>
        public ProductController(IRepository<Product, int> ProdRepository)
        {
            this.ProdRepository = ProdRepository;
        }

        public async Task<IActionResult> Index()
        {
            var cats = await ProdRepository.GetAsync();
            return View(cats);
        }

        public IActionResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            // check if the Cateogry Posted Model is valid
            if (ModelState.IsValid)
            {
                // create a new Product Record
                product = await ProdRepository.CraeteAsync(product);
                // redirect to Index Page
                return RedirectToAction("Index");
            }
            else
            {
                // else stey on the same page with errors
                return View(product);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            // search the record being edited
            var cat = await ProdRepository.GetAsync(id);
            // return a view with record being edited
            return View(cat);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            // check if the Cateogry Posted Model is valid
            if (ModelState.IsValid)
            {
                // update the Product Record
                product = await ProdRepository.UpdateAsync(id, product);
                // redirect to Index Page
                return RedirectToAction("Index");
            }
            else
            {
                // else stey on the same page with errors
                return View(product);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            // search the record being edited
            var res = await ProdRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
