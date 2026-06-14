using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers;
// ReSharper disable MemberCanBePrivate.Global

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class PrecisionMissile() : AytekCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    private int _currentDamage = 6;
    private int _increasedDamage;
    [SavedProperty]
    public int CurrentDamage
    {
        get => _currentDamage;
        set
        {
            AssertMutable();
            _currentDamage = value;
            DynamicVars.Damage.BaseValue = _currentDamage;
        }
    }
    [SavedProperty]
    public int IncreasedDamage
    {
        get => _increasedDamage;
        set
        {
            AssertMutable();
            _increasedDamage = value;
        }
    }
    protected override HashSet<CardTag> CanonicalTags => [AytekStuff.Missile];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(this.CurrentDamage, ValueProp.Move),
        new IntVar("Increase", 1M)
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords=>
    [
        CardKeyword.Exhaust
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(this).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
        var intValue = DynamicVars["Increase"].IntValue;
        BuffFromPlay(intValue);
        if (DeckVersion is not PrecisionMissile deckVersion)
            return;
        deckVersion.BuffFromPlay(intValue);
    }
    protected override void OnUpgrade() => DynamicVars["Increase"].UpgradeValueBy(1M);

    protected override void AfterDowngraded() => UpdateDamage();

    private void BuffFromPlay(int extraDamage)
    {
        IncreasedDamage += extraDamage;
        UpdateDamage();
    }

    private void UpdateDamage() => CurrentDamage = 1 + IncreasedDamage;
}