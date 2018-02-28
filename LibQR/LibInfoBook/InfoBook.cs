using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LibInfoBook
{


    public class InfoBook: IValidatableObject
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.Creator))
                yield return new ValidationResult($"{nameof(Creator)} trebuie sa existe");

            if (string.IsNullOrWhiteSpace(this.Identifier))
                yield return new ValidationResult($"{nameof(Identifier)} trebuie sa existe");

            if (string.IsNullOrWhiteSpace(this.Title))
                yield return new ValidationResult($"{nameof(Title)} trebuie sa existe");
            

        }
    }
}
