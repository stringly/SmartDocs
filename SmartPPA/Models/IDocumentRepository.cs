using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPPA.Models
{
    public interface IDocumentRepository
    {
        IEnumerable<User> Users { get; }
        IEnumerable<SmartDocument> Documents { get; }
        IEnumerable<SmartTemplate> Templates { get; }
    }
}
