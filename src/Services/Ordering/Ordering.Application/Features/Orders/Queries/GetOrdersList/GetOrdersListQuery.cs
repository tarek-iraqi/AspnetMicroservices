using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList;

public record GetOrdersListQuery(string username) : IRequest<List<OrderDto>>;
