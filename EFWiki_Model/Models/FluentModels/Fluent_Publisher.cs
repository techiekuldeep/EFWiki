using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EFWiki_Model.Models
{
    public class Fluent_Publisher
    {
        public int Publisher_Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public List<Fluent_Book> Books { get; set; }
    }
}
