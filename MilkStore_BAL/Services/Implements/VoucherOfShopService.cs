using AutoMapper;
using Azure.Core;
using MilkStore_BAL.ModelViews.VoucherOfShopDTOs;
using MilkStore_BAL.Services.Interfaces;
using MilkStore_DAL.Entities;
using MilkStore_DAL.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Implements
{
    public class VoucherOfShopService : IVoucherOfShopService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VoucherOfShopService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<VoucherOfShopDtoResponse>> Get()
        {
            try
            {
                var response = new List<VoucherOfShopDtoResponse>();
                var vouchers = await _unitOfWork.VoucherOfShopRepository.GetAsync(filter: v => v.Status == true);
                if (vouchers.Any()) {
                    foreach (var voucher in vouchers) {
                        response.Add(_mapper.Map<VoucherOfShopDtoResponse>(voucher));
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<VoucherOfShopDtoResponse?> Get(int id)
        {
            try
            {
                var voucher = (await _unitOfWork.VoucherOfShopRepository.GetAsync(filter: v => v.Status == true && v.VoucherId == id)).FirstOrDefault();
                if(voucher == null)
                {
                    return null;
                }
                return _mapper.Map<VoucherOfShopDtoResponse>(voucher);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<VoucherOfShopDtoResponseForAdmin>> GetByAdmin()
        {
            try
            {
                var response = new List<VoucherOfShopDtoResponseForAdmin>();
                var vouchers = await _unitOfWork.VoucherOfShopRepository.GetAllAsync();
                if (vouchers.Any())
                {
                    foreach (var voucher in vouchers)
                    {
                        response.Add(_mapper.Map<VoucherOfShopDtoResponseForAdmin>(voucher));
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<VoucherOfShopDtoResponseForAdmin?> GetByAdmin(int id)
        {
            try
            {
                var voucher = await _unitOfWork.VoucherOfShopRepository.GetByIDAsync(id);
                if (voucher == null)
                {
                    return null;
                }
                return _mapper.Map<VoucherOfShopDtoResponseForAdmin>(voucher);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Post(VoucherOfShopDtoRequest request)
        {
            try
            {
                var voucher = _mapper.Map<VoucherOfShop>(request);
                voucher.Status = true;
                await _unitOfWork.VoucherOfShopRepository.AddAsync(voucher);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Put(int id, VoucherOfShopDtoRequest request)
        {
            try
            {
                var voucher = await _unitOfWork.VoucherOfShopRepository.GetByIDAsync(id);
                if (voucher == null) {
                    return false;
                }
                voucher.VoucherValue = request.VoucherValue;
                voucher.VoucherQuantity = request.VoucherQuantity;
                voucher.StartDate = request.StartDate;
                voucher.EndDate = request.EndDate;
                await _unitOfWork.VoucherOfShopRepository.UpdateAsync(voucher);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateStatus(int id, bool status)
        {
            try
            {
                var voucher = await _unitOfWork.VoucherOfShopRepository.GetByIDAsync(id);
                if (voucher == null) {
                    return false;
                }
                voucher.Status = status;
                await _unitOfWork.VoucherOfShopRepository.UpdateAsync(voucher);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
