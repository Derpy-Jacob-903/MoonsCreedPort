using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace McpPolarix.McpPolarixCode.Cards;

public class SebCardPolarix() : PolarixCard(1,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self)
{
    private int _currentVuln = 1;
    private int _increasedVuln;
    [SavedProperty]
    public int CurrentVuln
    {
        get => _currentVuln;
        set
        {
            AssertMutable();
            _currentVuln = value;
            DynamicVars.Damage.BaseValue = _currentVuln;
        }
    }
    [SavedProperty]
    public int IncreasedVuln
    {
        get => _increasedVuln;
        set
        {
            AssertMutable();
            _increasedVuln = value;
        }
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<VulnerablePower>(1), new PowerVar<VulnerablePower>("VulnUp", 1), new MaxHpVar(2)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
            await PowerCmd.Apply<VulnerablePower>(context, play.Target, DynamicVars.Vulnerable.BaseValue,
                Owner.Creature, this);
        var intValue = DynamicVars["Increase"].IntValue;
        BuffFromPlay(intValue);
        if (DeckVersion is not SebCardPolarix deckVersion)
            return;
        deckVersion.BuffFromPlay(intValue);
    }
    protected override void AfterDowngraded() => UpdateDamage();

    private void BuffFromPlay(int extraDamage)
    {
        IncreasedVuln += extraDamage;
        UpdateDamage();
    }

    private void UpdateDamage() => CurrentVuln = 1 + IncreasedVuln;
    protected override void OnUpgrade() => this.DynamicVars.MaxHp.UpgradeValueBy(-1M);
}