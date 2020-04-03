using System;

namespace EdiTools
{
    /// <summary>
    /// Segment is a building block data structure.
    /// </summary>
    public abstract class Segment
    {
        private EdiTools.Index _index;
        internal EdiTools.Index Index { get { return _index; } set { _index = value; } }
    }
    public sealed class ISA : Segment
    {
        #region private
        private string _isa01;
        private string _isa02;
        private string _isa03;
        private string _isa04;
        private string _isa05;
        private string _isa06;
        private string _isa07;
        private string _isa08;
        private string _isa09;
        private string _isa10;
        private string _isa11;
        private string _isa12;
        private string _isa13;
        private string _isa14;
        private string _isa15;
        private string _isa16;
        #endregion

        public string ISA01 { get { return _isa01; } set { _isa01 = value; } }
        public string ISA02 { get { return _isa02; } set { _isa02 = value; } }
        public string ISA03 { get { return _isa03; } set { _isa03 = value; } }
        public string ISA04 { get { return _isa04; } set { _isa04 = value; } }
        public string ISA05 { get { return _isa05; } set { _isa05 = value; } }
        public string ISA06 { get { return _isa06; } set { _isa06 = value; } }
        public string ISA07 { get { return _isa07; } set { _isa07 = value; } }
        public string ISA08 { get { return _isa08; } set { _isa08 = value; } }
        public string ISA09 { get { return _isa09; } set { _isa09 = value; } }
        public string ISA10 { get { return _isa10; } set { _isa10 = value; } }
        public string ISA11 { get { return _isa11; } set { _isa11 = value; } }
        public string ISA12 { get { return _isa12; } set { _isa12 = value; } }
        public string ISA13 { get { return _isa13; } set { _isa13 = value; } }
        public string ISA14 { get { return _isa14; } set { _isa14 = value; } }
        public string ISA15 { get { return _isa15; } set { _isa15 = value; } }
        public string ISA16 { get { return _isa16; } set { _isa16 = value; } }
    }
    public sealed class IEA : Segment
    {
        #region private
        private string _iea01;
        private string _iea02;
        #endregion
        public string IEA01 { get { return _iea01; } set { _iea01 = value; } }
        public string IEA02 { get { return _iea02; } set { _iea02 = value; } }
    }
    public sealed class Interchange
    {
        private EdiTools.ISA _isa = new EdiTools.ISA();
        private EdiTools.IEA _iea = new EdiTools.IEA();
        private System.DateTime _interchangedate = System.DateTime.MinValue;
        internal Interchange(EdiTools.EdiFile parent, EdiTools.Index isa, EdiTools.Index iea)
        {
            string[] elements = isa.Split();
            _isa.ISA01 = elements[1];
            _isa.ISA02 = elements[2];
            _isa.ISA03 = elements[3];
            _isa.ISA04 = elements[4];
            _isa.ISA05 = elements[5];
            _isa.ISA06 = elements[6];
            _isa.ISA07 = elements[7];
            _isa.ISA08 = elements[8];
            _isa.ISA09 = elements[9];
            _isa.ISA10 = elements[10];
            _isa.ISA11 = elements[11];
            _isa.ISA12 = elements[12];
            _isa.ISA13 = elements[13];
            _isa.ISA14 = elements[14];
            _isa.ISA15 = elements[15];
            _isa.ISA16 = elements[16];
            _isa.Index = isa;

            elements = iea.Split();
            _iea.IEA01 = elements[1];
            _iea.IEA02 = elements[2];
            _iea.Index = iea;
        }
        public EdiTools.ISA ISA { get { return _isa; } }
        public EdiTools.IEA IEA { get { return _iea; } }
        public string SenderId { get { return _isa.ISA06.Trim(); } }
        public string ReceiverId { get { return _isa.ISA08.Trim(); } }
        public System.DateTime Date
        {
            get
            {
                if (_interchangedate == System.DateTime.MinValue)
                {
                    _interchangedate = System.DateTime.ParseExact(_isa.ISA09 + _isa.ISA10, "yyMMddHHmm", System.Globalization.CultureInfo.InvariantCulture);
                }
                return _interchangedate;
            }
        }
        public string ControlNumber { get { return _isa.ISA13; } }
    }
    public sealed class GS : Segment
    {
        #region private
        private string _gs01;
        private string _gs02;
        private string _gs03;
        private string _gs04;
        private string _gs05;
        private string _gs06;
        private string _gs07;
        private string _gs08;
        #endregion
        public string GS01 { get { return _gs01; } set { _gs01 = value; } }
        public string GS02 { get { return _gs02; } set { _gs02 = value; } }
        public string GS03 { get { return _gs03; } set { _gs03 = value; } }
        public string GS04 { get { return _gs04; } set { _gs04 = value; } }
        public string GS05 { get { return _gs05; } set { _gs05 = value; } }
        public string GS06 { get { return _gs06; } set { _gs06 = value; } }
        public string GS07 { get { return _gs07; } set { _gs07 = value; } }
        public string GS08 { get { return _gs08; } set { _gs08 = value; } }
    }
    public sealed class GE : Segment
    {
        #region private
        private string _ge01;
        private string _ge02;
        #endregion
        public string GE01 { get { return _ge01; } internal set { _ge01 = value; } }
        public string GE02 { get { return _ge02; } internal set { _ge02 = value; } }
    }
    public sealed class ST : Segment
    {
        private string _st01;
        private string _st02;
        public string ST01 { get { return _st01; } internal set { _st01 = value; } }
        public string ST02 { get { return _st02; } internal set { _st02 = value; } }
    }
    public sealed class SE : Segment
    {
        private string _se01;
        private string _se02;
        private int _transactionSegmentCount;
        public string SE01 
        { 
            get 
            { 
                return _se01; 
            } 
            internal set 
            { 
                _se01 = value;
                int i = 0;
                System.Int32.TryParse(value, out i);
                _transactionSegmentCount = i;
            } 
        }
        public string SE02 { get { return _se02; } internal set { _se02 = value; } }
        public int TransactionSegmentCount { get { return _transactionSegmentCount; } }
    }
    public sealed class Delimiter
    {
        #region private
        private char _element;
        private char _component;
        private char _segment;
        private string _line = string.Empty;
        #endregion
        public char Element { get { return _element; } set { _element = value; } }
        public char Component { get { return _component; } set { _component = value; } }
        public char Segment { get { return _segment; } set { _segment = value; } }
        public string Line { get { return _line; } set { _line = value; } }
    }
    public sealed class Index
    {
        internal Index(EdiTools.EdiFile ediFile)
        {
            _ediFile = ediFile;
        }
        #region properties
        public string Name { get { return _name; } set { _name = value; } }
        public int Start { get { return _start; } set { _start = value; } }
        public int Length { get { return _length; } set { _length = value; } }
        #endregion

