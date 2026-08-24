using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Character.Echo;
using MoonsCreedPort.MoonsCreedPortCode.Character.Polarix;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Polarix;

public class VoidFlame() : PolarixCard(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(7m, ValueProp.Move)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        VoidFlame card1 = this;
        ArgumentNullException.ThrowIfNull((object) play.Target, "cardPlay.Target");
        var list = PileType.Hand.GetPile(card1.Owner).Cards.ToList<CardModel>();
        var cardCount = list.Count;
        foreach (CardModel card2 in list)
            await CardCmd.Exhaust(context, card2);
        var scale = 0.8f;
        AttackCommand attackCommand = await DamageCmd.Attack(card1.DynamicVars.Damage.BaseValue).WithHitCount(cardCount).FromCard((CardModel) card1, play).Targeting(play.Target).BeforeDamage((Func<Task>) (() =>
        {
            NGroundFireVfx child = NGroundFireVfx.Create(play.Target);
            if (child == null)
                return Task.CompletedTask;
            SfxCmd.Play("event:/sfx/characters/attack_fire");
            child.Scale = Vector2.One * scale;
            NCombatRoom instance = NCombatRoom.Instance;
            if (instance != null)
                instance.CombatVfxContainer.AddChildSafely((Node) child);
            scale += 0.1f;
            return Task.CompletedTask;
        })).Execute(context);
    }

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}