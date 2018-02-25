using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace LibInfoBook
{

    [XmlRoot(ElementName = "request", Namespace = "http://www.openarchives.org/OAI/2.0/")]
    public class Request
    {
        [XmlAttribute(AttributeName = "identifier")]
        public string Identifier { get; set; }
        [XmlAttribute(AttributeName = "metadataPrefix")]
        public string MetadataPrefix { get; set; }
        [XmlAttribute(AttributeName = "verb")]
        public string Verb { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "header", Namespace = "http://www.openarchives.org/OAI/2.0/")]
    public class Header
    {
        [XmlElement(ElementName = "identifier", Namespace = "http://www.openarchives.org/OAI/2.0/")]
        public string Identifier { get; set; }
        [XmlElement(ElementName = "datestamp", Namespace = "http://www.openarchives.org/OAI/2.0/")]
        public string Datestamp { get; set; }
    }

    [XmlRoot(ElementName = "dc", Namespace = "http://www.openarchives.org/OAI/2.0/oai_dc/")]
    public class Dc
    {
        [XmlElement(ElementName = "title", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Title { get; set; }
        [XmlElement(ElementName = "creator", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Creator { get; set; }
        [XmlElement(ElementName = "subject", Namespace = "http://purl.org/dc/elements/1.1/")]
        public List<string> Subject { get; set; }
        [XmlElement(ElementName = "publisher", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Publisher { get; set; }
        [XmlElement(ElementName = "contributors", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Contributors { get; set; }
        [XmlElement(ElementName = "date", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Date { get; set; }
        [XmlElement(ElementName = "identifier", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Identifier { get; set; }
        [XmlElement(ElementName = "rights", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Rights { get; set; }
        [XmlElement(ElementName = "language", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Language { get; set; }
        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "dc", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string _dc { get; set; }
        [XmlAttribute(AttributeName = "oai_dc", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Oai_dc { get; set; }
    }

    [XmlRoot(ElementName = "metadata", Namespace = "http://www.openarchives.org/OAI/2.0/")]
    public class Metadata
    {
        [XmlElement(ElementName = "dc", Namespace = "http://www.openarchives.org/OAI/2.0/oai_dc/")]
        public Dc Dc { get; set; }
    }

    [XmlRoot(ElementName = "record", Namespace = "http://www.openarchives.org/OAI/2.0/")]
    public class Record
    {
        [XmlElement(ElementName = "header", Namespace = "http://www.openarchives.org/OAI/2.0/")]
        public Header Header { get; set; }
        [XmlElement(ElementName = "metadata", Namespace = "http://www.openarchives.org/OAI/2.0/")]
        public Metadata Metadata { get; set; }
    }

    [XmlRoot(ElementName = "GetRecord", Namespace = "http://www.openarchives.org/OAI/2.0/")]
    public class GetRecord
    {
        [XmlElement(ElementName = "record", Namespace = "http://www.openarchives.org/OAI/2.0/")]
        public Record Record { get; set; }
    }

    [XmlRoot(ElementName = "OAI-PMH", Namespace = "http://www.openarchives.org/OAI/2.0/")]
    public class OAIPMH
    {
        [XmlElement(ElementName = "responseDate", Namespace = "http://www.openarchives.org/OAI/2.0/")]
        public string ResponseDate { get; set; }
        [XmlElement(ElementName = "request", Namespace = "http://www.openarchives.org/OAI/2.0/")]
        public Request Request { get; set; }
        [XmlElement(ElementName = "GetRecord", Namespace = "http://www.openarchives.org/OAI/2.0/")]
        public GetRecord GetRecord { get; set; }
        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
}

