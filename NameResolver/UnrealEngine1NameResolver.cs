using System;
using System.Diagnostics.Contracts;
using System.Text;
using ReClassNET.Memory;
using UnrealPlugin.Config;

namespace UnrealPlugin.NameResolver
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
			var nameEntryIndex = process.ReadRemoteInt32(nameEntryPtr + config.FNameEntryIndexOffset);
			if (nameEntryIndex == index)
			{
				var name = process.ReadRemoteString(((UnrealEngine1Config)config).FNameEntryIsWide ? Encoding.Unicode : Encoding.ASCII, nameEntryPtr + config.FNameEntryNameDataOffset, 64);

				return name;
			}

			return null;
		}
	}
}
