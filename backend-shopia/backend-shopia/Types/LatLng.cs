using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace backend_shop.Types
{
    public class LatLng(double lat, double lng)
    {
        public double Lat { get; set; } = lat;

        public double Lng { get; set; } = lng;

        public Point ToGeometryPoint()
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            return geometryFactory.CreatePoint(new Coordinate(Lng, Lat));
        }
    }
}
