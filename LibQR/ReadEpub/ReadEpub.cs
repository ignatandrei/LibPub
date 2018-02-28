using System;
using System.Collections.Generic;
using VersOne.Epub;

namespace ReadEpub
{
    public class ReadEpubFile
    {
        EpubBook epubBook;
        public ReadEpubFile(string fileName)
        {
            FileName = fileName;
            epubBook = EpubReader.ReadBook(fileName);
            Title = epubBook.Title;
            Author = epubBook.Author;
            var cntList = new List<Content>();
            foreach (var chapter in epubBook.Chapters)
            {

                var cnt = new Content();
                cnt.Title = chapter.Title;
                cnt.HtmlContent = chapter.HtmlContent;
                var cntSubChapterList = new List<Content>();
                //TODO: more than 2  - recursive
                foreach (var item in chapter.SubChapters)
                {
                    var cntSubChapter = new Content();
                    cntSubChapter.Title = item.Title;
                    cntSubChapter.HtmlContent = item.HtmlContent;
                    cntSubChapterList.Add(cntSubChapter);
                }
                cntList.Add(cnt);
            }
            content = cntList.ToArray();
        }
        public string Title { get; set; }
        public string Author { get; set; }
        public string FileName { get; }
        public Content[] content;
    }
    public class Content {
        public Content[] Childs{get;set;}
        public string Title { get; set; }
        public string HtmlContent { get; set; }

    }
}
