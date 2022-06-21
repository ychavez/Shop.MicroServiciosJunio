﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;

namespace Ordering.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediator;

        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdersViewModel>>> GetOrders([FromQuery] string userName)
        {
            var query = new GetOrdersListQuery(userName);
            return await mediator.Send(query);
        }
    }
}
