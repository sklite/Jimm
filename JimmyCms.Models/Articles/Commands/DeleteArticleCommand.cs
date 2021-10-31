﻿using System;
using MediatR;

namespace JimmyCms.Domain.Articles.Commands
{
    public record DeleteArticleCommand : IRequest<BasicResponse>
    {
        public Guid Id { get; }

        public DeleteArticleCommand(Guid id)
        {
            Id = id;
        }
    }
}