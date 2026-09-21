using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoonsCreedBosses.MoonsCreedBossesCode.Powers;

public class HeirlessPower : MoonsCreedBossesPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterApplied(Creature applier, CardModel cardSource)
    {
        var smoggyPower = this;
        smoggyPower.Flash();
        if (smoggyPower.Owner.Player != null)
            foreach (CardModel allCard in smoggyPower.Owner.Player.PlayerCombatState.AllCards)
            {
                if (allCard.Type == CardType.Attack && allCard.Affliction == null)
                {
                    Smog smog = await CardCmd.Afflict<Smog>(allCard, 1M);
                }
            }
    }
    
    public override async Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side == CombatSide.Player && this.Owner.IsPlayer)
        {
            var smoggyPower = this;
            smoggyPower.Flash();
            if (smoggyPower.Owner.Player?.PlayerCombatState != null)
                foreach (CardModel allCard in smoggyPower.Owner.Player.PlayerCombatState.AllCards)
                {
                    if (allCard.Type == CardType.Skill && allCard.Affliction == null)
                    {
                        Smog smog = await CardCmd.Afflict<Smog>(allCard, 1M);
                    }
                }
        }
    }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner))
            return;
        Player player = Owner.Player;
        object obj;
        if (player == null)
        {
            obj = null;
        }
        else
        {
            PlayerCombatState playerCombatState = player.PlayerCombatState;
            obj = playerCombatState?.AllCards;
        }
        obj ??= Array.Empty<CardModel>();
        foreach (CardModel card in (IEnumerable<CardModel>) obj)
        {
            if (card.Affliction is Smog)
                CardCmd.ClearAffliction(card);
        }
        await PowerCmd.Remove(this);
    }

    public override bool ShouldPlay(CardModel card, AutoPlayType _)
    {
        return card.Owner != this.Owner.Player || !(card.Affliction is Smog);
    }
}