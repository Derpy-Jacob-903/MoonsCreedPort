using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

public class GunOverhaulPower : MoonsCreedPortPower
{
    public override PowerType Type => PowerType.Buff;
 
   public override PowerStackType StackType => PowerStackType.Counter;

   protected override IEnumerable<DynamicVar> CanonicalVars => [ 
     new CalculationBaseVar(0M),
     new CalculationExtraVar(3M),
     new CalculatedVar("CalculatedHits").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => PileType.Hand.GetPile(card.Owner).Cards.Count<CardModel>((Func<CardModel, bool>) (c => c.Tags.Contains(AytekStuff.Missile)))))
   ];

   public override async Task BeforeSideTurnEnd(
     PlayerChoiceContext choiceContext,
     CombatSide side,
     IEnumerable<Creature> participants)
   {
     if (Owner.Player != null && !participants.Contains(Owner))
       return;
     Flash();
     for (int i = 0; i < ((CalculatedVar)DynamicVars["CalculatedHits"]).Calculate(null); i++)
     {
       await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, Amount, ValueProp.Unpowered, Owner);
     }
   }
}