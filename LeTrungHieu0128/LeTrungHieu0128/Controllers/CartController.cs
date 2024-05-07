using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using LeTrungHieu0128.Models;

namespace LeTrungHieu0128.Controllers
{
    public class CartController : Controller
    {
        NorthwindDataContext da = new NorthwindDataContext();
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
        public ActionResult OrderProduct()
        {
            using (TransactionScope tranScope = new TransactionScope())
            {
                try
                {
                    //1. Tao mới 1 đơn hàng luu trong orders: chi them ngay dat hang
                    Order order = new Order();
                    order.OrderDate = DateTime.Now;
                    da.Orders.InsertOnSubmit(order);
                    da.SubmitChanges();

                    //2.Có bao nhiêu SP tạo moi bấy nhiêu dòng trong orderdetails 
                    //21 Lay ds cac sp trong gio hang
                    List<CartModel> carts = GetListCarts(); // Get the list of items in the cart

                    foreach (var item in carts)
                    {
                        //Tao moi orderdetals
                        Order_Detail d = new Order_Detail();

                        //Thiet lap cac thuoc tinh
                        d.OrderID = order.OrderID;
                        d.ProductID = item.ProductID;
                        d.Quantity = short.Parse(item.Quantity.ToString());
                        d.UnitPrice = decimal.Parse(item.UnitPrice.ToString());
                        d.Discount = 0;

                        // Add to order details
                        da.Order_Details.InsertOnSubmit(d);
                    }

                    // Save changes
                    da.SubmitChanges();

                    // Clear the cart
                    Session["CartModel"] = null;

                    tranScope.Complete();
                    return RedirectToAction("OrderDetailsList");
                }
                catch (Exception)
                {
                    tranScope.Dispose();
                    return RedirectToAction("ListCarts");
                }
            }
            
        }
        public ActionResult OrderDetailsList()
        {
            var ds = da.Order_Details.OrderByDescending(s => s.OrderID).ToList();
            return View(ds);
        }
    }
}
