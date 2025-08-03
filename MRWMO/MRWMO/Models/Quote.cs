using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRWMO.Models
{
    public class Quote
    {
        public string Text { get; set; }
        public string Author { get; set; }

        // A helper property to format the quote for display
        public string FormattedQuote => $"“{Text}”\n— {Author}";
    }
}
