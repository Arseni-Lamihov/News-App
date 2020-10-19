﻿using Microsoft.EntityFrameworkCore;
using news_server.Data;
using news_server.Data.dbModels;
using news_server.Features.Notify.Model;
using news_server.Features.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CProfile = news_server.Data.dbModels.Profile;

namespace news_server.Features.Notify
{
    public class NotificationService : INotificationService
    {
        private readonly NewsDbContext context;

        public NotificationService(NewsDbContext context)
        {
            this.context = context;
        }
               
        public async Task<List<GetNotificationsModel>> GetNotifications(string username, int page)
        {
            var user = await context
                .Users
                .FirstOrDefaultAsync(u => u.UserName == username);

            var profileTo = await context
                .Profiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            var result = await context
                .Notifications
                .Where(n => n.Profile == profileTo)
                .Select(n => new GetNotificationsModel 
                {
                    Id = n.Id,
                    Url = n.Url,
                    Alt = n.Alt,
                    NotificationText = n.NotificationText,
                    NotificationDate = n.NotificationDate,
                    CommentId = n.CommentId
                })
                .OrderBy(n => n.NotificationDate)
                .Skip(page * 20 - 20)
                .Take(20)
                .ToListAsync();

            return result;
        }
                
        public async Task AddNotification(
            CProfile profileTo, 
            int profileFrom, 
            string text,
            string link,
            string alt,
            int? commentId = null)
        {
            var notification = new Notification 
            {
                NotificationDate = DateTime.Now,
                ProfileIdFrom = profileFrom,
                Profile = profileTo,
                NotificationText = text,
                Url = link,
                Alt = alt,
                CommentId = commentId
            };

            await context.Notifications.AddAsync(notification);
            await context.SaveChangesAsync();
        }
    }
}