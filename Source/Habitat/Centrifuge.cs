using System;
//using System.Linq;
using UnityEngine;

namespace Habitat
{
	public class Centrifuge : PartModule
	{
		[KSPField]
		public string rotorTransformName = "center";

		public Transform rotorTransform;

		[KSPField]
		public string flywheelTransformName = "flywheel";

		public Transform flywheelTransform;

		[KSPField]
		public float rotorRPM = 0f;

		[KSPField]
		public float flywheelRotationMult = 0f;

		[KSPField]
		public float acceleration = 0.005f;

		[KSPField]
		public float habRadius = 0f;

		[KSPField(isPersistant = true)]
		private float speedMult = 0f;

		[KSPField]
		public string animationName = "";

		[KSPField(guiActive = true, guiActiveEditor = true, guiFormat = "F2", guiName = "Artificial Gravity", guiUnits = "g", isPersistant = false)]
		public float currentGeeforce = 0f;

		private float geeforce = 0f;

		private bool startrot = false;

		private Animation anim;

		public override void OnStart(PartModule.StartState state)
		{
			this.rotorTransform = base.part.FindModelTransform(this.rotorTransformName);
			this.flywheelTransform = base.part.FindModelTransform(this.flywheelTransformName);
			this.geeforce = this.habRadius * Mathf.Pow(3.14159274f * this.rotorRPM / 30f, 2f) / 9.81f;
		}

		private void Update()
		{
            this.anim = base.part.FindModelAnimators(this.animationName)[0];
			this.rotorTransform.Rotate(new Vector3(0f, 6f, 0f) * this.rotorRPM * this.speedMult * Time.deltaTime);
			this.flywheelTransform.Rotate(new Vector3(0f, -6f, 0f) * this.rotorRPM * this.speedMult * this.flywheelRotationMult * Time.deltaTime);
			this.currentGeeforce = this.habRadius * Mathf.Pow(3.14159274f * this.rotorRPM * this.speedMult / 30f, 2f) / 9.81f;
			if (this.anim[this.animationName].normalizedTime == 1f)
			{
				this.startrot = true;
			}
			else
			{
				this.startrot = false;
			}
			if (this.startrot && this.speedMult < 1f)
			{
				this.speedMult += this.acceleration;
			}
			if (!this.startrot && this.speedMult > 0f)
			{
				this.speedMult -= this.acceleration;
			}
			if (this.speedMult < 0f)
			{
				this.speedMult = 0f;
			}
		}

		public override string GetInfo()
		{
			string str = "";
			str = str + "<b>Artificial gravity: </b> " + (this.habRadius * Mathf.Pow(3.14159274f * this.rotorRPM / 30f, 2f) / 9.81f).ToString("0.00") + "g";
			return str + "\n<b>Speed of rotation: </b> " + this.rotorRPM.ToString("0") + "rpm";
		}
	}
}
