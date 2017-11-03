using System.Diagnostics.Contracts;
using ReClassNET.Memory;
using UnrealPlugin.Config;

namespace UnrealPlugin.NameResolver
{
	internal class UnrealEngine2NameResolver : UnrealEngine1NameResolver
	{
		public UnrealEngine2NameResolver(RemoteProcess process, UnrealEngine2Config config)
			: base(process, config)
		{
			Contract.Requires(process != null);
			Contract.Requires(config != null);
		}
	}
}
