using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public string Vendor { get; set; }
        public string DeliveryDate { get; set; }
        public ProductViewModelFields ToFields()
        {
            return new ProductViewModelFields()
            {
                DeliveryDate = DeliveryDate,
                Id = Id,
                ProductName = ProductName,
                Vendor = Vendor
            };
        }
    }
}
