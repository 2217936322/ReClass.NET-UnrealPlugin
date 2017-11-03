using System;
using System.Diagnostics.Contracts;
using System.Text;
using ReClassNET.Memory;
using UnrealPlugin.Config;

namespace UnrealPlugin.NameResolver
{
	internal class UnrealEngine3NameResolver : BaseTArrayNameResolver
	{
		private const int NameWideMask = 1;
		private const int NameIndexShift = 1;

		public UnrealEngine3NameResolver(RemoteProcess process, UnrealEngine3Config config)
			: base(process, config)
		{
			Contract.Requires(process != null);
			Contract.Requires(config != null);
		}

		protected override string ReadNameFromNameEntry(IntPtr nameEntryPtr, int index)
		{
			var nameEntryIndex = process.ReadRemoteInt32(nameEntryPtr + ((UnrealEngine3Config)config).FNameEntryIndexOffset);
			if (nameEntryIndex >> NameIndexShift == index)
			{
				var isWide = (nameEntryIndex & NameWideMask) != 0;

				var name = process.ReadRemoteString(isWide ? Encoding.Unicode : Encoding.ASCII, nameEntryPtr + config.FNameEntryNameDataOffset, 1024);

				return name;
			}

			return null;
		}
	}
}
