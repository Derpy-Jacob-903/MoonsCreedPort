using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

public class BlockProtocolPower : MoonsCreedPortPower
{
    public override PowerType Type => PowerType.Buff;
 
   public override PowerStackType StackType => PowerStackType.Counter;

   protected override object InitInternalData() => new Data();

   public override Task BeforeCardPlayed(CardPlay cardPlay)
   {
     if (!cardPlay.Card.IsUpgraded || cardPlay.Card.Owner.Creature != Owner) return Task.CompletedTask;
     GetInternalData<Data>().amountsForPlayedCards.Add(cardPlay.Card, Amount);
     return Task.CompletedTask;
   }

   public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
   {
     if (!cardPlay.Card.IsUpgraded) return;
     if (cardPlay.Card.Owner.Creature != Owner || !GetInternalData<Data>().amountsForPlayedCards.Remove(cardPlay.Card, out var amount) || amount <= 0)
       return;
     await CreatureCmd.GainBlock(Owner, amount, ValueProp.Unpowered, (CardPlay) null, true);
   }

   private class Data
   {
     /// <summary>
     /// Keep track of the cards we've seen played and the power amount at the time they were played.
     /// This lets After Image avoid triggering on cards that started play before it was applied, and avoid gaining
     /// extra block on multiple plays of After Image.
     /// </summary>
     public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
   }
}