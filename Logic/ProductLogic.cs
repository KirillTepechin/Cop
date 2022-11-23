using Contracts;
using Database;
using System;
using System.Collections.Generic;

namespace Logic
{
    public class ProductLogic
    {
        private readonly ProductStorage _productStorage;
        public ProductLogic(ProductStorage productStorage)
        {
            _productStorage = productStorage;
        }
        public List<ProductViewModel> Read(ProductBindingModel model)
        {
            if (model == null)
            {
                return _productStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<ProductViewModel> { _productStorage.GetElement(model) };
            }
            return _productStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(ProductBindingModel model)
        {
            var element = _productStorage.GetElement(new ProductBindingModel
            {
                ProductName = model.ProductName
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть продукт с таким названием");
            }
            if (model.Id.HasValue)
            {
                _productStorage.Update(model);
            }
            else
            {
                _productStorage.Insert(model);
            }
        }
        public void Delete(ProductBindingModel model)
        {
            var element = _productStorage.GetElement(new ProductBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _productStorage.Delete(model);
        }
    }
}