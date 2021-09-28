using Sandbox;
using System;
using System.Linq;

	partial class VRPlayer : Player
	{
		[Net, Predicted] AnimEntity LeftHand { get; set; }
		[Net, Predicted] AnimEntity RightHand { get; set; }
		[Net, Predicted] public AnimEntity PlayerPuppet { get; set; }

		VRPlayerAnimator AnimatorRef;

		public override void Respawn()
		{
		// broken - waiting for garry to fix
		/*if ( !Input.VR.IsActive )
		{
			Controller = new WalkController();
			Camera = new FirstPersonCamera();
			Animator = new StandardPlayerAnimator();

			SetModel( "models/citizen/citizen.vmdl" );

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			base.Respawn();
			return;
		}*/

			Controller = new VRWalkController();
			Camera = new FirstPersonCamera();

			if ( Animator == null ) // why do I need this?
			{
				Animator = new VRPlayerAnimator();
				AnimatorRef = Animator as VRPlayerAnimator;
			}

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			if(PlayerPuppet != null)
				PlayerPuppet.EnableDrawing = true;

		base.Respawn();
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
			if ( !Input.VR.IsActive ) return;

			// If you have active children (like a weapon etc) you should call this to simulate those too.
			SimulateActiveChild( cl, ActiveChild );

				if ( IsServer)
			{
				// --------------------------------- puppet ------------------------------------
				Transform LocalHead = Transform.ToLocal( Input.VR.Head );

				if ( PlayerPuppet == null )
				{
					PlayerPuppet = new AnimEntity();
					PlayerPuppet.SetModel( "models/citizen/citizen.vmdl" );
					PlayerPuppet.Parent = Local.Pawn;
					AnimatorRef.PlayerPuppet = PlayerPuppet;
					PlayerPuppet.EnableShadowInFirstPerson = true;
					PlayerPuppet.EnableHideInFirstPerson = true;

					PlayerPuppet.SetBodyGroup( "hands", 1 );
					Dress( true );
				}

				// puppet position
				float DuckHeight = (1 - (LocalHead.Position.z / 60f)) * 3f;


				Vector3 DuckOffset = new Vector3( -6 + (DuckHeight * 3) - ((DuckHeight) * 10f), 0, 0 );
				PlayerPuppet.Position = Position + LocalHead.Position.WithZ( 0 ) * Rotation + DuckOffset * PlayerPuppet.Rotation;

				Angles HeadRotation = Input.VR.Head.Rotation.Angles();
				HeadRotation.pitch = 0;
				HeadRotation.roll = 0;

				var allowYawDiff = ActiveChild == null ? 70 : 50;
				float turnSpeed = 0.01f;

				PlayerPuppet.Rotation = Rotation.Slerp( PlayerPuppet.Rotation, HeadRotation.ToRotation(), Time.Delta * turnSpeed );
				PlayerPuppet.Rotation = PlayerPuppet.Rotation.Clamp( HeadRotation.ToRotation(), allowYawDiff );

				PlayerPuppet.SetAnimFloat( "duck", DuckHeight );
				PlayerPuppet.SetAnimBool( "b_vr", true );

				// ---------------------------------- hands -------------------------------------
				if ( LeftHand == null )
				{
					LeftHand= new AnimEntity();
					LeftHand.SetModel( "models/handleft.vmdl" );
				}

				if ( RightHand == null )
				{
					RightHand = new AnimEntity();
					RightHand.SetModel( "models/handright.vmdl" );
				}


				LeftHand.Transform = Input.VR.LeftHand.Transform;
				RightHand.Transform = Input.VR.RightHand.Transform;


				LeftHand.SetAnimFloat( "Thumb", Input.VR.LeftHand.GetFingerValue( FingerValue.ThumbCurl ) );
				LeftHand.SetAnimFloat( "Index", Input.VR.LeftHand.GetFingerValue( FingerValue.IndexCurl ) );
				LeftHand.SetAnimFloat( "Middle", Input.VR.LeftHand.GetFingerValue( FingerValue.MiddleCurl ) );
				LeftHand.SetAnimFloat( "Ring", Input.VR.LeftHand.GetFingerValue( FingerValue.RingCurl ) );

				RightHand.SetAnimFloat( "Thumb", Input.VR.RightHand.GetFingerValue( FingerValue.ThumbCurl ) );
				RightHand.SetAnimFloat( "Index", Input.VR.RightHand.GetFingerValue( FingerValue.IndexCurl ) );
				RightHand.SetAnimFloat( "Middle", Input.VR.RightHand.GetFingerValue( FingerValue.MiddleCurl ) );
				RightHand.SetAnimFloat( "Ring", Input.VR.RightHand.GetFingerValue( FingerValue.RingCurl ) );

				// ---------------------------------- arm ik -------------------------------------
				Transform LocalLeftHand = Transform.ToLocal( Input.VR.LeftHand.Transform );
				Transform LocalRightHand = Transform.ToLocal( Input.VR.RightHand.Transform );

				Vector3 PositionOffsetLeft = new Vector3( -5.5f, 2f, 1f ) * LocalLeftHand.Rotation;
				Vector3 PositionOffsetRight = new Vector3( -5.5f, -2f, 1f ) * LocalRightHand.Rotation;

				Angles RotationOffsetLeft = new Angles( 50f, 0f, 90f );
				Angles RotationOffsetRight = new Angles( 50f, 0f, 90f );

				PlayerPuppet.SetAnimVector( "left_hand_ik.position", PlayerPuppet.Transform.ToLocal( LeftHand.GetBoneTransform( 0 ) ).Position );
				PlayerPuppet.SetAnimVector( "right_hand_ik.position", PlayerPuppet.Transform.ToLocal( RightHand.GetBoneTransform( 0 ) ).Position );

				PlayerPuppet.SetAnimRotation( "left_hand_ik.rotation", PlayerPuppet.Transform.ToLocal( Input.VR.LeftHand.Transform ).Rotation * RotationOffsetLeft.ToRotation() );
				PlayerPuppet.SetAnimRotation( "right_hand_ik.rotation", PlayerPuppet.Transform.ToLocal( Input.VR.RightHand.Transform ).Rotation * RotationOffsetRight.ToRotation() );
			}

		}

		public override void PostCameraSetup( ref CameraSetup setup )
		{
			base.PostCameraSetup( ref setup );

			// disable head for local client only
			if ( PlayerPuppet != null )
				PlayerPuppet.SetBodyGroup( "head", 1 );
		}
		public override void OnKilled()
		{
			base.OnKilled();

			if ( RightHand != null )
				RightHand.Delete();

			if ( LeftHand != null )
				LeftHand.Delete();

			EnableDrawing = false;
			PlayerPuppet.EnableDrawing = false;
		}
	}
