using Firebase.Database;
using Firebase.Database.Query;
using MilkStore_BAL.ModelViews.ChatDTOs;
using MilkStore_BAL.Services.Interfaces;
using MilkStore_DAL.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace MilkStore_BAL.Services.Implements
{
    public class ChatService : IChatService
    {
        private readonly FirebaseClient _firebaseClient;
        private readonly ILogger<ChatService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ChatService(ILogger<ChatService> logger, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _firebaseClient = new FirebaseClient(configuration["Firebase:DatabaseUrl"]);
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateRoomAsync(string roomId)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                _logger.LogInformation("Creating room with ID: {RoomId}", roomId);
                await _firebaseClient.Child("chatrooms").Child(roomId).PutAsync(new { createdAt = DateTime.Now });
                _logger.LogInformation("Room created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating room with ID: {RoomId}", roomId);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation("CreateRoomAsync took {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
            }
        }

        public async Task SendMessageAsync(ChatMessage message)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                // Kiểm tra xem phòng chat có tồn tại không
                var roomExists = await RoomExistsAsync(message.RoomId);
                if (!roomExists)
                {
                    throw new Exception("Room does not exist");
                }

                _logger.LogInformation("Sending message to room ID: {RoomId}", message.RoomId);
                await _firebaseClient.Child("chatrooms").Child(message.RoomId).Child("messages").PostAsync(message);
                _logger.LogInformation("Message sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message to room ID: {RoomId}", message.RoomId);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation("SendMessageAsync took {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
            }
        }

        public async Task<List<ChatMessage>> GetMessagesAsync(string roomId)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                _logger.LogInformation("Getting messages for room ID: {RoomId}", roomId);
                var messages = await _firebaseClient.Child("chatrooms").Child(roomId).Child("messages").OnceAsync<ChatMessage>();
                var result = new List<ChatMessage>();
                foreach (var msg in messages)
                {
                    result.Add(msg.Object);
                }
                _logger.LogInformation("Messages retrieved successfully");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting messages for room ID: {RoomId}", roomId);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation("GetMessagesAsync took {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
            }
        }

        public async Task<bool> RoomExistsAsync(string roomId)
        {
            var room = await _firebaseClient.Child("chatrooms").Child(roomId).OnceSingleAsync<object>();
            return room != null;
        }

        public async Task<bool> IsAdminAsync(int accountId)
        {
            var account = await _unitOfWork.AccountRepository.GetByIDAsync(accountId);
            return account != null && account.RoleId == 1; // Assuming RoleId 1 is for admin
        }
    }
}
