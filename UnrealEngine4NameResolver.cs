using System;
using System.Diagnostics.Contracts;
using System.Text;
using ReClassNET.Memory;
using ReClassNET.Util;

namespace UnrealPlugin
{
	internal class UnrealEngine4NameResolver : BaseNameResolver
	{
		private const int TNameEntryArray_MaxTotalElements = 2 * 1024 * 1024;
		private const int TNameEntryArray_ElementsPerChunk = 16384;
		private const int TNameEntryArray_ChunkTableSize = (TNameEntryArray_MaxTotalElements + TNameEntryArray_ElementsPerChunk - 1) / TNameEntryArray_ElementsPerChunk;

		private const int NameWideMask = 1;
		private const int NameIndexShift = 1;

		public UnrealEngine4NameResolver(RemoteProcess process, UnrealEngine4Config config)
			: base(process, config)
		{
			Contract.Requires(process != null);
			Contract.Requires(config != null);
		}

		protected override IntPtr ReadNameEntryPtr(int index)
		{
			Contract.Requires(index > 0);

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
			memory.Update(config.GlobalArrayPtr, false);

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

					return namePtr;
				}
			}

			return IntPtr.Zero;
		}

		protected override string ReadNameFromNameEntry(IntPtr nameEntryPtr, int index)
		{
			var nameEntryIndex = process.ReadRemoteInt32(nameEntryPtr + ((UnrealEngine4Config)config).FNameEntryIndexOffset);
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
