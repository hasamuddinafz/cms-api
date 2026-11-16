using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexusCms.Api.Authorization;
using NexusCms.Application.Assets.Commands.DeleteFile;
using NexusCms.Application.Assets.Commands.UpdateAltText;
using NexusCms.Application.Assets.Commands.UploadFile;
using NexusCms.Application.Assets.Queries.ExportFiles;
using NexusCms.Application.Assets.Queries.GetFileById;
using NexusCms.Application.Assets.Queries.GetFileList;
using NexusCms.Application.Common.Models;
using NexusCms.Domain.Common;

namespace NexusCms.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/files")]
public class FilesController(IMediator mediator) : ControllerBase
{
    /// <summary>POST /api/admin/files/upload — multipart/form-data</summary>
    [HttpPost("upload")]
    [RequirePermission(Permissions.Media.Upload)]
    [RequestSizeLimit(110 * 1024 * 1024)]  // 110 MB hard ceiling (video limit is 100 MB)
    public async Task<IActionResult> Upload(
        IFormFile file,
        [FromForm] string? folder,
        CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "No file selected." });

        await using var stream = file.OpenReadStream();
        var command = new UploadFileCommand(
            stream,
            file.FileName,
            file.ContentType,
            file.Length,
            folder);

        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    /// <summary>GET /api/admin/files</summary>
    [HttpGet]
    [RequirePermission(Permissions.Media.View)]
    public async Task<IActionResult> List([FromQuery] PagedRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new GetFileListQuery(request), ct);
        return Ok(result);
    }

    /// <summary>GET /api/admin/files/export?format=csv|excel</summary>
    [HttpGet("export")]
    [RequirePermission(Permissions.Media.View)]
    public async Task<IActionResult> Export(
        [FromQuery] PagedRequest request,
        [FromQuery] string format = "csv",
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new ExportFilesQuery(request, format), ct);
        return File(result.Content, result.ContentType, result.FileName);
    }

    /// <summary>GET /api/admin/files/{id}</summary>
    [HttpGet("{id:guid}")]
    [RequirePermission(Permissions.Media.View)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetFileByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { error = result.Error });
    }

    /// <summary>DELETE /api/admin/files/{id}</summary>
    [HttpDelete("{id:guid}")]
    [RequirePermission(Permissions.Media.Delete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteFileCommand(id), ct);
        return result.IsSuccess ? NoContent() : BadRequest(new { error = result.Error });
    }

    /// <summary>PUT /api/admin/files/{id}/alt-text</summary>
    [HttpPut("{id:guid}/alt-text")]
    [RequirePermission(Permissions.Media.Upload)]   // same permission tier as "manage media"
    public async Task<IActionResult> UpdateAltText(
        Guid id,
        [FromBody] UpdateAltTextRequest body,
        CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateAltTextCommand(id, body.AltTextJson), ct);
        return result.IsSuccess ? NoContent() : BadRequest(new { error = result.Error });
    }
}

public record UpdateAltTextRequest(string AltTextJson);
