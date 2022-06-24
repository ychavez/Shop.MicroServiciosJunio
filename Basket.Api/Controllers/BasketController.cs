using AutoMapper;
using Basket.Api.Entities;
using Basket.Api.Repositories;
using EventBus.Messages.Events;
using Existence.Grpc.Protos;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IMapper mapper;
        private readonly ExistenceService.ExistenceServiceClient client;

        public BasketController(IBasketRepository basketRepository, 
            IPublishEndpoint publishEndpoint,
            IMapper mapper,
            ExistenceService.ExistenceServiceClient client)
        {
            this.basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            this.publishEndpoint = publishEndpoint;
            this.mapper = mapper;
            this.client = client;
        }

        [HttpGet("TestGrpc")]
        public ActionResult<int> testGrpc() 
            => client.CheckExistence(new ProductRequest { Id = "algo" }).ProductQty;

        [HttpGet("{userName}")]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await basketRepository.GetBasket(userName);

            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpDelete("{userName}")]
        public async Task<ActionResult<ShoppingCart>> DeleteBasket(string userName)
        {
            await basketRepository.DeleteBasket(userName);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart shoppingCart)
        {
            await basketRepository.UpdateBasket(shoppingCart);

            return Ok(shoppingCart);
        }

        [HttpPost("Checkout")]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await basketRepository.GetBasket(basketCheckout.UserName);

            if (basket == null)
                return BadRequest();

            /// enviar el mensaje a RabbitMQ
            var eventMessage = mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;

            await publishEndpoint.Publish(eventMessage);

            await basketRepository.DeleteBasket(basket.UserName);

            return Accepted();

           
        }

    }
}
