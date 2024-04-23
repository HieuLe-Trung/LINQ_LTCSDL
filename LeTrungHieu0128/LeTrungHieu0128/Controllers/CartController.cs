using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeTrungHieu0128.Models;

namespace LeTrungHieu0128.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        private List<CartModel> GetListCarts()//lấy ds sp trong giỏ hàng
        {
            List<CartModel> carts = Session["CartModel"] as List<CartModel>;
            //2 có sp nào chưa, chưa có thì tạo mới giỏ hàng
            if (carts == null)
            {
                carts = new List<CartModel>();
                Session["CartModel"] = carts;
            }
            return carts;
        }
        public ActionResult ListCarts()
        {
            List<CartModel> carts = GetListCarts();
            ViewBag.CountProduct = carts.Sum(s => s.Quantity);
            ViewBag.Total = carts.Sum(s => s.Total);
            return View(carts);
        }
        public ActionResult AddToCart(int id)
        {
            List<CartModel> carts = GetListCarts();
            CartModel c = carts.Find(s => s.ProductID == id);
            if (c != null)
            {
                c.Quantity++;
            }
            else
            {
                c = new Models.CartModel(id);
                carts.Add(c);
            }
            return RedirectToAction("ListCarts");
        }
        // GET: Cart/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Cart/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cart/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cart/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cart/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cart/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cart/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
