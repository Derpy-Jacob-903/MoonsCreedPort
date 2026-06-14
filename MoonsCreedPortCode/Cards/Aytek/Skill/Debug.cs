using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class Debug() : AytekCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Cleanse", -10),
        new TechPointVar(1)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        var p = Owner.Creature;
        if (p.HasPower<VulnerablePower>())
        {
            await PowerCmd.ModifyAmount(context, p.GetPower<VulnerablePower>(), 
                DynamicVars["Cleanse"].BaseValue, null, this);
            await PlayerCmd.GainStars(DynamicVars["TechPointVar"].BaseValue, Owner);
        }
        if (p.HasPower<WeakPower>())
        {
            await PowerCmd.ModifyAmount(context, p.GetPower<WeakPower>(), 
                DynamicVars["Cleanse"].BaseValue, null, this);
            await PlayerCmd.GainStars(DynamicVars["TechPointVar"].BaseValue, Owner);
        }
        if (p.HasPower<FrailPower>())
        {
            await PowerCmd.ModifyAmount(context, p.GetPower<FrailPower>(), 
                DynamicVars["Cleanse"].BaseValue, null, this);
            await PlayerCmd.GainStars(DynamicVars["TechPointVar"].BaseValue, Owner);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars["Cleanse"].UpgradeValueBy(-5M);
}