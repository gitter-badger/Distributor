using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Distributor
{
    public class Delivery : IEquatable<Delivery>
    {
        public readonly int FileId;
        public readonly Guid EndpointId;

        public Delivery(int fileId, Guid endpointId)
        {
            FileId = fileId;
            EndpointId = endpointId;
        }

        public bool Equals(Delivery other)
        {
            if (other == null)
                return false;

            return FileId == other.FileId && EndpointId == other.EndpointId;
        }

        public override bool Equals(object other)
        {
            return Equals(other as Delivery);
        }

        public static bool operator ==(Delivery delivery1, Delivery delivery2)
        {
            if (delivery1 == null || delivery2 == null)
                return false;

            return delivery1.Equals(delivery2);
        }

        public static bool operator !=(Delivery delivery1, Delivery delivery2)
        {
            if (delivery1 == null || delivery2 == null)
                return false;

            return !delivery1.Equals(delivery2);
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = (hash * 7) + FileId.GetHashCode();
            hash = (hash * 7) + EndpointId.GetHashCode();
            return hash;
        }
    }

    public interface IDeliveryTracker
    {
        void DeliveryComplete(Delivery delivery, DateTime completedDateTime);
        void DeliveryFailed(Delivery delivery, DateTime completedDateTime);
        bool DeliveryIsComplete(Delivery delivery);
    }

    public class DeliveryTracker : IDeliveryTracker
    {
        private readonly List<Delivery> _completedDeliveries = new List<Delivery>();
        private readonly List<Delivery> _deliveryFailures = new List<Delivery>();

        public void DeliveryComplete(Delivery delivery, DateTime completedDateTime)
        {
            _completedDeliveries.Add(delivery);
        }

        public void DeliveryFailed(Delivery delivery, DateTime completedDateTime)
        {
            _deliveryFailures.Add(delivery);
        }

        public bool DeliveryIsComplete(Delivery delivery)
        {
            return _completedDeliveries.Contains(delivery);
        }
    }   
}
