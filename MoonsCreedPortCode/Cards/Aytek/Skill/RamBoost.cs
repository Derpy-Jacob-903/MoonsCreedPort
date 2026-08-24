using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class RamBoost() : AytekCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    public override bool HasStarCostX => true;
    public override string CustomPortraitPath => "beta_art_1.png".CardImagePath();

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<VigorPower>(0)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await PowerCmd.Apply<VigorPower>(context, Owner.Creature, ResolveEnergyXValue() + DynamicVars["VigorPower"].BaseValue,
            Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.DynamicVars["VigorPower"].UpgradeValueBy(3M);
}