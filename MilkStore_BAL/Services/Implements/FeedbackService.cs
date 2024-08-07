﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using MilkStore_BAL.ModelViews.FeedbackDTOs;
using MilkStore_BAL.Services.Interfaces;
using MilkStore_DAL.Entities;
using MilkStore_DAL.UnitOfWorks.Interfaces;

namespace MilkStore_BAL.Services.Implements
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper,
                                IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<FeedbackDtoRequest> CreateFeedback(FeedbackDtoRequest request)
        {
            var shopExist = _unitOfWork.ProductRepository.FindAsync(x => x.ProductId == request.ProductId);
            var orderExist = _unitOfWork.OrderDetailRepository.GetAsync(x => x.ProductId == request.ProductId && x.Order.CustomerId == request.CustomerId).Result;
            if( shopExist == null || orderExist.Count() == 0)
            {
                return null;
            }
            var map = _mapper.Map<Feedback>(request);
            map.Status = true;
            map.RateNumber = 0;
            await _unitOfWork.FeedbackRepository.AddAsync(map);
            await Task.Delay(500);
            await _unitOfWork.SaveAsync();

            var response = _mapper.Map<FeedbackDtoRequest>(map);

            return response;
        }

        public async Task<bool> DeleteFeedback(int feedbackId, int accountId)
        {
            var verifyAccountId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("AccountId")?.Value);

            if (verifyAccountId != accountId)
            {
                return false;
            }
            Customer accountExist = (Customer)await _unitOfWork.CustomerRepository.FindAsync(x => x.AccountId == verifyAccountId);

            var fbExist = await _unitOfWork.FeedbackRepository.GetAsync(filter: x => x.FeedbackId == feedbackId 
                                                                            && x.CustomerId == accountExist.CustomerId);
            var final = fbExist.FirstOrDefault();

            if (final == null)
            {
                return false;
            }

            await _unitOfWork.FeedbackRepository.DeleteAsync(final.FeedbackId);
            await Task.Delay(500);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<IList<FeedbackDtoResponse>> GetAllFeedbackOfProduct(int productId)
        {
            var getAllFbOShop = (await _unitOfWork.FeedbackRepository
                                            .GetAsync(filter: x => x.ProductId == productId
                                                                && x.Status == true, 
                                                                includeProperties: "Product,Customer")).ToList();

            var response = _mapper.Map<IList<FeedbackDtoResponse>>(getAllFbOShop);

            return response;
        }

        public async Task<FeedbackDtoResponse> GetRatingProduct(int productId)
        {
            var verifyAccountId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("AccountId")?.Value);
            var accountExist =await _unitOfWork.CustomerRepository.SingleOrDefaultAsync(x => x.AccountId == verifyAccountId);
            var totalRateOfShop = await _unitOfWork.FeedbackRepository
                                                .GetAsync(filter: x => x.ProductId == productId
                                                                    && x.Status == true);
            var getRateUrSelf = await _unitOfWork.FeedbackRepository
                                                .GetAsync(filter: x => x.ProductId == productId
                                                && x.CustomerId == accountExist.CustomerId
                                                                    && x.Status == true,
                                                          includeProperties: "Product");

            var getFirst = getRateUrSelf.FirstOrDefault();

            var averageNum = totalRateOfShop.Any() ? totalRateOfShop.Average(x => x.RateNumber) : (double?)null;

            var response = _mapper.Map<FeedbackDtoResponse>(getFirst);
            if (getFirst != null)
            {
                response.RateNumber = getFirst.RateNumber;
            }
            else
            {
                response.RateNumber = 0;
            }
            response.AverageNumber = averageNum;

            return response;
        }


        public async Task<FeedbackDtoResponse> GetOneFb(int productId, int accountId)
        {
            var getAll = await _unitOfWork.FeedbackRepository
                                            .GetAsync(filter: x => x.ProductId == productId
                                                                && x.Status != true
                                                                && x.CustomerId == accountId,
                                                                includeProperties: "Product,Customer");
            var final = getAll.FirstOrDefault();
            var response = _mapper.Map<FeedbackDtoResponse>(final);
            return response;
        }

        public async Task<FeedbackDtoResponse> UpdateFeedback(int feedbackId, FeedbackDtoRequest request)
        {
            var verifyAccountId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("AccountId")?.Value);
            var accountExist =await _unitOfWork.CustomerRepository.SingleOrDefaultAsync(x => x.AccountId == verifyAccountId);

            if (verifyAccountId != request.CustomerId)
            {
                return null;
            }

            var getAll = await _unitOfWork.FeedbackRepository
                                            .GetAsync(filter: x => x.ProductId == request.ProductId
                                                                && x.CustomerId == accountExist.CustomerId
                                                                && x.FeedbackId == feedbackId,
                                                                includeProperties: "Product,Customer");
            var final = getAll.FirstOrDefault();

            if(final == null)
            {
                return null;
            }

            var map = _mapper.Map(request, final);
            map.Status = true;

            await _unitOfWork.FeedbackRepository.UpdateAsync(map);
            await Task.Delay(500);
            await _unitOfWork.SaveAsync();

            var response = _mapper.Map<FeedbackDtoResponse>(map);

            return response;

        }

        public async Task<bool> UpdateStsAdmin(int feedbackId, UpdateFeedbackDtoRequest request)
        {
            var getAll = await _unitOfWork.FeedbackRepository
                                            .GetAsync(filter: x => x.FeedbackId == feedbackId);

            var final = getAll.FirstOrDefault();

            if(final == null)
            {
                return false;
            }

            var map = _mapper.Map(request,final);
            await _unitOfWork.FeedbackRepository.UpdateAsync(map);
            await Task.Delay(500);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
