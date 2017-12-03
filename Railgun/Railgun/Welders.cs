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
    public class Welders : MyGridProgram
    {
        #endregion

        string welderTag = "RDSA";
        string cockpitBlockTag = "RDSA";

        List<IMyShipWelder> welders = null;
        IMyShipController cockpitBlock = null;

        bool init = false;

        Welders()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Once;
        }

        void Init()
        {
            welders = new List<IMyShipWelder>();
            GridTerminalSystem.GetBlocksOfType(welders, block => { return block.CustomName.IndexOf(welderTag, StringComparison.OrdinalIgnoreCase) >= 0; });

            if (welders.Count > 0)
            {
                List<IMyShipController> cockpitBlocks = new List<IMyShipController>();
                GridTerminalSystem.GetBlocksOfType(cockpitBlocks, block => { return block.CustomName.IndexOf(cockpitBlockTag, StringComparison.OrdinalIgnoreCase) >= 0; });
                if (cockpitBlocks.Count > 0)
                {
                    cockpitBlock = cockpitBlocks[0];

                    init = true;
                }
            }

            if (init)
            {
                Echo("----- System Online -----");

                Echo("\n--[ RDSA Weleders ]--");
                foreach (IMyShipWelder welder in welders)
                {
                    Echo(welder.CustomName);
                }

                Echo("\n--[ Ship Cockpit Block ]--");
                Echo(cockpitBlock.CustomName);
            }
            else
            {
                Echo("No RDSA Welders or Ship Cockpit\n Block Found");
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

            if (cockpitBlock.IsUnderControl)
            {
                foreach (IMyShipWelder welder in welders)
                {
                    welder.ApplyAction("OnOff_On");
                }
            } 
            else
            {
                foreach (IMyShipWelder welder in welders)
                {
                    welder.ApplyAction("OnOff_Off");
                }
            }
        }



        #region post-script
    }
}
#endregion