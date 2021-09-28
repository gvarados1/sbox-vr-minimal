using Sandbox.Rcon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sandbox
{
	public class VRPlayerAnimator : PawnAnimator
	{
		TimeSince TimeSinceFootShuffle = 60;

		public AnimEntity PlayerPuppet;

		public override void SetParam( string name, Vector3 val )
		{
			PlayerPuppet?.SetAnimVector( name, val );
		}
		public override void SetParam( string name, float val )
		{
			PlayerPuppet?.SetAnimFloat( name, val );
		}
		public override void SetParam( string name, bool val )
		{
			PlayerPuppet?.SetAnimBool( name, val );
		}
		public override void SetParam( string name, int val )
		{
			PlayerPuppet?.SetAnimInt( name, val );
		}

		public override void Simulate()
		{
			
			if ( VR.Enabled )
			{

			}
			else {
				//VRRotation = Input.Rotation;
			}

			

			Vector3 aimPos = Pawn.EyePos + Rotation.Forward * 200;
			//Vector3 aimPos = Input.VR.Head.Position + Input.VR.Head.Rotation.Forward * 200;
			Vector3 lookPos = Input.VR.Head.Position + Input.VR.Head.Rotation.Forward * 200;
			//Vector3 lookPos = Input.VR.Head.Position + Input.VR.Head.Rotation.Forward * 200;


			//var idealRotation = Rotation.LookAt( VRRotation.Forward.WithZ( 0 ), Vector3.Up );
			//DoRotation( idealRotation );
			DoWalk();

			//
			// Let the animation graph know some shit
			//
			bool sitting = HasTag( "sitting" );
			bool noclip = HasTag( "noclip" ) && !sitting;

			SetParam( "b_grounded", GroundEntity != null || noclip || sitting );
			SetParam( "b_noclip", noclip );
			SetParam( "b_sit", sitting );
			SetParam( "b_swim", Pawn.WaterLevel.Fraction > 0.5f && !sitting );

			SetParam("aim_head_weight",1f);
			SetParam( "aim_body_weight", 1f );

			//
			// Look in the direction what the player's input is facing
			//

			SetLookAt( "aim_eyes", lookPos );
			SetLookAt( "aim_head", lookPos );
			SetLookAt( "aim_body", aimPos );

			if ( Pawn.ActiveChild is BaseCarriable carry )
			{
				carry.SimulateAnimator( this );
			}
			else
			{
				SetParam( "holdtype", 0 );
				SetParam( "aim_body_weight", 0.5f );
			}

		}

		public virtual void DoRotation( Rotation idealRotation )
		{
			//
			// Our ideal player model rotation is the way we're facing
			//
			var allowYawDiff = Pawn.ActiveChild == null ? 90 : 50;

			float turnSpeed = 0.01f;

			//
			// If we're moving, rotate to our ideal rotation
			//
			Rotation = Rotation.Slerp( Rotation, idealRotation, WishVelocity.Length * Time.Delta * turnSpeed );

			//
			// Clamp the foot rotation to within 120 degrees of the ideal rotation
			//
			Rotation = Rotation.Clamp( idealRotation, allowYawDiff, out var change );

			//
			// If we did restrict, and are standing still, add a foot shuffle
			//
			if ( change > 1 && WishVelocity.Length <= 1 ) TimeSinceFootShuffle = 0;

			SetParam( "b_shuffle", TimeSinceFootShuffle < 0.1 );
		}

		void DoWalk()
		{
			// Move Speed
			{
				var dir = Velocity;
				var forward = Rotation.Forward.Dot( dir );
				var sideward = Rotation.Right.Dot( dir );

				var angle = MathF.Atan2( sideward, forward ).RadianToDegree().NormalizeDegrees();

				SetParam( "move_direction", angle );
				SetParam( "move_speed", Velocity.Length );
				SetParam( "move_groundspeed", Velocity.WithZ( 0 ).Length );
				SetParam( "move_y", sideward );
				SetParam( "move_x", forward );
			}

			// Wish Speed
			{
				var dir = WishVelocity;
				var forward = Rotation.Forward.Dot( dir );
				var sideward = Rotation.Right.Dot( dir );

				var angle = MathF.Atan2( sideward, forward ).RadianToDegree().NormalizeDegrees();

				SetParam( "wish_direction", angle );
				SetParam( "wish_speed", WishVelocity.Length );
				SetParam( "wish_groundspeed", WishVelocity.WithZ( 0 ).Length );
				SetParam( "wish_y", sideward );
				SetParam( "wish_x", forward );
			}
		}

		public override void OnEvent( string name )
		{
			// DebugOverlay.Text( Pos + Vector3.Up * 100, name, 5.0f );

			if ( name == "jump" )
			{
				Trigger( "b_jump" );
			}

			base.OnEvent( name );
		}
	}
}
