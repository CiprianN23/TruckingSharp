using SampSharp.GameMode;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.Display;
using SampSharp.GameMode.SAMP;
using System;
using System.Threading.Tasks;
using TruckingSharp.Constants;
using TruckingSharp.Database;
using TruckingSharp.Database.Entities;
using TruckingSharp.Database.Repositories;
using TruckingSharp.Events;
using TruckingSharp.Extensions.PlayersExtensions;
using TruckingSharp.Services;

namespace TruckingSharp.Controllers
{
    [Controller]
    public class PlayerAccountController : IController, IEventListener
    {
        private PlayerAccountRepository _accountRepository => new PlayerAccountRepository(DapperConnection.ConnectionString);

        public event EventHandler<PlayerLoginEventArgs> PlayerLogin;

        public void RegisterEvents(BaseMode gameMode)
        {
            PlayerLogin += PlayerAccountController_PlayerLogin;
            gameMode.PlayerConnected += GameMode_PlayerConnected;
        }

        private void GameMode_PlayerConnected(object sender, EventArgs e)
        {
            Player player = sender as Player;

            if (player.Account == null)
                RegisterPlayer(player);
            else
                LoginPlayer(player);
        }

        private void LoginPlayer(Player player)
        {
            var message = $"Insert your password. Tries left: {player.LoginTries}/{Configuration.MaxLogins}";
            var dialog = new InputDialog("Login", message, true, "Login", "Cancel");
            dialog.Show(player);
            dialog.Response += async (sender, ev) =>
            {
                if (ev.DialogButton == DialogButton.Left)
                {
                    if (player.LoginTries >= Configuration.MaxLogins)
                    {
                        player.SendClientMessage(Color.OrangeRed, "You exceed maximum login tries. You have been kicked!");
                        await Task.Delay(Configuration.KickDelay);
                        player.Kick();
                    }
                    else if (PasswordHashingService.VerifyPasswordHash(ev.InputText, player.Account.Password))
                    {
                        player.IsLoggedIn = true;

                        PlayerLogin?.Invoke(player, new PlayerLoginEventArgs() { Success = true });
                    }
                    else
                    {
                        player.LoginTries++;
                        player.SendClientMessage(Color.Red, "Wrong password");

                        dialog.Message = $"Wrong password! Retype your password! Tries left: {player.LoginTries}/{Configuration.MaxLogins}";

                        LoginPlayer(player);
                    }
                }
                else
                {
                    player.Kick();
                }
            };
        }

        private void PlayerAccountController_PlayerLogin(object sender, PlayerLoginEventArgs e)
        {
            Player player = sender as Player;

            if (e.Success)
            {
                player.ToggleSpectating(false);
                player.Money = player.Account.Money;
                player.Score = player.Account.Score;
            }
        }

        private void RegisterPlayer(Player player)
        {
            player.ShowPlayerInputDialog("Register", "Insert your password", true, "Submit", "Cancel", async (senderPlayer, ev) =>
            {
                if (ev.DialogButton == DialogButton.Left)
                {
                    var hash = PasswordHashingService.GetPasswordHash(ev.InputText);

                    var newAccount = new PlayerAccount { Name = player.Name, Password = hash };

                    await _accountRepository.AddAsync(newAccount);

                    LoginPlayer(player);
                }
                else
                {
                    player.Kick();
                }
            });
        }
    }
}