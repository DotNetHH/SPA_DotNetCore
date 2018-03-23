using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Models.Requests;
using WebAPI.RequestPipeline;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class RequestsController : Controller
    {
        private readonly IMediator mediator;
        public RequestsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(AlleTodos.Name)]
        public Task<IActionResult> GetAlleTodos() => this.CreateResponse(new AlleTodos());

        [HttpPost(GetTodo.Name)]
        public Task<IActionResult> GetTodoByID([FromBody] GetTodo request) => this.CreateResponse(request);

        [HttpPost(TodoAnlegen.Name)]
        public Task<IActionResult> Post([FromBody] TodoAnlegen request) => this.CreateResponse(request);

        [HttpPost(TodoAendern.Name)]
        public Task<IActionResult> Post([FromBody] TodoAendern request) => this.CreateResponse(request);

        [HttpPost(GetTodoArten.Name)]
        public Task<IActionResult> Post([FromBody] GetTodoArten request) => this.CreateResponse(request);

        public async Task<IActionResult> CreateResponse<TResponse>(IRequest<TResponse> request)
        {
            try
            {
                var data = await this.mediator.Send(request);
                return this.Ok(data);
            }
            catch (AuthorizationException)
            {
                return Unauthorized();
                // TODO eigentlich ist 403 richtig!?
                // return Forbid();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // [HttpPost]
        // public async Task<IActionResult> Post([FromBody] ValueRequest request)
        // {
        //     if (request == null)
        //     {
        //         var error = new Error
        //         {
        //             Message = "A bad request was received.",
        //             Details = new[] { new ErrorDetail { Message = "The body of the request contained no usable content." } }
        //         };
        //         return BadRequest(error);
        //     }

        //     try
        //     {
        //         // var message = new ValueRequest { Text = "Hallo" };
        //         var data = await this.mediator.Send(request);
        //         return this.Ok(data);
        //     }
        //     catch (AuthorizationException)
        //     {
        //         return Unauthorized();
        //         // TODO eigentlich ist 403 richtig!?
        //         // return Forbid();
        //     }
        //     catch (ValidationException ex)
        //     {
        //         return BadRequest(ex.Errors);
        //     }
        //     catch (Exception)
        //     {

        //         throw;
        //     }
        // }

    }
}
