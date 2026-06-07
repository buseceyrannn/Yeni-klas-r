using BUSEB.Data;
using BUSEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BUSEB.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "Cart";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🛒 SEPETE EKLE
        public IActionResult AddToCart(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            var cartJson = HttpContext.Session.GetString(CartSessionKey);

            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            var existing = cart.FirstOrDefault(x => x.ProductId == id);

            if (existing != null)
                existing.Quantity++;
            else
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    ImageUrl = product.ImageUrl
                });

            HttpContext.Session.SetString(CartSessionKey, JsonConvert.SerializeObject(cart));

            return RedirectToAction("Index", "Product");
        }

        // 🧾 SEPETİ GÖR
        public IActionResult Index()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);

            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            return View(cart);
        }

        // ❌ ÜRÜN SİL
        public IActionResult Remove(int id)
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);

            if (string.IsNullOrEmpty(cartJson))
                return RedirectToAction("Index");

            var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
                cart.Remove(item);

            HttpContext.Session.SetString(CartSessionKey, JsonConvert.SerializeObject(cart));

            return RedirectToAction("Index");
        }

        // 📦 CHECKOUT (FORM AÇ)
        [HttpGet]
        public IActionResult Checkout()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);

            if (string.IsNullOrEmpty(cartJson))
                return RedirectToAction("Index");

            var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            if (cart == null || !cart.Any())
                return RedirectToAction("Index");

            return View(); // form sayfası
        }

        // 📦 CHECKOUT (SİPARİŞ OLUŞTUR)
        [HttpPost]
        public IActionResult Checkout(CheckoutViewModel model)
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);

            if (string.IsNullOrEmpty(cartJson))
                return RedirectToAction("Index");

            var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            if (!ModelState.IsValid)
                return View(model);

            var order = new Order
            {
                CreatedAt = DateTime.Now,
                TotalPrice = cart.Sum(x => x.Price * x.Quantity),
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in cart)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            HttpContext.Session.Remove(CartSessionKey);

            return RedirectToAction("Success");
        }

        // 🎉 BAŞARI SAYFASI
        public IActionResult Success()
        {
            return View();
        }

        // 📜 SİPARİŞLER
        public IActionResult Orders()
        {
            var orders = _context.Orders
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            return View(orders);
        }
    }
}