using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace McpAytek.McpAytekCode.Cards;

public class LaunchProtocol() : AytekCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        ..MakeCalculatedBlock(8, (card, c) => (CombatManager.Instance.History.CardPlaysStarted
            .LastOrDefault(e =>
                e.CardPlay.Card.Owner == card.Owner &&
                e.CardPlay.Card != card)?.CardPlay.Card.Tags.Contains(AytekStuff.Missile) ?? false) ? 1 : 0, 4)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, new BlockVar(DynamicVars.CalculatedBlock.BaseValue, DynamicVars.CalculatedBlock.Props), play);
    }
    
    private bool WasLastCardPlayedSkill(CardModel c) => CombatManager.Instance.History.CardPlaysStarted
        .LastOrDefault(e =>
            e.CardPlay.Card.Owner == Owner &&
            e.CardPlay.Card != c)?.CardPlay.Card.Tags.Contains(AytekStuff.Missile) ?? false;
    
    protected override bool ShouldGlowGoldInternal => base.ShouldGlowGoldInternal && WasLastCardPlayedSkill(this);

    protected override void OnUpgrade() => this.DynamicVars.CalculationBase.UpgradeValueBy(3M);
}