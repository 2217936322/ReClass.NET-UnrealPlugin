using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Text;
using ReClassNET.Memory;
using ReClassNET.Util;

namespace UnrealPlugin
{
	internal class UnrealEngine3NameResolver : INameResolver
	{
		[StructLayout(LayoutKind.Sequential)]
		private struct TArray
		{
			public IntPtr Data;
			public int NumElements;
			public int MaxElements;
		}

		private const int NAME_WIDE_MASK = 1;
		private const int NAME_INDEX_SHIFT = 1;

		public int UObjectNameOffset { get; set; }

		public int FNameEntryIndexOffset { get; set; } = 0;
		public int FNameEntryNameDataOffset { get; set; } = 4 + 4 + IntPtr.Size;

		public string ReadNameOfObject(IntPtr address, RemoteProcess process)
		{
			var nameIndex = ReadNameIndexOfObject(address, process);
			if (nameIndex < 1)
			{
				return null;
			}

			return ReadNameFromNameArray(nameIndex, process);
		}

		private int ReadNameIndexOfObject(IntPtr address, RemoteProcess process)
		{
			Contract.Requires(process != null);

			return process.ReadRemoteInt32(address + UObjectNameOffset);
		}

		private string ReadNameFromNameArray(int index, RemoteProcess process)
		{
			Contract.Requires(index > 0);
			Contract.Requires(process != null);

			IntPtr gNames = IntPtr.Zero;

			var namesArray = process.ReadRemoteObject<TArray>(gNames);
			if (index < namesArray.NumElements)
			{
				if (namesArray.Data.MayBeValid())
				{
					var nameEntryPtr = process.ReadRemoteIntPtr(namesArray.Data + index * IntPtr.Size);
					if (nameEntryPtr.MayBeValid())
					{
						var nameEntryIndex = process.ReadRemoteInt32(nameEntryPtr + FNameEntryIndexOffset);

						var isWide = (nameEntryIndex & NAME_WIDE_MASK) != 0;

						var name = process.ReadRemoteString(isWide ? Encoding.Unicode : Encoding.ASCII, nameEntryPtr + FNameEntryNameDataOffset, 1024);

						return name;
					}
				}
			}

			return null;
		}
	}
}