        #region methods
        public string Text
        {
            get
            {
                return _ediFile.GetRawText().Substring(_start, _length);
            }
        }
        public string[] Split()
        {
            return this.Text.Split(new char[] { _ediFile.Delimiter.Element }, StringSplitOptions.None);
        }
        #endregion

        #region private
        private string _name;
        private int _start;
        private int _length;
        private EdiTools.EdiFile _ediFile;
        #endregion
    }
    public sealed class EdiFile
    {
        #region private
        private string _filename;
        private string _directoryname;
        private EdiTools.Delimiter _delimiter = new EdiTools.Delimiter();
        private bool _isunwrapped = false;
        private string _rawtext;
        private EdiTools.Index[] _fileindexes;

        private EdiTools.Interchange _interchange;
        private EdiTools.FunctionalGroup[] _functionalgroups;
        #endregion
        public EdiFile(string filePath)
        {
            try
            {
                this._rawtext = System.IO.File.ReadAllText(filePath);
                if (this._rawtext.Substring(0, 3) != "ISA")
                {
                    throw new System.IO.InvalidDataException("No ISA segment found. Not an EDI file.");
                }
                _delimiter.Element = _rawtext[103];
                _delimiter.Component = _rawtext[104];
                _delimiter.Segment = _rawtext[105];

                /* Determine if wrapped (no line breaks) or unwrapped (line breaks) */
                if (_rawtext[106] == (char)13 || _rawtext[106] == (char)10) /* carriage-return or new-line */
                {
                    _isunwrapped = true;

                    if (_rawtext[107] == (char)10) /* if the next char is new-line, assume cr/lf */
                    {
                        _delimiter.Line = _rawtext.Substring(106, 2);
                    }
                    else
                    {
                        _delimiter.Line = _rawtext[106].ToString();
                    }
                }

                _filename = System.IO.Path.GetFileName(filePath);
                _directoryname = System.IO.Path.GetDirectoryName(filePath);

                // start position and length for every segment/line in the file. The foundation for parsing the file
                _fileindexes = this.GetIndexes();

                // to parse interchange
                EdiTools.Index isa = new EdiTools.Index(this);
                EdiTools.Index iea = new EdiTools.Index(this);

                // to parse functional groups
                System.Collections.Generic.List<EdiTools.FunctionalGroup> functionalGroups = new System.Collections.Generic.List<FunctionalGroup>();

                EdiTools.Index gs = new EdiTools.Index(this);
                EdiTools.Index ge = new EdiTools.Index(this);

                foreach (EdiTools.Index idx in _fileindexes)
                {
                    // interchange
                    if (idx.Name == "ISA") { isa = idx; }
                    if (idx.Name == "IEA")
                    {
                        iea = idx;
                        _interchange = new EdiTools.Interchange(this, isa, iea);
                    }

                    // functional group
                    if (idx.Name == "GS")
                    {
                        gs = idx;
                    }

                    // GE is the end of a functional group, so add to the functional group collection
                    if (idx.Name == "GE")
                    {
                        ge = idx;
                        EdiTools.FunctionalGroup group = new EdiTools.FunctionalGroup(this, gs, ge);
                        functionalGroups.Add(group);
                    }
                }

                _functionalgroups = functionalGroups.ToArray();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        #region methods
        internal Index[] GetIndexes()
        {
            System.Collections.Generic.List<EdiTools.Index> indexes = new System.Collections.Generic.List<EdiTools.Index>();
            EdiTools.Index idx = new EdiTools.Index(this);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            bool doRead = true;
            for (int i = 0; i < _rawtext.Length; i++)
            {
                char c = _rawtext[i];
                if (doRead && c != _delimiter.Element && c != (char)10 && c != (char)13)  // 10:new line, 13:carriage return
                {
                    sb.Append(c);
                }

                if (c == _delimiter.Element) { doRead = false; }

                if (c == _delimiter.Segment)
                {
                    doRead = true;

                    idx.Length = i - idx.Start;
                    idx.Name = sb.ToString();
                    indexes.Add(idx);

                    // reset for next loop
                    sb = new System.Text.StringBuilder();
                    idx = new EdiTools.Index(this);
                    idx.Start = i + 1 + _delimiter.Line.Length; /* The + 1 is to move the index past the segment delimiter position */
                }
            }

            return indexes.ToArray();
        }
        public string GetRawText() { return _rawtext; }
        public string Unwrap()
        {
            if (_isunwrapped)
            {
                return this.GetRawText();
            }
            else
            {
                return this.GetRawText().Replace(_delimiter.Segment.ToString(), _delimiter.Segment.ToString() + System.Environment.NewLine);
            }
        }
        #endregion

        #region properties
        public string Name { get { return _filename; } set { _filename = value; } }
        public string DirectoryName { get { return _directoryname; } set { _directoryname = value; } }
        public string FullName { get { return System.IO.Path.Combine(_directoryname, _filename); } }
        public EdiTools.Delimiter Delimiter { get { return _delimiter; } }
        public bool IsUnwrapped { get { return _isunwrapped; } }
        public EdiTools.Interchange Interchange { get { return _interchange; } }
        public EdiTools.FunctionalGroup[] FunctionalGroups { get { return _functionalgroups; } }
        #endregion
    }
    public sealed class FunctionalGroup
    {
        internal FunctionalGroup(EdiTools.EdiFile parent, EdiTools.Index gs, EdiTools.Index ge)
        {
            _parent = parent;

            // gs
            string[] elements = gs.Split();
            _gs.GS01 = elements[1];
            _gs.GS02 = elements[2];
            _gs.GS03 = elements[3];
            _gs.GS04 = elements[4];
            _gs.GS05 = elements[5];
            _gs.GS06 = elements[6];
            _gs.GS07 = elements[7];
            _gs.GS08 = elements[8];
            _gs.Index = gs;

            // ge
            elements = ge.Split();
            _ge.GE01 = elements[1];
            _ge.GE02 = elements[2];
            _ge.Index = ge;

            // st/se indexes are for getting the start and end of each transaction set in the group
            System.Collections.Generic.List<EdiTools.Index> _stIndexes = new System.Collections.Generic.List<Index>();
            System.Collections.Generic.List<EdiTools.Index> _seIndexes = new System.Collections.Generic.List<Index>();

            System.Collections.Generic.List<EdiTools.Index> segments = new System.Collections.Generic.List<Index>();
            foreach (EdiTools.Index idx in _parent.GetIndexes())
            {
                if (idx.Start <= _gs.Index.Start || idx.Start >= _ge.Index.Start)
                {
                    continue;
                }

                segments.Add(idx);

                if (idx.Name == "ST")
                {
                    _stIndexes.Add(idx);
                }

                if (idx.Name == "SE")
                {
                    _seIndexes.Add(idx);
                    _transactionSetIndexes.Add(segments.ToArray());
                    segments.Clear();
                }
            }
        }
        #region private
        private EdiTools.EdiFile _parent;
        private EdiTools.GS _gs = new EdiTools.GS();
        private EdiTools.GE _ge = new EdiTools.GE();
        private System.DateTime _groupdate = System.DateTime.MinValue;
        private System.Collections.Generic.List<EdiTools.Index[]> _transactionSetIndexes = new System.Collections.Generic.List<Index[]>();
        #endregion
        public EdiTools.GS GS { get { return _gs; } }
        public EdiTools.GE GE { get { return _ge; } }
        public System.DateTime GroupDate
        {
            get
            {
                if (_groupdate == System.DateTime.MinValue)
                {
                    _groupdate = System.DateTime.ParseExact(_gs.GS04 + _gs.GS05.PadRight(8, '0'), "yyyyMMddHHmmssff", System.Globalization.CultureInfo.InvariantCulture);
                }
                return _groupdate;
            }
        }
        public EdiTools.EdiFile EdiFile { get { return _parent; } }
        public EdiTools.TransactionSet[] GetTransactionSets()
        {
            System.Collections.Generic.List<EdiTools.TransactionSet> transactionSets = new System.Collections.Generic.List<EdiTools.TransactionSet>();
            foreach (EdiTools.Index[] idx in _transactionSetIndexes)
            {
                transactionSets.Add(new EdiTools.TransactionSet(this, idx));
            }
            return transactionSets.ToArray();
        }
    }
    public sealed class TransactionSet
    {
        public TransactionSet(EdiTools.FunctionalGroup parent, EdiTools.Index[] indexes)
        {
            _parent = parent;
            _indexes = indexes;

            EdiTools.Index st = indexes[0];
            EdiTools.Index se = indexes[indexes.Length - 1];

            string[] elements = st.Split();
            _st = new EdiTools.ST();
            _st.ST01 = elements[1];
            _st.ST02 = elements[2];
            _st.Index = st;

            elements = se.Split();
            _se = new EdiTools.SE();
            _se.SE01 = elements[1];
            _se.SE02 = elements[2];
            _se.Index = se;
        }

        #region private
        private EdiTools.FunctionalGroup _parent;
        private EdiTools.ST _st;
        private EdiTools.SE _se;
        private EdiTools.Index[] _indexes;
        #endregion

        #region properties
        public EdiTools.ST ST { get { return _st; } }
        public EdiTools.SE SE { get { return _se; } }
        public string ID { get { return _st.ST01; } }
        public string ControlNumber { get { return _st.ST02; } }
        public EdiTools.Index[] Indexes { get { return _indexes; } }
        public EdiTools.FunctionalGroup FunctionalGroup { get { return _parent; } }
        #endregion

        #region methods
        public string GetRawText()
        {
            return _parent.EdiFile.GetRawText().Substring(_st.Index.Start, _se.Index.Start + _se.Index.Length - _st.Index.Start + 1 + _parent.EdiFile.Delimiter.Line.Length);
        }
        public string Unwrap()
        {
            if (_parent.EdiFile.IsUnwrapped)
            {
                return this.GetRawText();
            }
            else
            {
                return this.GetRawText().Replace(_parent.EdiFile.Delimiter.Segment.ToString(), _parent.EdiFile.Delimiter.Segment.ToString() + System.Environment.NewLine);
            }
        }
        #endregion
    }
    public abstract class EdiImplementationBase
    {
        public EdiImplementationBase(EdiTools.TransactionSet transactionSet)
        {
            _transactionSet = transactionSet;
        }

        #region properties
        public string Text
        {
            get
            {
                if (_transactionSet.FunctionalGroup.EdiFile.IsUnwrapped)
                {
                    return _transactionSet.GetRawText();
                }
                else
                {
                    return _transactionSet.Unwrap();
                }
            }
        }
        #endregion

        #region private
        protected EdiTools.TransactionSet _transactionSet;
        #endregion
    }
    public sealed class Edi835 :EdiImplementationBase
    {
        public Edi835(EdiTools.TransactionSet transactionSet) : base(transactionSet)
        {
            if (transactionSet.ID != "835") { throw new ArgumentException("Expected an EDI transaction set of type 835."); }
            _transactionSet = transactionSet;

            // get convenience 835 properties
            foreach (EdiTools.Index idx in transactionSet.Indexes)
            {
                string[] elements;
                if (idx.Name == "BPR")
                {
                    elements = idx.Split();

                    System.Decimal.TryParse(elements[2], out _totalActualProviderPaymentAmount);
                    _senderBankAccountNumber = elements[9];
                }
                if (idx.Name == "TRN")
                {
                    elements = idx.Split();
                    _checkorEFTTraceNumber = elements[2];
                }
                if (idx.Name == "N1")
                {
                    elements = idx.Split();
                    if (elements[1] == "PR")
                    {
                        _payer = elements[2];
                    }
                    if (elements[1] == "PE")
                    {
                        _payee = elements[2];
                        break;
                    }
                }
            }
        }

        #region private
        private string _payer;
        private string _payee;
        private string _senderBankAccountNumber;
        private string _checkorEFTTraceNumber;
        private decimal _totalActualProviderPaymentAmount;
        #endregion

        #region properties
        public string Payer { get { return _payer; } }
        public string Payee { get { return _payee; } }
        public string SenderBankAccountNumber { get { return _senderBankAccountNumber; } }
        public string CheckorEFTTraceNumber { get { return _checkorEFTTraceNumber; } }
        public decimal TotalActualProviderPaymentAmount { get { return _totalActualProviderPaymentAmount; } }
        public TransactionSet TransactionSet { get { return _transactionSet; } }
        #endregion
    }
}