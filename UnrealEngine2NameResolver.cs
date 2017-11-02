using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Text;
using ReClassNET.Memory;
using ReClassNET.Util;

namespace UnrealPlugin
{
	internal class UnrealEngine2NameResolver : INameResolver
	{
		[StructLayout(LayoutKind.Sequential)]
		private struct TArray
		{
			public IntPtr Data;
			public int NumElements;
			public int MaxElements;
		}

		public int UObjectNameOffset { get; set; }

		public bool FNameEntryIsWide { get; set; } = true;
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
						var name = process.ReadRemoteString(FNameEntryIsWide ? Encoding.Unicode : Encoding.ASCII, nameEntryPtr + FNameEntryNameDataOffset, 64);

						return name;
					}
				}
			}

			return null;
		}
	}
}
