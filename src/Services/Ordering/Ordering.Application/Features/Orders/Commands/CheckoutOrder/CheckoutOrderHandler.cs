using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder;

internal class CheckoutOrderHandler : IRequestHandler<CheckoutOrderCommand, int>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<CheckoutOrderHandler> _logger;

    public CheckoutOrderHandler(IOrderRepository orderRepository,
        IMapper mapper,
        IEmailService emailService,
        ILogger<CheckoutOrderHandler> logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _emailService = emailService;
        _logger = logger;
    }
    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = _mapper.Map<Order>(request);

        var newOrder = await _orderRepository.AddAsync(orderEntity);

        _logger.LogInformation($"Order with id = {newOrder.Id} created successfully");

        await SendEmail(newOrder);

        return newOrder.Id;
    }

    private async Task SendEmail(Order newOrder)
    {
        var email = new Email { To = "tarek.iraqi@gmail.com", Body = "Order created successfully", Subject = "order created" };

        try
        {
            await _emailService.SendEmail(email);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Email send failed to order with id = {newOrder.Id}", ex);
        }
    }
}