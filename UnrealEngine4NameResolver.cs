using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Text;
using ReClassNET.Memory;
using ReClassNET.Util;

namespace UnrealPlugin
{
	internal class UnrealEngine4NameResolver : INameResolver
	{
		private const int TNameEntryArray_MaxTotalElements = 2 * 1024 * 1024;
		private const int TNameEntryArray_ElementsPerChunk = 16384;
		private const int TNameEntryArray_ChunkTableSize = (TNameEntryArray_MaxTotalElements + TNameEntryArray_ElementsPerChunk - 1) / TNameEntryArray_ElementsPerChunk;

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

			/*
			class TStaticIndirectArrayThreadSafeRead
			{
				enum
				{
					ChunkTableSize = (MaxTotalElements + ElementsPerChunk - 1) / ElementsPerChunk
				};
				ElementType** Chunks[ChunkTableSize];
				int32 NumElements;
				int32 NumChunks;
			}
			*/

			var memory = new MemoryBuffer(TNameEntryArray_ChunkTableSize * IntPtr.Size + sizeof(int) * 2);
			memory.Update(gNames, false);

			var numElements = memory.ReadInt32(TNameEntryArray_ChunkTableSize * IntPtr.Size);
			var numChunks = memory.ReadInt32(TNameEntryArray_ChunkTableSize * IntPtr.Size + sizeof(int));

			var indexChunk = index / TNameEntryArray_ElementsPerChunk;
			var indexName = index % TNameEntryArray_ElementsPerChunk;

			if (index < numElements && indexChunk < numChunks)
			{
				var chunkPtr = memory.ReadIntPtr(indexChunk * IntPtr.Size);
				if (chunkPtr.MayBeValid())
				{
					var namePtr = process.ReadRemoteIntPtr(chunkPtr + indexName * IntPtr.Size);

					var nameEntryIndex = process.ReadRemoteInt32(namePtr + FNameEntryIndexOffset);
					if (nameEntryIndex >> NAME_INDEX_SHIFT == index)
					{
						var isWide = (nameEntryIndex & NAME_WIDE_MASK) != 0;

						var name = process.ReadRemoteString(isWide ? Encoding.Unicode : Encoding.ASCII, namePtr + FNameEntryNameDataOffset, 1024);

						return name;
					}
				}
			}

			return null;
		}
	}
}
