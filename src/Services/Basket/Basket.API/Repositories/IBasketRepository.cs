﻿using Basket.API.Entities;

namespace Basket.API.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasketAsync(string username);
    Task<ShoppingCart> UpdateBasketAsync(ShoppingCart cart);
    Task DeleteBasketAsync(string username);
}
