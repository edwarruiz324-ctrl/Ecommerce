namespace Common.Constants
{
    public static class ErrorMessages
    {
        ///// <--Static Messages-->
        ///// Message Product
        public static string ProductDontExits(int productId) =>
            $"El producto con ID {productId} no existe.";
        public static string InvalidProductPrice(int productId) => 
            $"El precio del producto con ID {productId} no es valido.";

        public static string ProductOutOfStock(int productId, int quantity) =>
            $"El producto {productId} no tiene suficiente stock para la cantidad solicitada ({quantity}).";


        ///// Message Order
        public static string OrderItemInvalid(int orderId, string reason) =>
            $"El item de la orden {orderId} es inválido: {reason}.";

        public static string OrderDontExits(int orderId) =>
            $"La orde con ID {orderId} no existe.";

        public static string UpdateStatusDontAlllow(string status, string newStatus) =>
            $"No se Puede cambiar el estado de {status} a {newStatus}.";


        ///// <--Contance Messages-->>
        ///// Message Product
        public const string ProductIdMustBePositive = "El Id del producto debe ser positivo.";
        public const string QuantityMustBeGreaterThanZero = "La cantidad debe ser mayor que cero.";
        public const string UnitPriceMustBeNonNegative = "El precio unitario debe ser mayor o igual a cero.";
        public const string ProductWhitOutOfStock = "No existe Stock suficiente.";

        ///// Message Order
        public const string CustomerIdNotFound = "El campo CustomerId es requerido.";
        public const string ItemNotFound = "Item no encontrado.";

        public const string IdNotMatch = "Los Identificadores no coinciden.";

    }
}
