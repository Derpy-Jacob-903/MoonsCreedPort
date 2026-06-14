using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class LaunchProtocol() : AytekCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(6m, ValueProp.Move),
        new BlockVar("Block2", 9m, ValueProp.Move),
        new DynamicVar("ExtraBlock", 3m)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, WasLastCardPlayedSkill ? (BlockVar)base.DynamicVars["Block2"] : base.DynamicVars.Block, play);
    }
    
    private bool WasLastCardPlayedSkill => CombatManager.Instance.History.CardPlaysStarted
        .LastOrDefault(e =>
            e.CardPlay.Card.Owner == Owner &&
            e.CardPlay.Card != this)?.CardPlay.Card.Tags.Contains(AytekStuff.Missile)??false;
    
    protected override bool ShouldGlowGoldInternal => base.ShouldGlowGoldInternal && WasLastCardPlayedSkill;

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(2M);
}