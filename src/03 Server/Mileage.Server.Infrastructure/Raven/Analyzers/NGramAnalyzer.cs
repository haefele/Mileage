using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;
using Raven.Database.Indexing;

namespace Mileage.Server.Infrastructure.Raven.Analyzers
{
    [NotForQuerying]
    public class NGramAnalyzer : Analyzer
    {
        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            var standardTokenizer = new StandardTokenizer((Version)5, reader)
            {
                MaxTokenLength = byte.MaxValue
            };
            return new NGramTokenFilter(new StopFilter(false, new LowerCaseFilter(new StandardFilter(standardTokenizer)), StandardAnalyzer.STOP_WORDS_SET), 2, 15);
        }
    }
}