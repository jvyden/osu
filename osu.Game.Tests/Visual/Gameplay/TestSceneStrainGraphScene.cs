// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Difficulty.Skills;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Osu.Difficulty;
using osu.Game.Tests.Beatmaps;
using osuTK;

namespace osu.Game.Tests.Visual.Gameplay
{
    [TestFixture]
    public partial class TestSceneStrainGraphScene : OsuTestScene
    {
        private BarGraph graph = null!;
        private Ruleset ruleset = null!;
        private OsuDifficultyCalculator calculator = null!;
        private Dictionary<StrainSkill, List<double>> strainGraphs = null!;

        [SetUpSteps]
        public void SetUpSteps()
        {
            AddStep("create graph", () => Child = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(300, 150),
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = OsuColour.Gray(0.2f)
                    },
                    graph = new BarGraph
                    {
                        RelativeSizeAxes = Axes.Both,
                    }
                }
            });
            AddStep("create difficulty calculator", () =>
            {
                ruleset = new OsuRuleset();
                calculator = (OsuDifficultyCalculator)ruleset.CreateDifficultyCalculator(new TestWorkingBeatmap(new TestBeatmap(ruleset.RulesetInfo)));
            });
        }

        [Test]
        public void TestStrainGraph()
        {
            AddStep("calculate strain", () =>
            {
                strainGraphs = calculator.CalculateStrain();
            });
            AddAssert("has graphs", () => strainGraphs.Count != 0);
            AddAssert("any graph has values", () => strainGraphs.Sum(g => g.Value.Count) != 0);
            AddStep("add values to graph", () =>
            {
                graph.Values = strainGraphs.First().Value.Select(v => (float)v);
            });
        }
    }
}
