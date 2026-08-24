using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

public class LogicLoopholePower : MoonsCreedPortPower
{
    public override PowerType Type => PowerType.None;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterStarsGained(int amount, Player gainer)
    {
        await PowerCmd.Apply<StarNextTurnPower>(new ThrowingPlayerChoiceContext(), gainer.Creature, amount, null, null);
    }
}