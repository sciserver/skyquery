using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Vosi.Availability.V1_0
{
    [XmlRoot(Constants.TagAvailability, Namespace = Constants.VosiAvailabilityNamespaceV1_0)]
    public class Availability
    {
        [XmlElement(Constants.TagAvailable)]
        public bool Available { get; set; }

        [XmlElement(Constants.TagUpSince)]
        public DateTime? UpSince { get; set; }

        [XmlElement(Constants.TagDownAt)]
        public DateTime? DownAt { get; set; }

        [XmlElement(Constants.TagBackAt)]
        public DateTime? BackAt { get; set; }

        [XmlElement(Constants.TagNote)]
        public string Note { get; set; }
    }
}
