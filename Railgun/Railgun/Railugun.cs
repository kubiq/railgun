#region pre-script
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using VRageMath;
using VRage.Game;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Ingame;
using Sandbox.Game.EntityComponents;
using VRage.Game.Components;
using VRage.Collections;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;

namespace IngameScript
{
    public class Program : MyGridProgram
    {
        #endregion

        string rotorTag = "RDSA";
        string mergeBlockTag = "RDSA";

        List<IMyMotorStator> rdsaRotors = null;
        IMyShipMergeBlock mergeBlock = null;

        int state = 0;

        bool init = false;

        Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Once;
        }

        void Init()
        {
            rdsaRotors = new List<IMyMotorStator>();
            GridTerminalSystem.GetBlocksOfType(rdsaRotors, block => { return block.CustomName.IndexOf(rotorTag, StringComparison.OrdinalIgnoreCase) >= 0; });

            if (rdsaRotors.Count > 0)
            {
                List<IMyShipMergeBlock> mergeBlocks = new List<IMyShipMergeBlock>();
                GridTerminalSystem.GetBlocksOfType(mergeBlocks, block => { return block.CustomName.IndexOf(mergeBlockTag, StringComparison.OrdinalIgnoreCase) >= 0; });
                if (mergeBlocks.Count > 0)
                {
                    mergeBlock = mergeBlocks[0];

                    foreach (IMyMotorStator rotor in rdsaRotors)
                    {
                        rotor.SetValueBool("ShareInertiaTensor", true);
                    }

                    state = 0;
                    init = true;
                }
            }

            if (init)
            {
                Echo("----- System Online -----");

                Echo("\n--[ RDSA Rotors ]--");
                foreach (IMyMotorStator rotor in rdsaRotors)
                {
                    Echo(rotor.CustomName);
                }

                Echo("\n--[ Projectile Holder Merge Block ]--");
                Echo(mergeBlock.CustomName);
            }
            else
            {
                Echo("No RDSA Rotors Or Projectile Holder\nMerge Block Found");
            }
        }

        void Main(string arguments, UpdateType updateSource)
        {
            if (arguments.Length > 0 && arguments.Trim().Equals("RESET", StringComparison.OrdinalIgnoreCase))
            {
                init = false;
                Init();

                return;
            }

            if (!init)
            {
                Init();

                if (!init)
                {
                    return;
                }

                if ((updateSource & UpdateType.Once) > 0)
                {
                    return;
                }
            }

            if (state > 0 && ((updateSource & UpdateType.Update1) == 0))
            {
                return;
            }

            switch (state)
            {
                case 0:
                    Runtime.UpdateFrequency = UpdateFrequency.Update1;

                    foreach (IMyMotorStator rotor in rdsaRotors)
                    {
                        rotor.SetValueFloat("Displacement", 0.2f);
                    }
                    break;
                case 1:
                    foreach (IMyMotorStator rotor in rdsaRotors)
                    {
                        rotor.SetValueFloat("Displacement", -0.4f);
                    }
                    break;
                case 2:
                    foreach (IMyMotorStator rotor in rdsaRotors)
                    {
                        rotor.SetValueFloat("Displacement", 0.2f);
                    }
                    break;
                case 3:
                    foreach (IMyMotorStator rotor in rdsaRotors)
                    {
                        rotor.SetValueFloat("Displacement", -0.4f);
                    }
                    mergeBlock.ApplyAction("OnOff_Off");
                    break;
                case 4:
                    Runtime.UpdateFrequency = UpdateFrequency.None;
                    state = -1;

                    mergeBlock.ApplyAction("OnOff_On");
                    break;
            }
            state++;
        }



        #region post-script
    }
}
#endregion