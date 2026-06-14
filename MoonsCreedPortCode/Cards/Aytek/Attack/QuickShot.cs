using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class QuickShot() : AytekCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        AytekStuff.GunKeyword
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(1.4m, ValueProp.Move),
        new RepeatVar(3),
        new CardsVar(1)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(DynamicVars.Repeat.IntValue)
                .FromCard(this).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
        if (WasLastCardPlayedGun)
            await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, Owner);
    }
    
    private bool WasLastCardPlayedGun => CombatManager.Instance.History.CardPlaysStarted
        .LastOrDefault(e =>
            e.CardPlay.Card.Owner == Owner &&
            e.CardPlay.Card != this) != null && CombatManager.Instance.History.CardPlaysStarted
        .LastOrDefault(e =>
            e.CardPlay.Card.Owner == Owner &&
            e.CardPlay.Card != this)!
        .CardPlay.Card.Keywords.Contains(AytekStuff.GunKeyword)
    ;
    protected override bool ShouldGlowGoldInternal => base.ShouldGlowGoldInternal && WasLastCardPlayedGun;
    protected override void OnUpgrade()
    {
        this.DynamicVars.Repeat.UpgradeValueBy(2M);
        this.DynamicVars.Cards.UpgradeValueBy(1M);
    }
}