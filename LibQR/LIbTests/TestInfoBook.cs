using LibInfoBook;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIbTests
{
    [TestClass]
    public class TestInfoBook
    {
        [TestMethod]
        public void TestInfoXML()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
<OAI-PMH xsi:schemaLocation='http://www.openarchives.org/OAI/2.0/ http://www.openarchives.org/OAI/2.0/OAI-PMH.xsd' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns='http://www.openarchives.org/OAI/2.0/'>
<responseDate>2018-02-25T22:33:56Z</responseDate>
<request identifier='oai:ime.ro:88046' metadataPrefix='oai_dc' verb='GetRecord'>http://opac.aman.ro//oai</request>
<GetRecord>
<record>
<header>
<identifier>oai:ime.ro:88046</identifier>
<datestamp>2018-02-19T11:03:31Z</datestamp>
</header>
<metadata>
<oai_dc:dc xsi:schemaLocation='http://www.openarchives.org/OAI/2.0/oai_dc/ http://www.openarchives.org/OAI/2.0/oai_dc.xsd' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:dc='http://purl.org/dc/elements/1.1/' xmlns:oai_dc='http://www.openarchives.org/OAI/2.0/oai_dc/'>
<dc:title>Pe muchie de cuţit</dc:title>
<dc:creator>Forbes, Colin</dc:creator>
<dc:subject>roman poliţist</dc:subject>
<dc:subject>821eng/F79</dc:subject>
<dc:publisher>Editura Rao International Publishing Company</dc:publisher>
<dc:contributors>Traducere : Flavia Iustina Bosnari</dc:contributors>
<dc:date>2007</dc:date>
<dc:identifier>an identifier</dc:identifier>
<dc:rights>4</dc:rights>
<dc:language>rum</dc:language>
</oai_dc:dc>
</metadata>
</record>
</GetRecord>
</OAI-PMH>";
            var b = new InfoBook("88046");
            b.GetInfo(xml.Replace("'", "\""));
            Assert.AreEqual("Forbes, Colin", b.Creator);
            Assert.AreEqual("an identifier", b.Identifier);
        }

        [TestMethod]
        public async Task VerifyFromInternet()
        {
            var b = new InfoBook("88046");
            await b.GetInfoFromId();
            Assert.AreEqual("Forbes, Colin", b.Creator);

        }
    }
}
