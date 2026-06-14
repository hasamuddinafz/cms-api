using MediatR;
using NexusCms.Application.Common.Models;

namespace NexusCms.Application.Contact.Queries.GetContactMessages;

public record GetContactMessagesQuery(
    string? Search,
    bool? IsRead,
    string SortBy = "createdAt",
    string SortDir = "desc",
    int Page = 1,
    int PageSize = 20
) : IRequest<PagedResult<ContactMessageDto>>;

public record ContactMessageDto(
    Guid Id,
    string FullName,
    string Email,
    string? Phone,
    string Subject,
    string Message,
    bool IsRead,
    string? IpAddress,
    DateTime CreatedAt
);
