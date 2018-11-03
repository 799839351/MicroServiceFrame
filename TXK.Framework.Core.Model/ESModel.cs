using Nest;
using System;
using System.Xml.Linq;

namespace TXK.Framework.Core.Model
{
    [ElasticsearchType(Name = "esmodel", IdProperty = "id")]
    public class ESModel
    {
        [Number(Name = "id")]
        public int Id { set; get; }

        [Text(Name = "name")]
        public string Name { set; get; }
        [Date(Name = "birthday",Format = "yyyy-MM-dd HH:mm:ss")]
        public DateTime? Birthday { set; get; }
    }
}
