using ScooterController.Configuration;
using ScooterController.HardwareAbstractionLayer;
using System.Collections.Generic;
using System.Linq;

namespace ScooterController.Controller
{
    class HardwareKeyboardController : HardwareBaseController
    {
        private bool isEscapePressed = false;

        private readonly IEnumerable<KeyCode> allKeys;
        private readonly IEnumerable<KeyCode> actionKeys;

        private readonly Dictionary<int, bool> actionChannelStatus = new Dictionary<int, bool>()
        {
            { HardwareSetting.ChannelMoveForward, false },
            { HardwareSetting.ChannelMoveBack, false },
            { HardwareSetting.ChannelTurnLeft, false },
            { HardwareSetting.ChannelTurnRight, false },
            { HardwareSetting.ChannelSpeedUp, false },
            { HardwareSetting.ChannelBrake, false },
        }; 

        private readonly Dictionary<KeyCode, int> keyChannelMapping = new Dictionary<KeyCode, int>()
        {
            { KeyCode.ArrowUp, HardwareSetting.ChannelMoveForward },
            //{ KeyCode.LetterW, HardwareSetting.ChannelMoveForward },
            
            { KeyCode.ArrowDown, HardwareSetting.ChannelMoveBack },
            //{ KeyCode.LetterS, HardwareSetting.ChannelMoveBack },
                 
            { KeyCode.ArrowLeft, HardwareSetting.ChannelTurnLeft },
            //{ KeyCode.LetterA, HardwareSetting.ChannelTurnLeft },
                  
            { KeyCode.ArrowRight, HardwareSetting.ChannelTurnRight },
            //{ KeyCode.LetterD, HardwareSetting.ChannelTurnRight },
            
            { KeyCode.Alt, HardwareSetting.ChannelSpeedUp },

            { KeyCode.SpaceBar, HardwareSetting.ChannelBrake },
            //{ KeyCode.Escape, HardwareSetting.ChannelBrake },
        };

        public HardwareKeyboardController()
        {
            this.actionKeys = this.keyChannelMapping.Select(item => item.Key);
            this.allKeys = this.actionKeys.Concat(new List<KeyCode>() { KeyCode.Escape });
        }

        public void ExecuteKeyboardInstruction()
        {
            var keys = this.GetPressedKeys();

            if (keys.Count == 0)
            {
                this.CloseAllActionChannel();
            }
            else if (keys.Contains(KeyCode.Escape))
            {
                this.CloseAllActionChannel();
                this.isEscapePressed = true;
            }
            else
            {
                foreach (var key in this.actionKeys)
                {
                    var targetStatus = keys.Contains(key);
                    var targetChannel = this.keyChannelMapping[key];
                    var currentStatus = this.actionChannelStatus[targetChannel];

                    if (targetStatus != currentStatus)
                    {
                        if (targetStatus == true)
                        {
                            this.OpenChannel(targetChannel);
                            LogInfo("open for " + key);
                        }
                        else
                        {
                            this.CloseChannel(targetChannel);
                            LogInfo("close for " + key);
                        }

                        this.actionChannelStatus[targetChannel] = targetStatus;
                    }
                }
            }
        }

        public bool ShouldExit()
        {
            return this.isEscapePressed;
        }

        private void CloseAllActionChannel()
        {
            var activeChannels = this.actionChannelStatus.Where(channelStatus => channelStatus.Value).Select(channel => channel.Key).ToList();
            foreach (var channel in activeChannels)
            {
                LogInfo("close");
                this.CloseChannel(channel);
                this.actionChannelStatus[channel] = false;
            }
        }

        private List<KeyCode> GetPressedKeys()
        {
            var pressedKeys = new List<KeyCode>();
            foreach (var key in this.allKeys)
            {
                if (NativeKeyboard.IsKeyDown(key))
                {
                    pressedKeys.Add(key);
                };
            }

            return pressedKeys;
        }
    }
}
