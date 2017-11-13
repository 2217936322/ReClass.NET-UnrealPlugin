namespace UnrealPlugin.Config
{
	internal class UnrealEngine1Config : BaseConfig
	{
		public UnrealEngine1Config(UnrealApplicationSettings settings)
			: base(settings)
		{
			FNameEntryIsWide = settings.FNameEntryIsWide;
		}

		public bool FNameEntryIsWide { get; set; }
	}
}
