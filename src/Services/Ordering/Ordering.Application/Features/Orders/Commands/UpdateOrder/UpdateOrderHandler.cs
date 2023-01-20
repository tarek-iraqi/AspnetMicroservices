using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder;

internal class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateOrderHandler> _logger;

    public UpdateOrderHandler(IOrderRepository orderRepository,
        IMapper mapper,
        ILogger<UpdateOrderHandler> logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetByIdAsync(request.Id);

        if (orderToUpdate == null)
            throw new NotFoundException(nameof(Order), request.Id);

        _mapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Order));

        await _orderRepository.UpdateAsync(orderToUpdate);

        _logger.LogInformation($"Order with id = {request.Id} is updated successfully");

        return Unit.Value;
    }
}
