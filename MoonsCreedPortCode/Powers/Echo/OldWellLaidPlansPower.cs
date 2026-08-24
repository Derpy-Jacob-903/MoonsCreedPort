using System.Reflection;
using System.Reflection.Emit;
using BaseLib.Hooks;
using BaseLib.Utils;
using BaseLib.Utils.Patching;
using HarmonyLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Runs;

namespace MoonsCreedPort.MoonsCreedPortCode.Powers.Echo;

public class OldWellLaidPlansPower : MoonsCreedPortPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>
    /// We use BeforeFlushLate instead of BeforeFlush here so that the player can have full information about the other
    /// BeforeFlush effects before choosing a card to retain.
    /// </summary>
    public override async Task BeforeFlushLate(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player || !Hook.ShouldFlush(player.Creature.CombatState, player))
            return;
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 0, Amount);
        List<CardModel> list = (await CardSelectCmd.FromHand(choiceContext, Owner.Player, prefs, new Func<CardModel, bool>(RetainFilter), (AbstractModel) this)).ToList<CardModel>();
        if (list.Count == 0)
            return;
        foreach (CardModel cardModel in list)
            cardModel.GiveSingleTurnRetain();
    }

    private bool RetainFilter(CardModel card) => !card.ShouldRetainThisTurn;
}