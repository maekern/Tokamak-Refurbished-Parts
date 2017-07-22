using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Habitat
{
	public class DeployableHabitat : PartModule
	{
		[KSPField]
		public string animationName = "";

		[KSPField]
		public int crewCapacityDeployed = 0;

		[KSPField]
		public int crewCapacityRetracted = 0;

		private Animation anim;

		public override void OnStart(PartModule.StartState state)
		{
			base.OnStart(state);
		}

		public override string GetInfo()
		{
			string text = "";
			text = text + "<b>Crew capacity deployed: </b>" + this.crewCapacityDeployed.ToString("0");
			if (this.crewCapacityRetracted > 0)
			{
				text = text + "\n<b>Crew capacity retracted: </b>" + this.crewCapacityRetracted.ToString("0");
			}
			else
			{
				text += "\n<b><color=orange>Can't be crewed while retracted</color></b>";
			}
			return text;
		}

		public void Update()
		{
			anim = part.FindModelAnimators(this.animationName)[0];
			if (anim[animationName].normalizedTime < 1f)
			{
				part.CrewCapacity = this.crewCapacityRetracted;
			}
			if (this.anim[this.animationName].normalizedTime == 1f)
			{
				base.part.CrewCapacity = this.crewCapacityDeployed;
			}
			ModuleAnimateGeneric moduleAnimateGeneric = (ModuleAnimateGeneric)base.part.GetComponent("ModuleAnimateGeneric");
			if (base.part.protoModuleCrew.Count > 0)
			{
				foreach (BaseEvent current in base.part.GetComponent<ModuleAnimateGeneric>().Events)
				{
					if (current.guiName == moduleAnimateGeneric.endEventGUIName)
					{
						current.guiActive = false;
					}
				}
			}
			else
			{
				foreach (BaseEvent current in base.part.GetComponent<ModuleAnimateGeneric>().Events)
				{
					if (current.guiName == moduleAnimateGeneric.endEventGUIName)
					{
						current.guiActive = true;
					}
				}
			}
			base.OnUpdate();
		}
	}
}
