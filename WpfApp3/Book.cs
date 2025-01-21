using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp3
{
    public class Book
    {
        public int bookId { get; set; } 
        public string title { get; set; } 
    }

}