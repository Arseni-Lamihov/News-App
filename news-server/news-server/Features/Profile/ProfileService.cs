﻿using Microsoft.EntityFrameworkCore;
using news_server.Data;
using news_server.Data.dbModels;
using news_server.Features.News;
using news_server.Features.Profile.Models;
using news_server.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace news_server.Features.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly NewsDbContext context;
        private readonly INewsService newsService;

        public ProfileService(NewsDbContext context, INewsService newsService)
        {
            this.context = context;
            this.newsService = newsService;
        }

        public async Task<List<GetUserPmodel>> GetUserByName(string username)
        {
            var result = await context
                .Profiles
                .Include(p => p.User)
                .Where(p => !p.isBaned && p.User.UserName != username)
                .Select(p => new GetUserPmodel
                {
                    ProfileID = p.Id,
                    UserName = p.User.UserName
                })
                .ToListAsync();

            return result;
        }

        public async Task<GetProfileById> GetProfileById(int profileId)
        {
            var result = await Task.Run( () => context
                .Subscriptions
                .Include(s => s.Profile)
                .Include(s => s.Profile.User)
                .Select(s => new GetProfileById 
                { 
                    ProfileId = s.ProfileId,
                    UserName = s.Profile.User.UserName,
                    UserNews = newsService.GetProfileNews(s.ProfileId)
                })
                .FirstOrDefault(s => s.ProfileId == profileId));

            return result;
        }
    }
}
