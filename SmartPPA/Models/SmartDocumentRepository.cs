using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPPA.Models
{
    public class SmartDocumentRepository : IDocumentRepository
    {
        private DocumentContext context;

        public SmartDocumentRepository(DocumentContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<User> Users => context.Users;
        public IEnumerable<SmartDocument> Documents => context.Documents;
        public IEnumerable<SmartTemplate> Templates => context.Templates;
    }
}
