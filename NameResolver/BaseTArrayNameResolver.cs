using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using ReClassNET.Memory;
using ReClassNET.Util;
using UnrealPlugin.Config;

namespace UnrealPlugin.NameResolver
{
	internal abstract class BaseTArrayNameResolver : BaseNameResolver
	{
		[StructLayout(LayoutKind.Sequential)]
		private struct TArray
		{
			public IntPtr Data;
			public int NumElements;
			public int MaxElements;
		}

		protected BaseTArrayNameResolver(RemoteProcess process, BaseConfig config)
			: base(process, config)
		{
			Contract.Requires(process != null);
			Contract.Requires(config != null);
		}

		protected override IntPtr ReadNameEntryPtr(int index)
		{
			Contract.Requires(index > 0);

			var namesArray = process.ReadRemoteObject<TArray>(config.GlobalArrayPtr);
			if (index < namesArray.NumElements)
			{
				if (namesArray.Data.MayBeValid())
				{
					var nameEntryPtr = process.ReadRemoteIntPtr(namesArray.Data + index * IntPtr.Size);

					return nameEntryPtr;
				}
			}

			return IntPtr.Zero;
		}
	}
}
