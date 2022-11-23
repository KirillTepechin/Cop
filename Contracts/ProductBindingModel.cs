
using System;
using System.Collections.Generic;

namespace Contracts
{
    public class ProductBindingModel
    {
        public int? Id { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public string Vendor { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}