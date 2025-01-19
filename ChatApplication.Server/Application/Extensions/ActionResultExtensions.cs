using ChatApplication.Server.Domain.DTOs.Common;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.Server.Application.Extensions
{
    public static class ActionResultExtensions
    {
        public static IActionResult ToActionResult(this Result result) =>
            result.IsSuccess ? new OkResult() : new BadRequestObjectResult(result.Failures);

        public static ActionResult ToActionResult<T>(this Result<T> result) =>
            result.IsSuccess ? new OkObjectResult(result.Value) : new BadRequestObjectResult(result.Failures);
    }
}
