using ScooterController.Configuration;
using ScooterController.HardwareAbstractionLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterController.Controller
{
    class HardwareStatusCommandController : HardwareBaseController
    {
        private Dictionary<int, bool> currentChannelStatus = new Dictionary<int, bool>()
        {
            { HardwareSetting.ChannelMoveForward, false },
            { HardwareSetting.ChannelMoveBack, false },
            { HardwareSetting.ChannelTurnLeft, false },
            { HardwareSetting.ChannelTurnRight, false },
            { HardwareSetting.ChannelSpeedUp, false },
            { HardwareSetting.ChannelBrake, false },
        };

        private readonly Dictionary<string, int> commandChannelMapping = new Dictionary<String, int>()
        {
            { "fd", HardwareSetting.ChannelMoveForward },
            { "bk", HardwareSetting.ChannelMoveBack },
            { "lt", HardwareSetting.ChannelTurnLeft },
            { "rt", HardwareSetting.ChannelTurnRight },
            { "sp", HardwareSetting.ChannelSpeedUp },
            { "st", HardwareSetting.ChannelBrake },
        };

        public void ExecuteStatusCommand(string command)
        {
            if(!commandChannelMapping.ContainsKey(command))
            {
                this.closeOtherChannel(-1);
            }
            else
            {
                var targetChannel = commandChannelMapping[command];
                if (! currentChannelStatus[targetChannel])
                {
                    currentChannelStatus[targetChannel] = true;
                    this.OpenChannel(targetChannel);
                }
                this.closeOtherChannel(targetChannel);
            }
        }

        private void closeOtherChannel(int noCloseChannel) //if set noCloseChannel to -1, close all channels
        {
            var activeChannels = currentChannelStatus.Where(status => status.Value).Select(channel => channel.Key).ToList();
            foreach (var channel in activeChannels)
            {
                if(channel != noCloseChannel)
                {
                    this.CloseChannel(channel);
                    this.currentChannelStatus[channel] = false;
                }
            }
        }
    }
}
