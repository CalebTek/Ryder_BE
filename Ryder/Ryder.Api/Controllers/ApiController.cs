using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NLog;
using Ryder.Infrastructure.Common;
using Ryder.Infrastructure.Common.Exceptions;
using System.Net;
using IResult = AspNetCoreHero.Results.IResult;
using ValidationException = Ryder.Infrastructure.Common.Exceptions.ValidationException;

namespace Ryder.Api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/v1/[Controller]")]
    [ApiController]
    public class ApiController : Controller
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected IActionResult BadRequestResponse(string message)
        {
            return BadRequest(new Result<string> { Message = message });
        }

        protected IActionResult ForbiddenResponse(string message)
        {
            return BadRequest(new Result<string> { Message = message });
        }

        protected IActionResult ServerErrorResponse(string message)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new Result<string> { Message = message });
        }

        protected async Task<IActionResult> Initiate<TOut>(Func<Task<IResult<TOut>>> action)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetErrorsAsList(ModelState));

                var result = await action.Invoke();
                if (result.Succeeded)
                    return Ok(result);

                return BadRequest(result);
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex, ex.Message);
                return BadRequestResponse(ex.Message);
            }
            catch (ValidationException ex)
            {
                _logger.Error(ex, ex.Message);
                return BadRequestResponse(string.Join('\n', ex.Errors));
            }
            catch (ForbiddenAccessException ex)
            {
                _logger.Error(ex, ex.Message);
                return ForbiddenResponse("Forbidden");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return ServerErrorResponse(Constants.InternalServerErrorMessage);
            }
        }

        protected async Task<IActionResult> Initiate(Func<Task<IResult>> action)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(GetErrorsAsList(ModelState));

                var result = await action.Invoke();
                if (result.Succeeded)
                    return Ok(result);

                return BadRequest(result);
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex, ex.Message);
                return BadRequestResponse(ex.Message);
            }
            catch (ValidationException ex)
            {
                _logger.Error(ex, ex.Message);
                return BadRequestResponse(string.Join('\n', ex.Errors));
            }
            catch (ForbiddenAccessException ex)
            {
                _logger.Error(ex, ex.Message);
                return ForbiddenResponse("Forbidden");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return ServerErrorResponse(Constants.InternalServerErrorMessage);
            }
        }

        public static List<string> GetErrorsAsList(ModelStateDictionary? modelState)
        {
            if (modelState == null || !modelState.Values.Any())
                return new List<string>();

            IList<string> allErrors = modelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)).ToList();

            var err = allErrors.Where(error => !string.IsNullOrEmpty(error)).ToList();

            if (err.Count == 0)
                err = modelState.Values.SelectMany(v => v.Errors.Select(b => b.Exception.Message)).ToList();

            return err;
        }
    }
}