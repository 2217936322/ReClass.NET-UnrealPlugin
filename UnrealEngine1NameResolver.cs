using System;
using System.Diagnostics.Contracts;
using System.Text;
using ReClassNET.Memory;

namespace UnrealPlugin
{
	internal class UnrealEngine1NameResolver : BaseTArrayNameResolver
	{
		public UnrealEngine1NameResolver(RemoteProcess process, UnrealEngine1Config config)
			: base(process, config)
		{
			Contract.Requires(process != null);
			Contract.Requires(config != null);
		}

		protected override string ReadNameFromNameEntry(IntPtr nameEntryPtr, int index)
		{
			var ue1Config = (UnrealEngine1Config)config;

			var name = process.ReadRemoteString(ue1Config.FNameEntryIsWide ? Encoding.Unicode : Encoding.ASCII, nameEntryPtr + ue1Config.FNameEntryNameDataOffset, 64);

			return name;
		}
	}
}
