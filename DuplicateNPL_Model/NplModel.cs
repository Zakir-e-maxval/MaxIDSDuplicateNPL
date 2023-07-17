using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateNPL_Model
{
    public class NplModel
    {
        public string RecordId { get; set; }
        public string Citation { get; set; }
        public string PlainCitation { get; set; }
        public int IsDuplicateCheck { get; set; }
        public int id { get; set; }
    }
    public class NplDuplicateModel
    {
        public string RecordId { get; set; }
        public string Citation { get; set; }
        public string MatchingRecord { get; set; }
        public int Score { get; set; }
        public bool IsDuplicate { get; set; }
        public string plaincitation { get; set; }
    }
    public class RecordData<T>
    {
        public string Source { get; set; }
        public T Items { get; set; }
        public bool OnceOffRequired { get; set; }
    }

    public class NplParsedModel
    {
        public string RecordID { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public string DOI { get; set; }
        public string Date { get; set; }
        public string Volume { get; set; }
        public string Title { get; set; }
        public string Series { get; set; }
        public string School { get; set; }
        public string Publisher { get; set; }
        public string Pages { get; set; }
        public string Organization { get; set; }
        public string Number { get; set; }
        public string Note { get; set; }
        public string Journal { get; set; }
        public string Institution { get; set; }
        public string Howpublished { get; set; }
        public string Editor { get; set; }
        public string Edition { get; set; }
        public string Crossref { get; set; }
        public string Chapter { get; set; }
        public string Booktitle { get; set; }
        public string Author { get; set; }
        public string Annote { get; set; }
        public string Key { get; set; }
        public string Mouth { get; set; }
        public string Source { get; set; }

    }

    public class NplSettingModel
    {
        public string ExcludedWords { get; set; }
        public int? Percentage { get; set; }
    }
}
