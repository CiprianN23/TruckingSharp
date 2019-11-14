using SampSharp.GameMode.Display;
using SampSharp.GameMode.SAMP;
using TruckingSharp.Database.Repositories;

namespace TruckingSharp.Extensions.PlayersExtensions
{
    public static class PlayerBankExtension
    {
        public static void ShowBankAccountOptions(this Player player)
        {
            string title = $"Your bank funds: {{00FF00}}${player.BankAccount.Money}";
            string message = string.Empty;

            var bankOptionsDialog = new ListDialog(title, "Select", "Cancel");
            bankOptionsDialog.AddItem("{00FF00}Deposit money");
            bankOptionsDialog.AddItem("{00FF00}Withdraw money");
            bankOptionsDialog.AddItem("{00FF00}Transfer money");
            bankOptionsDialog.AddItem("{00FF00}Cancel bank account");

            bankOptionsDialog.Show(player);
            bankOptionsDialog.Response += (sender, e) =>
            {
                if (e.DialogButton == SampSharp.GameMode.Definitions.DialogButton.Left)
                {
                    switch (e.ListItem)
                    {
                        case 0:
                            message = $"Your money: {{00FF00}}${player.Account.Money}\n{{FFFFFF}}Bank account funds: {{00FF00}}${player.BankAccount.Money}" +
                            $"\n \n{{FFFFFF}}Please enter the amount of money you want to deposit into your bank account:";

                            var moneyDepositDialog = new InputDialog("Enter amount of money", message, false, "Accept", "Cancel");
                            moneyDepositDialog.Show(player);
                            moneyDepositDialog.Response += MoneyDepositDialog_Response;
                            break;

                        case 1:
                            if (player.BankAccount.Money <= 0)
                            {
                                player.SendClientMessage(Color.Red, "You can't withdraw from an empty bank account.");
                                player.ShowBankAccountOptions();
                                return;
                            }

                            message = $"Your money: {{00FF00}}${player.Account.Money}\n{{FFFFFF}}Bank account funds: {{00FF00}}${player.BankAccount.Money}" +
                            $"\n \n{{FFFFFF}}Please enter the amount of money you want to withdraw into your bank account:";

                            var moneyWithdrawDialog = new InputDialog("Enter amount of money", message, false, "Accept", "Cancel");
                            moneyWithdrawDialog.Show(player);
                            moneyWithdrawDialog.Response += MoneyWithdrawDialog_Response;
                            break;

                        case 2:
                            if (player.BankAccount.Money < 1)
                            {
                                player.SendClientMessage(Color.Red, "You can't transfer money from an empty bank account.");
                                player.ShowBankAccountOptions();
                                return;
                            }

                            message = $"Your money: {{00FF00}}${player.Account.Money}\n{{FFFFFF}}Bank account funds: {{00FF00}}${player.BankAccount.Money}" +
                            $"\n \n{{FFFFFF}}Please enter the amount of money you want to transfer to another player's bank account:";

                            var moneyTransferDialog = new InputDialog("Enter amount of money", message, false, "Accept", "Cancel");
                            moneyTransferDialog.Show(player);
                            moneyTransferDialog.Response += MoneyTransferDialog_Response;
                            break;

                        case 3:
                            string hasMoneyMessage = string.Empty;
                            if (player.BankAccount.Money > 0)
                            {
                                hasMoneyMessage = $"{{FFFFFF}}Your bank account has {{00FF00}}${player.BankAccount.Money}{{FFFFFF}} in it\n{{FFFFFF}}Your bank funds will be returned to you when you cancel your bank account\n";
                            }

                            message = $"{hasMoneyMessage}{{FFFFFF}}Are you sure you want to cancel your bank account?";

                            var bankCancelDialog = new MessageDialog("Are you sure?", message, "Yes", "No");
                            bankCancelDialog.Show(player);
                            bankCancelDialog.Response += BankCancelDialog_Response;
                            break;
                    }
                }
            };
        }

        private static async void BankCancelDialog_Response(object sender, SampSharp.GameMode.Events.DialogResponseEventArgs e)
        {
            if (e.DialogButton == SampSharp.GameMode.Definitions.DialogButton.Left)
            {
                var player = e.Player as Player;

                if (player.BankAccount.Money > 0)
                {
                    player.Reward(player.BankAccount.Money);

                    player.SendClientMessage(Color.GreenYellow, "There was still some money in your bank account, it has been added to your account.");
                }

                var playerBankAccount = player.BankAccount;
                await new PlayerBankAccountRepository().DeleteAsync(playerBankAccount);

                player.IsLoggedInBankAccount = false;

                player.SendClientMessage(Color.GreenYellow, "Your bank account has been deleted.");
            }
        }

