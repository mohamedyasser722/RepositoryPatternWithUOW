using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Dtos
{
    public class BookDto
    {
        public string Title { get; set; }
        public int AuthorId { get; set; }
    }
}
