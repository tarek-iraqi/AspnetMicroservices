﻿using MediatR;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder;

public record DeleteOrderCommand(int id) : IRequest;
