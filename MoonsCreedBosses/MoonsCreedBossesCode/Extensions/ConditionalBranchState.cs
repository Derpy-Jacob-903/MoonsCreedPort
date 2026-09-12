using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Random;

namespace MoonsCreedBosses.MoonsCreedBossesCode.Extensions;

/// <summary>
/// A state that selects the next state based on a custom function.
/// Useful for complex move patterns that can't be expressed with RandomBranchState.
/// </summary>
public class ConditionalBranchState(
    string stateId,
    Func<Creature, Rng, MonsterMoveStateMachine, string> selectNextState)
    : MonsterState
{
    public override string Id => stateId;
    public override bool ShouldAppearInLogs => false;

    public override string GetNextState(Creature owner, Rng rng)
    {
        return selectNextState(owner, rng, owner.Monster.MoveStateMachine);
    }
    
    public override void RegisterStates(Dictionary<string, MonsterState> monsterStates)
    {
        monsterStates.Add(Id, this);
    }
}