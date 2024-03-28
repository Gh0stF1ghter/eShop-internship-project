using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Domain.Entities.Exceptions
{
    public class AlreadyExistsException(string message) : Exception(message);
}
