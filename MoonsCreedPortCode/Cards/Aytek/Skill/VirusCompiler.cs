using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class VirusCompiler() : AytekCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "beta_art_8.png".CardImagePath();
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<VulnerablePower>(1),
        ..MakeCalculatedVar("CalcStars", 0, (model, creature) => creature != null && creature.HasPower<VulnerablePower>() ? creature.GetPowerAmount<VulnerablePower>() : 0)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target == null) return;
        await CommonActions.Apply<VulnerablePower>(context, play.Target, this);
        await PlayerCmd.GainStars(((CalculatedVar)DynamicVars["CalculatedStars"]).Calculate(null), Owner);
    }
    protected override void OnUpgrade()
    {
        this.DynamicVars.Vulnerable.UpgradeValueBy(1M);
    }
}