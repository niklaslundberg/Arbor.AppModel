using Arbor.KVConfiguration.Urns;
using JetBrains.Annotations;

namespace Arbor.AppModel.Configuration;

[Optional]
[Urn(RegistrationConstants.ExcludedType)]
[UsedImplicitly]
public class ExcludedAutoRegistrationType(string fullName)
{
    public string FullName { get; } = fullName;
}