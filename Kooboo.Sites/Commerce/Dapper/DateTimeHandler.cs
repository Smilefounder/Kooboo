using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Kooboo.Sites.Commerce
{
    public class DateTimeHandler : Dapper.SqlMapper.TypeHandler<DateTime>
    {
        public override void SetValue(IDbDataParameter parameter, DateTime value)
        {
            parameter.Value = value;
        }

        public override DateTime Parse(object value)
        {
            return DateTime.SpecifyKind((DateTime)value, DateTimeKind.Local);
        }
    }
}
