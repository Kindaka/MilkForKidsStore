using MilkStore_BAL.ModelViews.FeedbackDTOs;

namespace MilkStore_BAL.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<IList<FeedbackDtoResponse>> GetAllFeedbackOfProduct(int productId);
        Task<FeedbackDtoRequest> CreateFeedback(FeedbackDtoRequest request);
        Task<FeedbackDtoResponse> GetRatingProduct(int productId);
        Task<FeedbackDtoResponse> GetOneFb(int productId, int accountId);
        Task<FeedbackDtoResponse> UpdateFeedback(int feedbackId, FeedbackDtoRequest request);
        Task<bool> DeleteFeedback(int feedbackId, int accountId);
        Task<bool> UpdateStsAdmin(int feedbackId, UpdateFeedbackDtoRequest request);
    }
}
