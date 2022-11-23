using Contracts;
using Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class VendorStorage
    {
        public List<Vendor> GetFullList()
        {
            using (var context = new ProductDatabase())
            {
                return context.Vendors
            .Select(v=>v)
            .ToList();
            };

        }
        public List<Vendor> GetFiltredlList(Vendor vendor)
        {
            using (var context = new ProductDatabase())
            {
                return context.Vendors.Where(v=>vendor.VendorName.Equals(v.VendorName))
            .Select(v => v)
            .ToList();
            };

        }
        public Vendor GetElement(Vendor model)
        {
            if (model is null)
            {
                return null;
            }
            using (var context = new ProductDatabase())
            {
                var vendor = context.Vendors
                .FirstOrDefault(rec => rec.VendorName == model.VendorName || rec.Id == model.Id);
                return vendor is null ? null :
                new Vendor
                {
                    Id = vendor.Id,
                    VendorName = vendor.VendorName
                };
            }
        }
        public void Insert(Vendor model)
        {
            using (var context = new ProductDatabase())
            {
                context.Vendors.Add(model);
               
                context.SaveChanges();
            };

        }
        public void Update(Vendor model)
        {
            using (var context = new ProductDatabase())
            {
                var element = context.Vendors.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                element.VendorName = model.VendorName;
                context.SaveChanges();
            };
            
        }
        public void Delete(Vendor model)
        {
            using (var context = new ProductDatabase())
            {
                Vendor element = context.Vendors.FirstOrDefault(rec => rec.Id ==
                          model.Id);
                if (element != null)
                {
                    context.Vendors.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            };

        }
    }
}
