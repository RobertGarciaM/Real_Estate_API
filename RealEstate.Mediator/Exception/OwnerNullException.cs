namespace RealEstate.Mediator.CustomException
{
    public class EntityNullException : Exception
    {
        public EntityNullException() : base("Entity object is null.")
        {
        }
    }
}
