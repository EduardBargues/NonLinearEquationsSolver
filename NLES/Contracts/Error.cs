namespace NLES.Contracts
{
    public class Error
    {
        public string Description { get; set; }

        public Error(string errorDescription)
        {
            Description = errorDescription;
        }
    }
}