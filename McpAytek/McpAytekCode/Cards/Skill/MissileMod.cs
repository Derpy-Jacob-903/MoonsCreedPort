using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace McpAytek.McpAytekCode.Cards;

//Ordnance
public class MissileMod() : AytekCard(0,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(4m, ValueProp.Move),
        new CardsVar(1)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        
        await CreatureCmd.GainBlock(Owner.Creature, base.DynamicVars.Block, play);
        CardPile pile = PileType.Hand.GetPile(Owner);
        for (int i = 0; i < DynamicVars.Cards.BaseValue; i++)
        {
            CardModel card = Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards.Where(c => c.IsUpgradable && (this.IsUpgraded || c.Tags.Contains(AytekStuff.Missile))));
            if (card == null) return;
            CardCmd.Upgrade(card);
        }
    }
    //protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1M);
}