        private static void MoneyTransferDialog_Response(object sender, SampSharp.GameMode.Events.DialogResponseEventArgs e)
        {
            if (e.DialogButton == SampSharp.GameMode.Definitions.DialogButton.Left)
            {
                var player = e.Player as Player;
                if (int.TryParse(e.InputText, out int transferMoney))
                {
                    if (transferMoney < 1)
                    {
                        player.SendClientMessage(Color.Red, "You need to enter a positive amount of money to transfer money to another player's account.");
                        player.ShowBankAccountOptions();
                        return;
                    }

                    if (transferMoney > player.BankAccount.Money)
                    {
                        player.SendClientMessage(Color.Red, "You don't have that amount of money in your bank account to transfer to another player's account.");
                        player.ShowBankAccountOptions();
                        return;
                    }

                    string message = $"Your money: {player.Account.Money}\nBank account funds: {player.BankAccount.Money}\nRequested transfer amount: {transferMoney}\n\nPlease enter the name of the player who must receive your money-transfer:";

                    var playerNameDialog = new InputDialog("Enter player name", message, false, "Accept", "Cancel");
                    playerNameDialog.Show(player);
                    playerNameDialog.Response += async (senderObject, ev) =>
                    {
                        if (ev.DialogButton == SampSharp.GameMode.Definitions.DialogButton.Left)
                        {
                            if (string.IsNullOrEmpty(ev.InputText))
                            {
                                player.SendClientMessage(Color.Red, "You need to enter a valid player name.");
                                playerNameDialog.Show(player);
                                return;
                            }

                            if (ev.InputText.Equals(player.Name, System.StringComparison.OrdinalIgnoreCase))
                            {
                                player.SendClientMessage(Color.Red, "You can't transfer money to yourself.");
                                playerNameDialog.Show(player);
                                return;
                            }

                            if (new PlayerBankAccountRepository().Find(new PlayerAccountRepository().Find(ev.InputText).Id) == null)
                            {
                                player.SendClientMessage(Color.Red, "That player doesn't have a bank account.");
                                playerNameDialog.Show(player);
                                return;
                            }

                            var playerBankAccount = player.BankAccount;
                            var otherBankAccount = new PlayerBankAccountRepository().Find(new PlayerAccountRepository().Find(ev.InputText).Id);

                            otherBankAccount.Money += transferMoney;
                            playerBankAccount.Money -= transferMoney;

                            await new PlayerBankAccountRepository().UpdateAsync(playerBankAccount);
                            await new PlayerBankAccountRepository().UpdateAsync(otherBankAccount);

                            player.SendClientMessage(Color.GreenYellow, $"{{0FF00}}You have transferred {{FFFF00}}${transferMoney}{{00FF00}} to {{FFFF00}}{ev.InputText}{{00FF00}}'s bank account.");

                            // TODO: Inform other player about the transfer
                        }
                    };
                }
                else
                {
                    player.SendClientMessage(Color.Red, "You need input a valid number.");
                    player.ShowBankAccountOptions();
                    return;
                }
            }
        }

        private static async void MoneyWithdrawDialog_Response(object sender, SampSharp.GameMode.Events.DialogResponseEventArgs e)
        {
            if (e.DialogButton == SampSharp.GameMode.Definitions.DialogButton.Left)
            {
                var player = e.Player as Player;
                if (int.TryParse(e.InputText, out int withdrawMoney))
                {
                    if (withdrawMoney < 1)
                    {
                        player.SendClientMessage(Color.Red, "You need to enter a positive amount of money to withdraw from your bank account.");
                        player.ShowBankAccountOptions();
                        return;
                    }

                    if (withdrawMoney > player.BankAccount.Money)
                    {
                        player.SendClientMessage(Color.Red, "You don't have that amount of money to withdraw from your bank account.");
                        player.ShowBankAccountOptions();
                        return;
                    }

                    var playerBankAccount = player.BankAccount;
                    playerBankAccount.Money -= withdrawMoney;

                    player.Reward(withdrawMoney);

                    await new PlayerBankAccountRepository().UpdateAsync(playerBankAccount);

                    player.SendClientMessage(Color.GreenYellow, $"You have withdrawn {{FFFF00}}${withdrawMoney}{Color.GreenYellow} from your bank account.");
                    player.ShowBankAccountOptions();
                }
                else
                {
                    player.SendClientMessage(Color.Red, "You need input a valid number.");
                    player.ShowBankAccountOptions();
                    return;
                }
            }
        }

        private static async void MoneyDepositDialog_Response(object sender, SampSharp.GameMode.Events.DialogResponseEventArgs e)
        {
            if (e.DialogButton == SampSharp.GameMode.Definitions.DialogButton.Left)
            {
                var player = e.Player as Player;
                if (int.TryParse(e.InputText, out int depositMoney))
                {
                    if (depositMoney < 1)
                    {
                        player.SendClientMessage(Color.Red, "You need to enter a positive amount of money to deposit into your bank account.");
                        player.ShowBankAccountOptions();
                        return;
                    }

                    if (depositMoney > player.Account.Money)
                    {
                        player.SendClientMessage(Color.Red, "You don't have that amount of money to deposit into your bank account.");
                        player.ShowBankAccountOptions();
                        return;
                    }

                    var playerBankAccount = player.BankAccount;
                    playerBankAccount.Money += depositMoney;

                    player.Reward(-depositMoney);

                    await new PlayerBankAccountRepository().UpdateAsync(playerBankAccount);

                    player.SendClientMessage(Color.GreenYellow, $"You have deposited {{FFFF00}}${depositMoney}{Color.GreenYellow} into your bank account.");
                    player.ShowBankAccountOptions();
                }
                else
                {
                    player.SendClientMessage(Color.Red, "You need input a valid number.");
                    player.ShowBankAccountOptions();
                    return;
                }
            }
        }
    }
}