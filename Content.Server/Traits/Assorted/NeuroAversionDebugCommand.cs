// SPDX-FileCopyrightText: 2026 TrixxedHeart <46364955+TrixxedBit@users.noreply.github.com>
// SPDX-License-Identifier: MIT
using Content.Shared.Traits.Assorted;
using Robust.Shared.Console;

namespace Content.Server.Traits.Assorted;

public sealed class NeuroAversionDebugCommand : IConsoleCommand
{
    public string Command => "neurodebug";
    public string Description => "Prints debug info for NeuroAversionComponent on yourself or a specified entity.";
    public string Help => "Usage: neurodebug [entityUid]";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var entMan = IoCManager.Resolve<IEntityManager>();
        EntityUid target;

        if (args.Length > 0 && EntityUid.TryParse(args[0], out var parsed))
        {
            target = parsed;
        }
        else
        {
            var player = shell.Player;
            if (player == null || !player.AttachedEntity.HasValue)
            {
                shell.WriteLine("No entity specified and you have no attached mob.");
                return;
            }
            target = player.AttachedEntity.Value;
        }

        if (!entMan.TryGetComponent(target, out NeuroAversionComponent? comp))
        {
            shell.WriteLine($"Entity {target} does not have NeuroAversionComponent.");
            return;
        }

        float build = comp.SeizureBuild;
        float threshold = comp.SeizureThreshold;
        float chance = threshold > 0f ? build / threshold : 0f;
        bool mindshield = comp.IsMindShielded;
        bool startedMindshield = comp.StartedMindShielded;
        float goodMult = comp.ConditionGoodMultiplier;
        float okayMult = comp.ConditionOkayMultiplier;
        float badMult = comp.ConditionBadMultiplier;
        float critMult = comp.ConditionCriticalMultiplier;
        float startMult = comp.StartedMindShieldedMultiplier;
        float midMult = comp.MidRoundMindShieldedMultiplier;
        float baseBuild = comp.BaseSeizurePassivePerSec;
        float postSeizure = comp.PostSeizureResidual;

        shell.WriteLine($"NeuroAversion Debug for {target}:");
        shell.WriteLine($"  SeizureBuild: {build:F2} / {threshold} ({chance:P1} chance per roll)");
        shell.WriteLine($"  IsMindShielded: {mindshield}");
        shell.WriteLine($"  StartedMindShielded: {startedMindshield}");
        shell.WriteLine($"  BaseSeizurePassivePerSec: {baseBuild}");
        shell.WriteLine($"  PostSeizureResidual: {postSeizure}");
        shell.WriteLine($"  Condition Multipliers: Good={goodMult}, Okay={okayMult}, Bad={badMult}, Critical={critMult}");
        shell.WriteLine($"  Mindshield Multipliers: Started={startMult}, MidRound={midMult}");
    }
}
