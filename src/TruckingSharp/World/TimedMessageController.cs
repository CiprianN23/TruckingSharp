using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;
using System;
using System.Collections.Generic;

namespace TruckingSharp.World
{
    [Controller]
    public class TimedMessageController : IEventListener
    {
        private readonly List<TimedMessage> _timedMessages = new List<TimedMessage>
        {
            new TimedMessage("You want to refuel your vehicle? Park on a refuel-pickup and honk the horn."),
            new TimedMessage("You want to start a convoy? Enter \"/convoy\" to start or join one."),
            new TimedMessage("You want to teleport to a house you own? Use \"/gohome\" to go home."),
            new TimedMessage("You want to rent a vehicle? Go visit Wang Cars in Doherty, San Fierro."),
            new TimedMessage(
                "You're tired of random trucker/busdriver missions? Visit the driving school in Doherty and buy a license."),
            new TimedMessage("Beware of speedtraps (60kph in the city, 90kph on roads, 120kph on highways)."),
            new TimedMessage("You want to teleport to a business you own? Use \"/gobus\" to go to your business.")
        };

        private int _lastTimedMessageIndex;

        private Timer _timedMessageTimer;

        public void RegisterEvents(BaseMode gameMode)
        {
            gameMode.Initialized += TimeMessage_GamemodeInitialized;
        }

        private void TimedMessageTimer_Tick(object sender, EventArgs e)
        {
            BasePlayer.SendClientMessageToAll(Color.LightGray, _timedMessages[_lastTimedMessageIndex].Message);

            _lastTimedMessageIndex++;

            if (_lastTimedMessageIndex == _timedMessages.Count)
                _lastTimedMessageIndex = 0;
        }

        private void TimeMessage_GamemodeInitialized(object sender, EventArgs e)
        {
            _timedMessageTimer = new Timer(TimeSpan.FromMinutes(2), true);
            _timedMessageTimer.Tick += TimedMessageTimer_Tick;
        }
    }
}