using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Assets.Commands.UpdateAltText;

/// <summary>AltTextJson: { "tr": "...", "en": "..." } — only meaningful for Image type.</summary>
public record UpdateAltTextCommand(Guid Id, string AltTextJson) : IRequest<Result>;
