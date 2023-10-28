namespace GroceryAPI.Application.Exceptions
{
    public class NotFoundProductException : Exception
    {
        public NotFoundProductException() : base("Product not found!")
        {
        }

        public NotFoundProductException(string? message) : base(message)
        {
        }

        public NotFoundProductException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
