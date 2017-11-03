using System;

namespace UnrealPlugin
{
	internal class BaseConfig
	{
		public IntPtr GlobalArrayPtr { get; set; }

		public int UObjectNameOffset { get; set; }

		public int FNameEntryNameDataOffset { get; set; }
	}
}
