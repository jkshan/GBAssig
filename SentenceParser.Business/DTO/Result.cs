namespace SentenceParser.Business.DTO
{
    public class Result
    {
        public long MaxCount { get; internal set; }
        public string MaxOccuredWords { get; internal set; }
        public long MedianCount { get; internal set; }
        public object MedianOccuredWords { get; internal set; }
        public long MinCount { get; internal set; }
        public object MinOccuredWords { get; internal set; }
    }
}
