using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LibInfoBook
{


    public class InfoBook
    {
        public InfoBook(string id)
        {
            Id = id;
        }

        public string Id { get; }
        
        public string Title { get; set; }
        public string  Creator { get; set; }
        public string Identifier { get; set; }

        public void GetInfo(string xml)
        {
            var serializer = new XmlSerializer(typeof(OAIPMH));
            using (TextReader reader = new StringReader(xml))
            {
                var res =(OAIPMH) serializer.Deserialize(reader);

                if (!res.Request.Identifier.Contains(Id))
                    return;
                this.Title = res.GetRecord.Record.Metadata.Dc.Title;
                this.Creator= res.GetRecord.Record.Metadata.Dc.Creator;
                this.Identifier= res.GetRecord.Record.Metadata.Dc.Identifier;

            }
        }
        
        public async Task GetInfoFromId()
        {
            var url = $"http://opac.aman.ro//oai?verb=GetRecord&identifier=oai:ime.ro:{Id}&metadataPrefix=oai_dc";
            var request = WebRequest.Create(url);
            request.Method = "GET";
            using (var wr = await request.GetResponseAsync())
            {
                using (var receiveStream = wr.GetResponseStream())
                {
                    using (var reader = new StreamReader(receiveStream, Encoding.UTF8))
                    {
                        string content = reader.ReadToEnd();
                        GetInfo(content);
                    }
                        
                }
                    
            }
                

        }



    }
}
