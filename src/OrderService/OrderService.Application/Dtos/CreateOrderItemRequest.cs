﻿namespace OrderService.Application.Dtos
{
    public class CreateOrderItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        //public int UnitPrice { get; set; }
    }
}
