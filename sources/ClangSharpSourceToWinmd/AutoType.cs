using Newtonsoft.Json;

namespace ClangSharpSourceToWinmd
{
    public class AutoType
    {
        public string Namespace { get; set; }
        public string Name { get; set; }
        // "native" selects nominal projection; the scraper supplies identity/storage from the compiler.
        public string ValueType { get; set; }
        public string CloseApi { get; set; }
        public string AlsoUsableFor { get; set; }
        public long[] InvalidHandleValues { get; set; }
        public bool NativeTypedef { get; set; }
    }
}
