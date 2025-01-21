using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
public class Tabel
{
    public int bookId { get; set; } 
    public Book book { get; set; } 

    public int personId { get; set; } 
    public Person person { get; set; } 

    public DateTime returnDate { get; set; } 
}
}
