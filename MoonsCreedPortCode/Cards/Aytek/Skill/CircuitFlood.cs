using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using Haunt = MoonsCreedPort.MoonsCreedPortCode.Cards.Polarix.Haunt;
using Void = MegaCrit.Sts2.Core.Models.Cards.Void;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class CircuitFlood() : AytekCard(0,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [ HoverTipFactory.FromCard<Void>() ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        if (IsUpgraded) return;
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Void>(Owner), PileType.Draw, Owner));
        await Cmd.Wait(0.5f);
    }
}