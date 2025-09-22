using Dapper;
using System.Data;
using Pgvector;

namespace backend_shopia.Types
{
    public class VectorFloatArrayHandler
        : SqlMapper.TypeHandler<float[]>
    {
        public override void SetValue(IDbDataParameter parameter, float[]? value)
            => parameter.Value = new Vector(value);

        public override float[]? Parse(object value)
            => value switch
            {
                Vector v => v.ToArray(),
                float[] fa => fa,
                _ => throw new DataException($"Tipo inesperado: {value?.GetType().FullName}")
            };
    }
}
