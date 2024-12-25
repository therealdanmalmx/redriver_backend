using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace redriver_test.Dtos.Book
{
    public class UpdateBookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string PublishDate { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int Pages { get; set; } = 0;
    }
}