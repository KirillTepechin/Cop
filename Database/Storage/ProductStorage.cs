using Contracts;
using Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class ProductStorage
    {
        public void Delete(ProductBindingModel model)
        {
            using (var context = new ProductDatabase())
            {
                Product element = context.Products.FirstOrDefault(rec => rec.Id ==
                          model.Id);
                if (element != null)
                {
                    context.Products.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            } ;
           
        }

        public ProductViewModel GetElement(ProductBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new ProductDatabase())
            {
                var component = context.Products
            .FirstOrDefault(rec => rec.ProductName == model.ProductName || rec.Id
           == model.Id);
                return component != null ? CreateModel(component) : null;
            } ;
            

        }

        public List<ProductViewModel> GetFilteredList(ProductBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new ProductDatabase())
            {
                return context.Products
            .Where(rec => rec.ProductName.Contains(model.ProductName))
            .Select(CreateModel)
            .ToList();
            } ;
            

        }

        public List<ProductViewModel> GetFullList()
        {
            using (var context = new ProductDatabase())
            {
                return context.Products
            .Select(CreateModel)
            .ToList();
            } ;
            
        }

        public void Insert(ProductBindingModel model)
        {
            using (var context = new ProductDatabase())
            {
                context.Products.Add(CreateModel(model, new Product(), context));
                context.SaveChanges();
            } ;
           
        }

        public void Update(ProductBindingModel model)
        {
            using (var context = new ProductDatabase())
            {
                var element = context.Products.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element, context);
                context.SaveChanges();
            } ;
           
        }
        private static Product CreateModel(ProductBindingModel model, Product product, ProductDatabase context)
        {
            product.ProductName = model.ProductName;
            product.DeliveryDate = model.DeliveryDate;
            product.Vendor = context.Vendors.Where(v => model.Vendor.Equals(v.VendorName)).Select(v=>v.VendorName).First();
            product.Image = model.Image;
            
            return product;
        }
        private static ProductViewModel CreateModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                DeliveryDate = product.DeliveryDate.ToShortDateString(),
                Image = product.Image,
                Vendor = product.Vendor,
            };
        }
    }
}
