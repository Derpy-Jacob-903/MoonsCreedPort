using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

namespace McpAytek.McpAytekCode.Cards;

public class BalancedLoad() : AytekCard(0,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "beta_art_7.png".CardImagePath();
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Retain];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(2),
        new CardsVar(2)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (!MyShouldGlowGoldInternal) return;
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        await CommonActions.Draw(this, context);
    }
    protected override void OnUpgrade()
    {
        this.DynamicVars.Energy.UpgradeValueBy(1M);
        this.DynamicVars.Cards.UpgradeValueBy(1M);
    }
    
    protected override bool ShouldGlowGoldInternal => base.ShouldGlowGoldInternal && MyShouldGlowGoldInternal;
    protected bool MyShouldGlowGoldInternal => WasMissilePlayed(this) && WasGunPlayed(this);
    private bool WasMissilePlayed(CardModel c) => CombatManager.Instance.History.CardPlaysStarted
        .LastOrDefault(e =>
            e.CardPlay.Card.Owner == Owner &&
            e.CardPlay.Card != c)?.CardPlay.Card.Tags.Contains(AytekStuff.Missile) ?? false;
    private bool WasGunPlayed(CardModel c) => CombatManager.Instance.History.CardPlaysStarted
        .LastOrDefault(e =>
            e.CardPlay.Card.Owner == Owner &&
            e.CardPlay.Card != c)?.CardPlay.Card.Keywords.Contains(AytekStuff.GunKeyword) ?? false;
}