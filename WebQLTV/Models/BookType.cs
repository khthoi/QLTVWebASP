using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebQLTV.Models
{
    public class BookType
    {
        [Key]
        public int TypeID { get; set; }
        public string TypeName { get; set; }
    }
}
