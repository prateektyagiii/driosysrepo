using OutReach.CoreDataAccess.Attributes;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

namespace OutReach.CoreDataAccess.PostgreDBConnection
{
    public class SqlParameterHelper
    {
        public static List<DbParameter> GetSqlParameters<T>(T Entity)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            var _result = new List<DbParameter>();
            List<T> data = new List<T>();
            foreach (PropertyInfo pro in temp.GetProperties())
            {
                if (!Attribute.IsDefined(pro, typeof(IgnoreDbParameter)))
                {
                    if (pro.GetValue(Entity, null) != null)
                    {
                        if (pro.GetValue(Entity, null).GetType().Name == typeof(long).Name || pro.GetValue(Entity, null).GetType().Name == typeof(int).Name)
                        {
                            NpgsqlParameter _param = new NpgsqlParameter { ParameterName = string.Format("p_{0}", pro.Name.ToLower()), NpgsqlValue = pro.GetValue(Entity, null), NpgsqlDbType = GetNpgsqlDbType(pro.GetValue(Entity, null).GetType()) };
                            _result.Add(_param);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(pro.GetValue(Entity, null).ToString()))
                            {
                                NpgsqlParameter _param = new NpgsqlParameter { ParameterName = string.Format("p_{0}", pro.Name.ToLower()), NpgsqlValue = pro.GetValue(Entity, null), NpgsqlDbType = GetNpgsqlDbType(pro.GetValue(Entity, null).GetType()) };
                                _result.Add(_param);
                            }
                        }
                        
                    }
                   
                }
            }
            return _result;
        }

        public static DbParameter CreateSqlParameter<T>(string name, T value)
        {
            DbParameter _param = new NpgsqlParameter { ParameterName = name, NpgsqlValue = value, NpgsqlDbType = GetNpgsqlDbType(typeof(T)) };
            return _param;
        }
        private static NpgsqlDbType GetNpgsqlDbType(Type t)
        {
            NpgsqlDbType _return = new NpgsqlDbType();
            if (t.Name == typeof(string).Name)
                return _return = NpgsqlDbType.Text;
            if (t.Name == typeof(bool).Name || t.Name == typeof(Boolean).Name)
                return _return = NpgsqlDbType.Boolean;
            if (t.Name == typeof(long).Name)
                return _return = NpgsqlDbType.Integer;
            if (t.Name == typeof(DateTime).Name)
                return _return = NpgsqlDbType.Timestamp;
            if (t.Name == typeof(int).Name)
                return _return = NpgsqlDbType.Integer;
            if (t.Name == typeof(decimal).Name)
                return _return = NpgsqlDbType.Numeric;
            if (t.Name == typeof(double).Name)
                return _return = NpgsqlDbType.Double;
            if (t.Name == typeof(List<int>).Name)
                return _return = NpgsqlDbType.Array | NpgsqlDbType.Integer;
            if (t.Name == typeof(List<string>).Name)
                return _return = NpgsqlDbType.Array | NpgsqlDbType.Text;
            return _return;
        }
    }
}