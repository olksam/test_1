﻿using System;
using System.Data;
using Dapper;

namespace AccManagement.API.Common.DapperTypeHandlers {
    public class GuidTypeHandler : SqlMapper.TypeHandler<Guid> {
        public override void SetValue(IDbDataParameter parameter, Guid value) {
            parameter.Value = value.ToString();
        }

        public override Guid Parse(object value) {
            return Guid.Parse((string) value);
        }
    }
}