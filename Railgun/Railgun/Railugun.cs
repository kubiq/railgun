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
        List<IMyShipWelder> welderBlocks = new List<IMyShipWelder>();
        string rotorName = "Rotor";
        string mergeBlockName = "Merge";
        string welderBlockName = "Welder";
        float currentDisplacement = -1.0f;
        const float maxDisplacement = 3.0f;
        const float minDisplacement = -1.0f;
        double tick = 0;

        Program()
        {
            Echo("Install program");
            Runtime.UpdateFrequency = UpdateFrequency.None;
            tick = 0;


        }

        public void Main(string argument)
        {


            GridTerminalSystem.GetBlocksOfType(rotors, block => block.CustomName.Contains(rotorName));
            GridTerminalSystem.GetBlocksOfType(mergeBlocks, block => block.CustomName.Contains(mergeBlockName));
            //GridTerminalSystem.GetBlocksOfType(welderBlocks, block => block.CustomName.Contains(welderBlockName));

            Echo("Nasel jsem " + rotors.Count + "motoru");
            Echo("Nasel jsem " + mergeBlocks.Count + "merge");
            //Echo("Nasel jsem " + welderBlocks.Count + "welderu");

            Echo("Tick:" + tick.ToString());
         
            // start
            if (tick == 0) {
                Echo("Starting");
                Runtime.UpdateFrequency = UpdateFrequency.Update10;
            }

            tick++;

            if ((tick >= 1) && (tick < 10))  // move it
            {
                
                currentDisplacement += 0.4f;
                Echo("current displacement var: " + currentDisplacement);

                foreach (IMyMotorStator rotor in rotors)
                {
                    Echo("motor displacement: " + rotor.Displacement.ToString());
                    rotor.Displacement = currentDisplacement;
                }
            }

            if (tick == 10)
            {
                Echo("Launching");
                currentDisplacement = minDisplacement;

                foreach (IMyShipMergeBlock merge in mergeBlocks)
                {
                    merge.Enabled = false;
                }

                foreach (IMyMotorStator rotor in rotors)
                {
                    rotor.Displacement = minDisplacement;
                }
            }

            // ended
            if (tick == 15)
            {
                Echo("Reseting merge block");
                
                foreach (IMyShipMergeBlock merge in mergeBlocks)
                {
                    merge.Enabled = true;
                }

                //Echo("Turning on welders");
                //foreach (IMyShipWelder welder in welderBlocks)
                //{
                    //welder.Enabled = true;
                //}
            }

            if (tick >= 59)
            {
                Echo("Turning off welders");
                foreach (IMyShipWelder welder in welderBlocks)
                {
                    welder.Enabled = false;
                }

                foreach (IMyShipMergeBlock merge in mergeBlocks)
                {
                    merge.Enabled = true;
                }
            }

            if (tick >= 60)
            {
                Echo("Ended program");
                tick = 0;
                Runtime.UpdateFrequency = UpdateFrequency.None;
            }
        }

        

        #region post-script
    }
}
#endregion