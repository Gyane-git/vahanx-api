using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VahanX.Application.Common;

namespace VahanX.Api.Filters;

/// <summary>
/// Action filter that validates FluentValidation results before executing the action.
/// </summary>
public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(kvp => kvp.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

            var apiErrors = errors
                .SelectMany(kvp => kvp.Value.Select(v => new ApiError
                {
                    Code = "VALIDATION_ERROR",
                    Field = kvp.Key,
                    Message = v
                }))
                .ToList();

            var response = ApiResponse<object>.ErrorResponse("Validation failed", apiErrors);

            context.Result = new BadRequestObjectResult(response);
            return;
        }

        await next();
    }
}
