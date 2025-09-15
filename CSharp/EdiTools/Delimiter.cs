namespace EdiTools
{
    public sealed class Delimiter
    {
        public char Element 
        { 
            get;
            internal set;    
        }
        public char Component 
        { 
            get;
            internal set;
         }
        public char Segment 
        {
            get;
            internal set;
        }
        public string LineTerminator
        {
            get;
            internal set;
        }
    }
}