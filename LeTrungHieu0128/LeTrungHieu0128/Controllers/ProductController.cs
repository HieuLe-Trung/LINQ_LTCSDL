using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeTrungHieu0128.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        NorthwindDataContext da = new NorthwindDataContext();
        public ActionResult ListProducts()
        {
            var ds = da.Products.OrderByDescending(s=>s.ProductID).ToList();
            return View(ds);
        }
        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            Product p = da.Products.FirstOrDefault(s => s.ProductID == id);
            return View(p);
        }
        
        // GET: Product/Create
        public ActionResult Create()
        {
            ViewData["NCC"] = new SelectList(da.Suppliers, "SupplierID", "CompanyName");
            ViewData["LSP"] = new SelectList(da.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, Product p)
        {
            try
            {
                //Tạo mới 1 SP product
                Product product = new Product();
                //gán thuộc tính cần thêm cho sp product
                product = p;
                product.SupplierID = int.Parse(collection["NCC"]);
                product.CategoryID = int.Parse(collection["LSP"]);
                //XỬ LÝ THÊM product vào bảng Products
                da.Products.InsertOnSubmit(product);
                //cập nhật thay đổi xuống db
                da.SubmitChanges();
                return RedirectToAction("ListProducts");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            ViewData["NCC"] = new SelectList(da.Suppliers, "SupplierID", "CompanyName");
            ViewData["LSP"] = new SelectList(da.Categories, "CategoryID", "CategoryName");
            var p = da.Products.FirstOrDefault(s => s.ProductID == id);
            return View(p);
        }
        //thực hiện sửa
        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // Lấy product muốn sửa từ db
                Product product = da.Products.FirstOrDefault(s => s.ProductID == id);
                //gán cá thuộc tính cần sửa cho sp product
                product.ProductName = collection["ProductName"];
                product.UnitPrice = decimal.Parse(collection["UnitPrice"]);
                product.SupplierID = int.Parse(collection["NCC"]);
                product.CategoryID = int.Parse(collection["LSP"]);
                product.UnitsInStock = short.Parse(collection["UnitsInstock"]);

                //cập nhật thay đổi xuống db
                da.SubmitChanges();
                return RedirectToAction("ListProducts");
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            var p = da.Products.FirstOrDefault(s => s.ProductID == id);
            return View(p);
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // lấy product muốn xóa từ db
                Product product = da.Products.First(s => s.ProductID == id);
                da.Products.DeleteOnSubmit(product);
                da.SubmitChanges();
                return RedirectToAction("ListProducts");
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
