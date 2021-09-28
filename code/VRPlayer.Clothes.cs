using Sandbox;

partial class VRPlayer
{
	ModelEntity pants;
	ModelEntity jacket;
	ModelEntity shoes;
	ModelEntity hat;

	bool dressed = false;

	public void Dress( bool UsingPuppet = false )
	{
		if ( dressed ) return;
		dressed = true;

		if ( true )
		{
			var model = Rand.FromArray( new[]
			{
				"models/citizen_clothes/trousers/trousers.jeans.vmdl",
				"models/citizen_clothes/trousers/trousers.lab.vmdl",
				"models/citizen_clothes/trousers/trousers.police.vmdl",
				"models/citizen_clothes/trousers/trousers.smart.vmdl",
				"models/citizen_clothes/trousers/trousers.smarttan.vmdl",
				//"models/citizen/clothes/trousers_tracksuit.vmdl",
				"models/citizen_clothes/trousers/trousers_tracksuitblue.vmdl",
				"models/citizen_clothes/trousers/trousers_tracksuit.vmdl",
				"models/citizen_clothes/shoes/shorts.cargo.vmdl",
			} );

			pants = new ModelEntity();
			pants.SetModel( model );
			pants.SetParent( UsingPuppet ? PlayerPuppet : this, true );
			pants.EnableShadowInFirstPerson = true;
			pants.EnableHideInFirstPerson = false;

			PlayerPuppet.SetBodyGroup( "Legs", 1 );
		}

		if ( true )
		{
			var model = Rand.FromArray( new[]
			{
				"models/citizen_clothes/jacket/labcoat.vmdl",
				"models/citizen_clothes/jacket/jacket.red.vmdl",
				"models/citizen_clothes/jacket/jacket.tuxedo.vmdl",
				"models/citizen_clothes/jacket/jacket_heavy.vmdl",
			} );

			jacket = new ModelEntity();
			jacket.SetModel( model );
			jacket.SetParent( UsingPuppet ? PlayerPuppet : this, true );
			jacket.EnableShadowInFirstPerson = true;
			jacket.EnableHideInFirstPerson = false;

			var propInfo = jacket.GetModel().GetPropData();
			if ( propInfo.ParentBodyGroupName != null )
			{
				PlayerPuppet.SetBodyGroup( propInfo.ParentBodyGroupName, propInfo.ParentBodyGroupValue );
			}
			else
			{
				PlayerPuppet.SetBodyGroup( "Chest", 0 );
			}
		}

		if ( true )
		{
			var model = Rand.FromArray( new[]
			{
				"models/citizen_clothes/shoes/trainers.vmdl",
				"models/citizen_clothes/shoes/shoes.workboots.vmdl"
			} );

			shoes = new ModelEntity();
			shoes.SetModel( model );
			shoes.SetParent( UsingPuppet ? PlayerPuppet : this, true );
			shoes.EnableShadowInFirstPerson = true;
			shoes.EnableHideInFirstPerson = false;

			PlayerPuppet.SetBodyGroup( "Feet", 1 );
		}

		if ( false )
		{
			var model = Rand.FromArray( new[]
			{
				"models/citizen_clothes/hat/hat_hardhat.vmdl",
				"models/citizen_clothes/hat/hat_woolly.vmdl",
				"models/citizen_clothes/hat/hat_securityhelmet.vmdl",
				"models/citizen_clothes/hair/hair_malestyle02.vmdl",
				"models/citizen_clothes/hair/hair_femalebun.black.vmdl",
				"models/citizen_clothes/hat/hat_beret.red.vmdl",
				"models/citizen_clothes/hat/hat.tophat.vmdl",
				"models/citizen_clothes/hat/hat_beret.black.vmdl",
				"models/citizen_clothes/hat/hat_cap.vmdl",
				"models/citizen_clothes/hat/hat_leathercap.vmdl",
				"models/citizen_clothes/hat/hat_leathercapnobadge.vmdl",
				"models/citizen_clothes/hat/hat_securityhelmetnostrap.vmdl",
				"models/citizen_clothes/hat/hat_service.vmdl",
				"models/citizen_clothes/hat/hat_uniform.police.vmdl",
				"models/citizen_clothes/hat/hat_woollybobble.vmdl",
			} );

			hat = new ModelEntity();
			hat.SetModel( model );
			hat.SetParent( UsingPuppet ? PlayerPuppet : this, true );
			hat.EnableShadowInFirstPerson = true;
			hat.EnableHideInFirstPerson = false;
		}
	}
}
