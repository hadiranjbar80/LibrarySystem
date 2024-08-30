namespace Application.Interfaces
{
    public interface IBookAccessor
    {
        /// <summary>
        /// Returns those books that nobody has borrowed them
        /// </summary>
        /// <returns>A list of all available books</returns>
        Task<List<Domain.Entities.Book>> GetAllAvailableBooks();
    }
}
