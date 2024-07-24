using MilkStore_BAL.BackgroundServices.Interfaces;
using MilkStore_DAL.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.BackgroundServices.Implements
{
    public class ProductBackgroundService : IProductBackgroundService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductBackgroundService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task RemoveHiddenProductInCustomerCarts()
        {
            try
            {
                var hiddenProducts = await _unitOfWork.ProductRepository.GetAllAsync(p => p.ProductStatus == false);
                if (hiddenProducts.Any())
                {
                    foreach (var product in hiddenProducts)
                    {
                        var productInCarts = await _unitOfWork.CartRepository.GetAllAsync(c => c.ProductId == product.ProductId);
                        if (productInCarts.Any())
                        {
                            await _unitOfWork.CartRepository.DeleteRangeAsync(productInCarts);
                            await _unitOfWork.SaveAsync();
                        }
                    }
                }
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
