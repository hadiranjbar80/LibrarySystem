using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class BookAccessor : IBookAccessor
    {
        private readonly DataContext _context;

        public BookAccessor(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllAvailableBooks()
        {
            return await _context.Books.Where(x=> !x.Lendings.Any()).AsNoTracking().ToListAsync();
        }

    }
}
