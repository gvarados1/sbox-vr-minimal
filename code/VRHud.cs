using Sandbox.UI;

	/// <summary>
	/// This is the HUD entity. It creates a RootPanel clientside, which can be accessed
	/// via RootPanel on this entity, or Local.Hud.
	/// </summary>
	public partial class VRHudEntity : Sandbox.HudEntity<RootPanel>
	{
		public VRHudEntity()
		{
			if ( IsClient )
			{
				RootPanel.SetTemplate( "/vrhud.html" );
			}
		}
	}
