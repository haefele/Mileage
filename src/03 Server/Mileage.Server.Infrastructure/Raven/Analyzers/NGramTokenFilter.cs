using System;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;
using Lucene.Net.Util;

namespace Mileage.Server.Infrastructure.Raven.Analyzers
{
    public class NGramTokenFilter : TokenFilter
    {
        #region Fields
        private readonly int _minGram;
        private readonly int _maxGram;
        private readonly TermAttribute _termAtt;
        private readonly OffsetAttribute _offsetAtt;

        private char[] _curTermBuffer;
        private int _curTermLength;
        private int _curGramSize;
        private int _curPos;
        private int _tokStart;
        #endregion

        #region Constructors
        public NGramTokenFilter(TokenStream input, int minGram, int maxGram)
            : base(input)
        {
            if (minGram < 1)
                throw new ArgumentException("minGram must be greater than zero");
            if (minGram > maxGram)
                throw new ArgumentException("minGram must not be greater than maxGram");

            this._minGram = minGram;
            this._maxGram = maxGram;
            this._termAtt = (TermAttribute)this.AddAttribute<ITermAttribute>();
            this._offsetAtt = (OffsetAttribute)this.AddAttribute<IOffsetAttribute>();
        }
        #endregion

        #region Methods
        public override bool IncrementToken()
        {
            while (true)
            {
                if (this._curTermBuffer == null)
                {
                    if (this.input.IncrementToken())
                    {
                        this._curTermBuffer = (char[])this._termAtt.TermBuffer().Clone();
                        this._curTermLength = this._termAtt.TermLength();
                        this._curGramSize = this._minGram;
                        this._curPos = 0;
                        this._tokStart = this._offsetAtt.StartOffset;
                    }
                    else
                        break;
                }
                while (this._curGramSize <= this._maxGram)
                {
                    if (this._curPos + this._curGramSize <= this._curTermLength)
                    {
                        this.ClearAttributes();
                        this._termAtt.SetTermBuffer(this._curTermBuffer, this._curPos, this._curGramSize);
                        this._offsetAtt.SetOffset(this._tokStart + this._curPos, this._tokStart + this._curPos + this._curGramSize);
                        ++this._curPos;
                        return true;
                    }
                    else
                    {
                        ++this._curGramSize;
                        this._curPos = 0;
                    }
                }
                this._curTermBuffer = (char[])null;
            }
            return false;
        }

        public override void Reset()
        {
            base.Reset();
            this._curTermBuffer = (char[])null;
        }
        #endregion
    }
}