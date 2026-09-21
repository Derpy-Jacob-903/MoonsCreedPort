using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace MoonsCreedBosses.MoonsCreedBossesCode.Extensions;

public abstract class ZodiacEncounterModel() : CustomEncounterModel(RoomType.Boss)
{
    public enum Element
    {
        Blank,
        Air,
        Earth,
        Fire,
        Water,
        Chaos
    }

    public virtual Element myElement => Element.Blank;
    public int actNumber;
    public override bool IsValidForAct(ActModel act) => act.ActNumber() == actNumber;
}