using Arbor.KVConfiguration.Urns;

namespace Arbor.AppModel.IO
{
    [Urn(Urn)]
    [Optional]
    public class ApplicationPaths
    {
        public const string Urn = "urn:arbor:app:web:paths";

        public string? BasePath { get; set; }

        public string? ContentBasePath { get; set; }
    }
}