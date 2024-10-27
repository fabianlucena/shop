using System.Collections;

namespace RFService.DataLib
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
