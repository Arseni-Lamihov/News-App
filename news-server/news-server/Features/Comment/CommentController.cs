﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using news_server.Features.Comment.Models;
using news_server.Infrastructure.Extensions;
using news_server.Infrastructure.Filter;
using System.Threading.Tasks;

namespace news_server.Features.Comment
{
    [Authorize]
    public class CommentController: ApiController
    {
        private readonly ICommentService commentService;

        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpPost]
        [ServiceFilter(typeof(BanFilter))]
        public async Task<ActionResult> CreateComment(CommentCreateModel model)
        {
            var username = User.GetUserName();
            string link = Url
                  .Action(
                      "GetNewsById",
                      "News",
                      new { newsId = model.NewsId },
                   protocol: HttpContext.Request.Scheme);

            var commentIdTo = model.CommentIdTo;
            var result = await commentService.CreateComment(model, username, link, commentIdTo);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut(nameof(EditComment))]
        public async Task<ActionResult> EditComment(EditCommentModel model)
        {
            var username = User.GetUserName();
            var comment = await commentService.EditComment(
                model.CommentId,
                username,
                model.Text);
            if (comment)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
