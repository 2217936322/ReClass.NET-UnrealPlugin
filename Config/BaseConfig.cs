using System;

namespace UnrealPlugin.Config
{
	internal class BaseConfig
	{
		public BaseConfig()
		{
		}

		public BaseConfig(UnrealApplicationSettings settings)
		{
			UObjectNameOffset = settings.UObjectNameOffset;
			UObjectOuterOffset = settings.UObjectOuterOffset;

			FNameEntryIndexOffset = settings.FNameEntryIndexOffset;
			FNameEntryNameDataOffset = settings.FNameEntryNameDataOffset;

			DisplayFullName = settings.DisplayFullName;
		}

		public IntPtr GlobalArrayPtr { get; set; }

		public int UObjectNameOffset { get; set; }
		public int UObjectOuterOffset { get; set; }

		public int FNameEntryIndexOffset { get; set; }
		public int FNameEntryNameDataOffset { get; set; }

		public bool DisplayFullName { get; set; }
	}
}
