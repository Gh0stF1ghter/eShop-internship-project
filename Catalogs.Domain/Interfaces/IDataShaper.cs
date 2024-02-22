﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Domain.Interfaces
{
    public interface IDataShaper<T>
    {
        IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entitities, string fieldsString);

        ExpandoObject ShapeData(T entity, string fieldsString);
    }
}
