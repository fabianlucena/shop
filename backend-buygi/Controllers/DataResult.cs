using System.Collections;

namespace backend_buygi.Controllers
{
    public class DataRowsResult
    {
        public IEnumerable Rows { get; set; }

        public DataRowsResult(IEnumerable rows)
        {
            Rows = rows;
        }
    }
}
