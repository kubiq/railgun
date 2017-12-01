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

        List<IMyMotorStator> rotors = new List<IMyMotorStator>();
        List<IMyShipMergeBlock> mergeBlocks = new List<IMyShipMergeBlock>();
        string rotorName = "Rotor";
        string mergeBlockName = "Merge";
        float currentDisplacement = -.4f;
        const float maxDisplacement = 1.0f;
        const float minDisplacement = -0.4f;
        bool launching = false;

        Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.None;
        }

        public void Main(string argument)
        {

            GridTerminalSystem.GetBlocksOfType(rotors, block => block.CustomName.Contains(rotorName));
            GridTerminalSystem.GetBlocksOfType(mergeBlocks, block => block.CustomName.Contains(mergeBlockName));

            if (!launching) {
                launching = true;
                Echo("Launching");
                Runtime.UpdateFrequency = UpdateFrequency.Update1;

                foreach (IMyShipMergeBlock merge in mergeBlocks)
                {
                    Echo("merge enabled: " + merge.Enabled.ToString());
                    merge.Enabled = false;
                }

            }
            
            // move it
            currentDisplacement += 0.1f;

            foreach (IMyMotorStator rotor in rotors)
            {
                Echo("current displacement: " + rotor.Displacement.ToString());
                rotor.Displacement = currentDisplacement;
            }

            // ended
            if (currentDisplacement > maxDisplacement)
            {
                Echo("Finished");
                
                launching = false;
                currentDisplacement = minDisplacement;
                
                foreach (IMyShipMergeBlock merge in mergeBlocks)
                {
                    Echo("merge enabled: " + merge.Enabled.ToString());
                    merge.Enabled = true;
                }

                Runtime.UpdateFrequency = UpdateFrequency.None;
            }

        }

        

        #region post-script
    }
}
#endregion