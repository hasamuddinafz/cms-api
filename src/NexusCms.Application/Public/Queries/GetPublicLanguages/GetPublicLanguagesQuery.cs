using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Public.Queries.GetPublicLanguages;

public record GetPublicLanguagesQuery : IRequest<Result<List<PublicLanguageDto>>>;
