using System.Xml.Serialization;

namespace AlphaPlus.BetLists.LastBets;

[Serializable]
public class SportXml
{
    [XmlAttribute(AttributeName = "s")]
    public string Sport { get; set; }

    [XmlAttribute(AttributeName = "l")]
    public string League { get; set; }
}
